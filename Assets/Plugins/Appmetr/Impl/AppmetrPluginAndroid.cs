using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class AppmetrPluginAndroid : AppmetrWrapper
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

	public static void AttachProperties(string properties)
	{
		ConnectHelper.CallStatic("attachProperties", properties);
	}

	public static void TrackSession()
	{
		ConnectHelper.CallStatic("trackSession");
	}

	public static void TrackSession(string properties)
	{
		ConnectHelper.CallStatic("trackSession", properties);
	}

	public static void TrackLevel(int level)
	{
		Connect.CallStatic("trackLevel", level);
	}

	public static void TrackEvent(string _event)
	{
		ConnectHelper.CallStatic("trackEvent", _event);
	}

	public static void TrackEvent(string _event, string properties)
	{
		ConnectHelper.CallStatic("trackEvent", _event, properties);
	}

	public static void TrackPayment(string payment)
	{
		ConnectHelper.CallStatic("trackPayment", payment);
	}

	public static void TrackPayment(string payment, string properties)
	{
		ConnectHelper.CallStatic("trackPayment", payment, properties);
	}

	public static void TrackOptions(string options, string commandId)
	{
		ConnectHelper.CallStatic("trackOptions", commandId, options);
	}

	public static void TrackOptions(string options, string commandId, string code, string message)
	{
		ConnectHelper.CallStatic("trackOptionsError", commandId, options, code, message);
	}
}
