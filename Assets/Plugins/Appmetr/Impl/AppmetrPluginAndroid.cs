#if UNITY_ANDROID
using UnityEngine;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JsonFx.Json;

public class AppmetrPluginAndroid
{
	private static bool initialized = false;

	private static AndroidJavaObject currentActivity = null;
	
	private static AndroidJavaClass clsAppMetr = null;
	private static AndroidJavaClass clsAppMetrHelper = null;
	
	private static AndroidJavaClass AppMetr
	{
		get
		{
			getActivity();
			if (clsAppMetr == null)
			{
				clsAppMetr = new AndroidJavaClass("com.appmetr.android.AppMetr");
			}
			return clsAppMetr;
		}
	}
	
	private static AndroidJavaClass AppMetrHelper
	{
		get
		{
			getActivity();
			if (clsAppMetrHelper == null)
			{
				clsAppMetrHelper = new AndroidJavaClass("com.appmetr.android.integration.AppMetrHelper");
			}
			return clsAppMetrHelper;
		}
	}
	
	private static void getActivity()
	{
		if (currentActivity == null)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		}
	}

	private static string ToJson(IDictionary<string, object> properties) 
	{
		var json = new StringBuilder ();

		var writer = new JsonWriter (json);
		writer.Write(properties);

		return json.ToString ();
	}

	private static string ToJson(IDictionary<string, object>[] properties) 
	{
			var json = new StringBuilder ();

			var writer = new JsonWriter (json);
			writer.Write(properties);

			return json.ToString ();
	}
	
	public static void SetupWithToken(string token)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			UnityEngine.AndroidJNI.AttachCurrentThread();
		}
		
		getActivity();
		
		AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
		currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
        	AndroidJavaObject listener = new AndroidJavaObject("com.appmetr.unity.AppMetrListenerImpl");
            AppMetr.CallStatic("setup", token, context, listener);
            initialized = true;
        }));
	}

	//Bad code, smells like shit, but what can we do? 
	private static void waitForInitialize() 
	{
		while (!initialized) 
		{
			Thread.Sleep(10);
		}
	}

	public static void TrackSession()
	{
		waitForInitialize();	
		AppMetrHelper.CallStatic("trackSession");
	}

	public static void TrackSession(IDictionary<string, object> properties)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("trackSession", ToJson(properties));
	}

	public static void TrackLevel(int level)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("trackLevel", level);
	}

	public static void TrackLevel(int level, IDictionary<string, object> properties)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("trackLevel", level, ToJson(properties));
	}

	public static void TrackEvent(string eventName)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("trackEvent", eventName);
	}

	public static void TrackEvent(string eventName, IDictionary<string, object> properties)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("trackEvent", eventName, ToJson(properties));
	}

	public static void TrackPayment(IDictionary<string, object> payment)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("trackPayment", ToJson(payment));
	}

	public static void TrackPayment(IDictionary<string, object> payment, IDictionary<string, object> properties)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("trackPayment", ToJson(payment), ToJson(properties));
	}
	
	public static void AttachProperties()
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("attachProperties");
	}
	
	public static void AttachProperties(IDictionary<string, object> properties)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("attachProperties", ToJson(properties));
	}

	public static void TrackOptions(string commandId, IDictionary<string, object>[] options)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("trackOptions", commandId, ToJson(options));
	}

	public static void TrackOptionsError(string commandId, IDictionary<string, object>[] options, string code, string message)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("trackOptionsError", commandId, ToJson(options), code, message);
	}

	public static void TrackExperimentStart(string experiment, string groupId)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("trackExperimentStart", experiment, groupId);
	}

	public static void TrackExperimentEnd(string experiment)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("trackExperimentEnd", experiment);
	}

	public static void Identify(string userId)
	{
		waitForInitialize();
		AppMetrHelper.CallStatic("identify", userId);
	}

	public static void Flush()
	{
		waitForInitialize();
		AppMetr.CallStatic("flush");
	}

	public static string GetInstanceIdentifier()
	{
		waitForInitialize();
		return AppMetr.CallStatic<string>("getInstanceIdentifier");
	}
}
#endif
