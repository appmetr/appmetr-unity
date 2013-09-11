﻿#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AppmetrPluginAndroid
{
	private static AndroidJavaObject currentActivity;
	private static AndroidJavaObject applicationContext;
	
	private static AndroidJavaClass clsConnect;
	private static AndroidJavaClass clsConnectHelper;
	private static AndroidJavaClass clsConnectImpl;
	
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
				clsConnectImpl = new AndroidJavaClass("com.appmetr.android.AppMetrImpl");
			}
			return clsConnectImpl;
		}
	}
	
	private static void getActivity()
	{
		if (clsConnect == null && clsConnectHelper == null)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			applicationContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
		}
	}
	
	public static void SetupWithToken(string token)
	{
		Connect.CallStatic("setup", token, applicationContext, null);
	}

	public static void AttachProperties(IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in properties)
		{
			ConnectImpl.CallStatic("setKey", pair.Key, pair.Value);
		}
		ConnectImpl.CallStatic("attachProperties");
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

	public static void TrackEvent(string _event)
	{
		ConnectImpl.CallStatic("trackEvent", _event);
	}

	public static void TrackEvent(string _event, IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in properties)
		{
			ConnectImpl.CallStatic("setKey", pair.Key, pair.Value);
		}
		ConnectImpl.CallStatic("trackEventWithProperties", _event);
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

	public static void TrackOptions(IDictionary<string, string> options, string commandId)
	{
		ConnectImpl.CallStatic("trackOptions", commandId);
	}

	public static void TrackOptions(IDictionary<string, string> options, string commandId, string code, string message)
	{
		ConnectImpl.CallStatic("trackOptions", commandId, code, message);
	}
}
#endif