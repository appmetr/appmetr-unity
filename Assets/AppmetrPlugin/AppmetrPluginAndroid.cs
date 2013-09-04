using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;

public class AppmetrPluginAndroid : MonoBehaviour
{
	private static AndroidJavaObject currentActivity;
	
	private static AndroidJavaClass clsConnect;
	private static AndroidJavaClass Connect
	{
		get
		{
			if (clsConnect == null)
			{
				Debug.Log("C#: Loading AppmetrPlugin");
			
				AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
				currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
				
				clsConnect = new AndroidJavaClass("com.appmetr.android.AppMetr");
			}
			
			return clsConnect;
		}
	}
	
	public static void SetupWithToken(string token)
	{
	}

	public static void SetupWithUserID(string userID)
	{
	}

	public static void AttachProperties(string properties)
	{
		object json = new JavaScriptSerializer.Deserialize<object>(properties);
		Connect.CallStatic("attachProperties", json);
	}

	public static void TrackSession()
	{
		Connect.CallStatic("trackSession");
	}

	public static void TrackSession(string properties)
	{
		object json = new JavaScriptSerializer.Deserialize<object>(properties);
		Connect.CallStatic("trackSession", json);
	}

	public static void TrackLevel(int level)
	{
		Connect.CallStatic("trackLevel", level);
	}

	public static void TrackLevel(int level, string properties)
	{
		object json = new JavaScriptSerializer.Deserialize<object>(properties);
		Connect.CallStatic("trackLevel", level, json);
	}

	public static void TrackEvent(string _event)
	{
		Connect.CallStatic("trackEvent", _event);
	}

	public static void TrackEvent(string _event, string properties)
	{
		object json = new JavaScriptSerializer.Deserialize<object>(properties);
		Connect.CallStatic("trackEvent", _event, json);
	}

	public static void TrackPayment(string payment)
	{
		object json = new JavaScriptSerializer.Deserialize<object>(payment);
		Connect.CallStatic("trackPayment", json);
	}

	public static void TrackPayment(string payment, string properties)
	{
		object jsonPayment = new JavaScriptSerializer.Deserialize<object>(payment);
		object jsonProperties = new JavaScriptSerializer.Deserialize<object>(properties);
		Connect.CallStatic("trackPayment", jsonPayment, jsonProperties);
	}

	public static void TrackGameState(string state, string properties)
	{
		object json = new JavaScriptSerializer.Deserialize<object>(properties);
		Connect.CallStatic("trackGameState", state, json);
	}

	public static void TrackOptions(string options, string commandId)
	{
		object json = new JavaScriptSerializer.Deserialize<object>(options);
		Connect.CallStatic("trackOptions", commandId, json);
	}

	public static void TrackOptions(string options, string commandId, string code, string message)
	{
		object json = new JavaScriptSerializer.Deserialize<object>(options);
		Connect.CallStatic("trackOptionsError", commandId, json, code, message);
	}

	public static void TrackExperimentStart(string experiment, string group)
	{
		Connect.CallStatic("trackExperimentStart", experiment, group);
	}

	public static void TrackExperimentEnd(string experiment)
	{
		Connect.CallStatic("trackExperimentEnd", experiment);
	}

	public static void PullCommands()
	{
		Connect.CallStatic("pullCommands");
	}

	public static void Flush()
	{
		Connect.CallStatic("flush");
	}

	public static void SetDebugLoggingEnabled(bool debugLoggingEnabled)
	{
	}
}
