#if UNITY_ANDROID
using System;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Appmetr.Unity.Json;

namespace Appmetr.Unity.Impl
{
    public class AppmetrPluginAndroid
    {
        private static AndroidJavaClass _clsAppMetr;
        private static AndroidJavaClass _clsAppMetrHelper;
        private static AndroidJavaClass _clsUnityPlayer;
        private static readonly object AppMetrMutex = new object();
        private static readonly ConcurrentQueue<Action> AppMetrActionQueue = new ConcurrentQueue<Action>();
        private static bool _appMetrThreadInitialized;
        private static bool _jniAttached;
        private static bool _jniPaused;
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

        public static void SetupWithToken(string token, string platform)
        {
            AndroidJNI.AttachCurrentThread();
            if (_clsUnityPlayer == null)
            {
                _clsUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            }
            var currentActivity = _clsUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            if (_clsAppMetr == null)
            {
                _clsAppMetr = new AndroidJavaClass("com.appmetr.android.AppMetr");
            }
            if (_clsAppMetrHelper == null)
            {
                _clsAppMetrHelper = new AndroidJavaClass("com.appmetr.android.integration.AppMetrHelper");
            }
            lock (AppMetrMutex)
            {
                currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    lock (AppMetrMutex)
                    {
                        var macAddress = GetMacAddress(currentActivity);
                        _clsAppMetr.CallStatic("setup", currentActivity, token, macAddress);
                        _instanceIdentifier = _clsAppMetr.CallStatic<string>("getInstanceIdentifier");
                        currentActivity.Dispose();
                        Monitor.Pulse(AppMetrMutex);
                    }
                }));
                Monitor.Wait(AppMetrMutex);
            }
            if (_appMetrThreadInitialized)
            {
                return;
            }

            var jniThread = new Thread(() =>
            {
                while (Thread.CurrentThread.IsAlive)
                {
                    lock (AppMetrMutex)
                    {
                        while (AppMetrActionQueue.Count <= 0 || _jniPaused)
                        {
                            Monitor.Wait(AppMetrMutex);
                        }
                    }
                    try
                    {
                        if (!_jniAttached)
                        {
                            AndroidJNI.AttachCurrentThread();
                            _jniAttached = true;
                        }

                        Action action;
                        while (AppMetrActionQueue.TryDequeue(out action))
                        {
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
                        
                    if (_jniPaused && _jniAttached)
                    {
                        AndroidJNI.DetachCurrentThread();
                        _jniAttached = false;
                    }
                }

                if (_jniAttached)
                {
                    AndroidJNI.DetachCurrentThread();
                    _jniAttached = false;
                }
            }) {Name = "Appmetr Jni"};
            jniThread.Start();
            _appMetrThreadInitialized = true;
        }

        private static string GetMacAddress(AndroidJavaObject currentActivity)
        {
            var macAddr = string.Empty;
#if !APPMETR_DISABLE_MAC_SENDING
            const string WifiContext = "wifi";
            
            var wifiManager = currentActivity.Call<AndroidJavaObject>("getSystemService", WifiContext);
            if (wifiManager == null)
            {
                return macAddr;
            }
            var connectionInfo = wifiManager.Call<AndroidJavaObject>("getConnectionInfo");
            if (connectionInfo == null)
            {
                return macAddr;
            }
            
            macAddr = connectionInfo.Call<string>("getMacAddress");
            macAddr = Regex.Replace(macAddr, @"\W", "").ToUpper(CultureInfo.GetCultureInfo("en-US"));
#endif
            return macAddr;
        }        

        public static void OnPause()
        {
            DispatchJni(() => { 
                _jniPaused = true;
                _clsAppMetr.CallStatic("onPause");
            });
        }

        public static void OnResume()
        {
            _jniPaused = false;
            DispatchJni(() => {
                var activity = _clsUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                _clsAppMetr.CallStatic("onResume", activity);
                activity.Dispose();
            });
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

        public static void AttachProperties()
        {
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("attachProperties"); });
        }

        public static void AttachProperties(IDictionary<string, object> properties)
        {
            var propertiesJson = ToJson(properties);
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("attachProperties", propertiesJson); });
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

        public static void AttachEntityAttributes(string entityName, string entityValue, IDictionary<string, object> properties)
        {
            var propertiesJson = ToJson(properties);
            DispatchJni(() => { _clsAppMetrHelper.CallStatic("attachEntityAttributes", entityName, entityValue, propertiesJson); });
        }

        public static void Flush()
        {
            DispatchJni(() => { _clsAppMetr.CallStatic("flush"); });
        }
        
        public static void FlushLocal()
        {
            DispatchJni(() => { _clsAppMetr.CallStatic("flushLocal"); });
        }

        public static string GetInstanceIdentifier()
        {
            return _instanceIdentifier;
        }

        public static string GetDeviceKey()
        {
            if (_clsAppMetrHelper == null) return null;
            return _clsAppMetrHelper.CallStatic<string>("getDeviceKey"); 
        }

        private static void DispatchJni(Action action)
        {
            if (action == null) return;
            AppMetrActionQueue.Enqueue(action);
            if (Monitor.TryEnter(AppMetrMutex))
            {
                Monitor.PulseAll(AppMetrMutex);
                Monitor.Exit(AppMetrMutex);
            }
        }
    }
}

#endif