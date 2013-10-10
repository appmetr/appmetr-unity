#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AppmetrPluginAndroid
{
	private static AndroidJavaObject currentActivity = null;
	
	private static AndroidJavaClass clsConnect = null;
	private static AndroidJavaClass clsConnectHelper = null;
	private static AndroidJavaClass clsConnectImpl = null;
	
	private static AndroidJavaClass Connect
	{
		get
		{
			getActivity();
			if (clsConnect == null)
			{
				clsConnect = new AndroidJavaClass("com.appmetr.android.AppMetr");
			}
			return clsConnect;
		}
	}

	private static AndroidJavaObject connectInstance;
	private static AndroidJavaObject ConnectInstance
	{
		get
		{
			getActivity();
			if (connectInstance == null)
			{
				connectInstance = Connect.CallStatic<AndroidJavaObject>("getInstance");
			}
			return connectInstance;
		}
	}
	
	private static AndroidJavaClass ConnectHelper
	{
		get
		{
			getActivity();
			if (clsConnectHelper == null)
			{
				clsConnectHelper = new AndroidJavaClass("com.appmetr.android.integration.AppMetrHelper");
			}
			return clsConnectHelper;
		}
	}

	private static AndroidJavaClass ConnectImpl
	{
		get
		{
			getActivity();
			if (clsConnectImpl == null)
			{
				clsConnectImpl = new AndroidJavaClass("com.appmetr.android.impl.AppMetrImpl");
			}
			return clsConnectImpl;
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
            ConnectImpl.CallStatic("setup", token, context);
        }));
	}

	public static void TrackSession()
	{
		ConnectImpl.CallStatic("trackSession");
	}

	public static void TrackSession(IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in properties)
		{
			ConnectImpl.CallStatic("setKey", pair.Key, pair.Value);
		}
		ConnectImpl.CallStatic("trackSessionWithProperties");
	}

	public static void TrackLevel(int level)
	{
		ConnectImpl.CallStatic("trackLevel", level);
	}

	public static void TrackLevel(int level, IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in properties)
		{
			ConnectImpl.CallStatic("setKey", pair.Key, pair.Value);
		}
		ConnectImpl.CallStatic("trackLevelWithProperties", level);
	}

	public static void TrackEvent(string eventName)
	{
		ConnectImpl.CallStatic("trackEvent", eventName);
	}

	public static void TrackEvent(string eventName, IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in properties)
		{
			ConnectImpl.CallStatic("setKey", pair.Key, pair.Value);
		}
		ConnectImpl.CallStatic("trackEventWithProperties", eventName);
	}

	public static void TrackPayment(IDictionary<string, string> payment)
	{
		foreach (KeyValuePair<string, string> pair in payment)
		{
			ConnectImpl.CallStatic("setKey", pair.Key, pair.Value);
		}
		ConnectImpl.CallStatic("trackPayment");
	}

	public static void TrackPayment(IDictionary<string, string> payment, IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in payment)
		{
			ConnectImpl.CallStatic("setKey", pair.Key, pair.Value);
		}
		foreach (KeyValuePair<string, string> pair in properties)
		{
			ConnectImpl.CallStatic("setKeyOptional", pair.Key, pair.Value);
		}
		ConnectImpl.CallStatic("trackPaymentWithProperties");
	}

	public static void AttachProperties(IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in properties)
		{
			ConnectImpl.CallStatic("setKey", pair.Key, pair.Value);
		}
		ConnectImpl.CallStatic("attachProperties");
	}

	public static void TrackOptions(IDictionary<string, string> options, string commandId)
	{
		ConnectImpl.CallStatic("trackOptions", commandId);
	}

	public static void TrackOptions(IDictionary<string, string> options, string commandId, string code, string message)
	{
		ConnectImpl.CallStatic("trackOptions", commandId, code, message);
	}

	public static void TrackExperimentStart(string experiment, string groupId)
	{
		ConnectImpl.CallStatic("trackExperimentStart", experiment, groupId);
	}

	public static void TrackExperimentEnd(string experiment)
	{
		ConnectImpl.CallStatic("trackExperimentEnd", experiment);
	}

	public static void Identify(string userId)
	{
		ConnectImpl.CallStatic("identify", userId);
	}

	public static void Flush()
	{
		ConnectImpl.CallStatic("flush");
	}
}
#endif
