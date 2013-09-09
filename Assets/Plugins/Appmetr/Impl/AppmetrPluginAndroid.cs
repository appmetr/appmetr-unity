using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AppmetrPluginAndroid
{
	private static AndroidJavaObject currentActivity;
	
	private static AndroidJavaClass clsConnect;
	private static AndroidJavaClass clsConnectHelper;
	
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
	
	private static void getActivity()
	{
		if (clsConnect == null && clsConnectHelper == null)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		}
	}
	
	public static void SetupWithToken(string token)
	{
		Connect.CallStatic("setup", token, null, null);
	}

	public static void AttachProperties(IDictionary<string, string> properties)
	{
		string value = MiniJSON.jsonEncode(properties);
		ConnectHelper.CallStatic("attachProperties", value);
	}

	public static void TrackSession()
	{
		ConnectHelper.CallStatic("trackSession");
	}

	public static void TrackSession(IDictionary<string, string> properties)
	{
		string value = MiniJSON.jsonEncode(properties);
		ConnectHelper.CallStatic("trackSession", value);
	}

	public static void TrackLevel(int level)
	{
		Connect.CallStatic("trackLevel", level);
	}

	public static void TrackEvent(string _event)
	{
		ConnectHelper.CallStatic("trackEvent", _event);
	}

	public static void TrackEvent(string _event, IDictionary<string, string> properties)
	{
		string value = MiniJSON.jsonEncode(properties);
		ConnectHelper.CallStatic("trackEvent", _event, value);
	}

	public static void TrackPayment(IDictionary<string, string> payment)
	{
		string value = MiniJSON.jsonEncode(payment);
		ConnectHelper.CallStatic("trackPayment", value);
	}

	public static void TrackPayment(IDictionary<string, string> payment, IDictionary<string, string> properties)
	{
		string paymentValue = MiniJSON.jsonEncode(payment);
		string propertiesValue = MiniJSON.jsonEncode(properties);
		ConnectHelper.CallStatic("trackPayment", paymentValue, propertiesValue);
	}

	public static void TrackOptions(IDictionary<string, string> options, string commandId)
	{
		string value = MiniJSON.jsonEncode(options);
		ConnectHelper.CallStatic("trackOptions", commandId, value);
	}

	public static void TrackOptions(IDictionary<string, string> options, string commandId, string code, string message)
	{
		string value = MiniJSON.jsonEncode(options);
		ConnectHelper.CallStatic("trackOptionsError", commandId, value, code, message);
	}
}
