﻿#if UNITY_ANDROID
using System;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using Appmetr.Unity.Json;

namespace Appmetr.Unity.Impl
{
    public class AppmetrPluginAndroid
    {
        private class AppMetrCommandListener : AndroidJavaProxy
        {
            // GameObject name for UnitySendMessage
            private readonly string _commandListenerName;

            // Android interface implementation
            public AppMetrCommandListener(string commandListenerName) : base("com.appmetr.android.AppMetrListener")
            {
                _commandListenerName = commandListenerName;
            }

            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once InconsistentNaming
            public void executeCommand(AndroidJavaObject command)
            {
                if (!string.IsNullOrEmpty(_commandListenerName))
                {
                    var listenerObject = GameObject.Find(_commandListenerName);
                    if (listenerObject != null)
                    {
                        listenerObject.SendMessage("OnAppMetrCommand", command.Call<string>("toString"));
                    }
                }
            }
        }

        private static AndroidJavaClass _clsAppMetr;
        private static AndroidJavaClass _clsAppMetrHelper;
        private static readonly object AppMetrMutex = new object();
        private static readonly Queue<Action> AppMetrActionQueue = new Queue<Action>();
        private static bool _appMetrThreadInitialized;
        private static string _instanceIdentifier = "";

        private static string ToJson(IDictionary<string, object> properties)
        {
            return Serializer.Serialize(properties);
        }

        private static string ToJson(IDictionary<string, object>[] properties)
        {
            var propertiesList = new List<object>(properties);
            return Serializer.Serialize(propertiesList);
        }

        public static void SetupWithToken(string token, string commandListenerName)
        {
            AndroidJNI.AttachCurrentThread();
            var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            if (_clsAppMetr == null)
            {
                _clsAppMetr = new AndroidJavaClass("com.appmetr.android.AppMetr");
            }
            if (_clsAppMetrHelper == null)
            {
                _clsAppMetrHelper = new AndroidJavaClass("com.appmetr.android.integration.AppMetrHelper");
            }
            var context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaProxy listener = string.IsNullOrEmpty(commandListenerName)
                ? null
                : new AppMetrCommandListener(commandListenerName);
            lock (AppMetrMutex)
            {
                currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    lock (AppMetrMutex)
                    {
                        _clsAppMetr.CallStatic("setup", token, context, listener);
                        _instanceIdentifier = _clsAppMetr.CallStatic<string>("getInstanceIdentifier");
                        context.Dispose();
                        Monitor.Pulse(AppMetrMutex);
                    }
                }));
                Monitor.Wait(AppMetrMutex);
            }
            currentActivity.Dispose();
            activityClass.Dispose();
            if (_appMetrThreadInitialized)
            {
                return;
            }

            var jniThread = new Thread(() =>
            {
                AndroidJNI.AttachCurrentThread();
                while (Thread.CurrentThread.IsAlive)
                {
                    lock (AppMetrMutex)
                    {
                        try
                        {
                            while (AppMetrActionQueue.Count <= 0)
                            {
                                Monitor.Wait(AppMetrMutex);
                            }
                            while (AppMetrActionQueue.Count > 0)
                            {
                                var action = AppMetrActionQueue.Dequeue();
                                if (action != null)
                                {
                                    action();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }
                AndroidJNI.DetachCurrentThread();
            }) {Name = "Appmetr Jni"};
            jniThread.Start();
            _appMetrThreadInitialized = true;
        }

        public static void OnPause()
        {
            DispatchJni(() => { _clsAppMetr.CallStatic("onPause"); });
        }

        public static void OnResume()
        {
            DispatchJni(() => { _clsAppMetr.CallStatic("onResume"); });
        }

        public static void TrackSession()
        {
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackSession"); });
        }

        public static void TrackSession(IDictionary<string, object> properties)
        {
            var propertiesJson = ToJson(properties);
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackSession", propertiesJson); });
        }

        public static void TrackLevel(int level)
        {
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackLevel", level); });
        }

        public static void TrackLevel(int level, IDictionary<string, object> properties)
        {
            var propertiesJson = ToJson(properties);
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackLevel", level, propertiesJson); });
        }

        public static void TrackEvent(string eventName)
        {
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackEvent", eventName); });
        }

        public static void TrackEvent(string eventName, IDictionary<string, object> properties)
        {
            var propertiesJson = ToJson(properties);
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackEvent", eventName, propertiesJson); });
        }

        public static void TrackPayment(IDictionary<string, object> payment)
        {
            var paymentJson = ToJson(payment);
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackPayment", paymentJson); });
        }

        public static void TrackPayment(IDictionary<string, object> payment, IDictionary<string, object> properties)
        {
            var paymentJson = ToJson(payment);
            var propertiesJson = ToJson(properties);
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackPayment", paymentJson, propertiesJson); });
        }

        public static void TrackAdsEvent(string eventName)
        {
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackAdsEvent", eventName); });
        }

        public static void AttachProperties()
        {
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("attachProperties"); });
        }

        public static void AttachProperties(IDictionary<string, object> properties)
        {
            var propertiesJson = ToJson(properties);
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("attachProperties", propertiesJson); });
        }

        public static void TrackOptions(string commandId, IDictionary<string, object>[] options)
        {
            var optionsJson = ToJson(options);
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackOptions", commandId, optionsJson); });
        }

        public static void TrackOptionsError(string commandId, IDictionary<string, object>[] options, string code,
            string message)
        {
            var optionsJson = ToJson(options);
            DispatchJni(() =>
            {
                _clsAppMetrHelper.CallStatic("trackOptionsError", commandId, optionsJson, code, message);
            });
        }

        public static void TrackExperimentStart(string experiment, string groupId)
        {
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackExperimentStart", experiment, groupId); });
        }

        public static void TrackExperimentEnd(string experiment)
        {
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackExperimentEnd", experiment); });
        }

        public static bool VerifyIosPayment(string productId, string transactionId, string receipt, string privateKey)
        {
            return false;
        }

        public static void VerifyAndroidPayment(string purchaseInfo, string signature, string privateKey, Action<bool> callback)
        {
            DispatchJni(() =>
            {
                var result = _clsAppMetr.CallStatic<bool>("verifyPayment", purchaseInfo, signature, privateKey);
                if (callback != null)
                {
                    callback(result);
                }
            });
        }

        public static void TrackState(IDictionary<string, object> state)
        {
            var stateJson = ToJson(state);
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("trackState", stateJson); });
        }

        public static void Identify(string userId)
        {
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("identify", userId); });
        }

        public static void Flush()
        {
            DispatchJni(() => { _clsAppMetr.CallStatic("flush"); });
        }

        public static string GetInstanceIdentifier()
        {
            return _instanceIdentifier;
        }

        private static void DispatchJni(Action action)
        {
            if (action == null) return;
            lock (AppMetrMutex)
            {
                AppMetrActionQueue.Enqueue(action);
                Monitor.Pulse(AppMetrMutex);
            }
        }
    }
}

#endif