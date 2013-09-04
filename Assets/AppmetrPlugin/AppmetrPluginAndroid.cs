using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Json;

public class AppmetrPluginAndroid : AppmetrWrapper
{
	private static string msDefaultPaymentProcessor = "google_checkout";

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
	
	public static void SetDefaultPaymentProcessor(string processor)
	{
		msDefaultPaymentProcessor = processor;
	}

	private static JsonObject paymentWithPaymentProcessor(string serializedPayment)
	{
		JsonObject json = new JavaScriptSerializer.Deserialize<JsonObject>(serializedPayment);
		if (msDefaultPaymentProcessor != null && !json.ContainsKey("processor"))
		{
			json.Add("processor", msDefaultPaymentProcessor);
		}
		return json;
	}
	
	public static void SetupWithToken(string token)
	{
		Connect.CallStatic("setup", token, null);
	}

	public static void SetupWithUserID(string userID)
	{
	}

	public static void AttachProperties(string properties)
	{
		JsonObject json = new JavaScriptSerializer.Deserialize<JsonObject>(properties);
		if (json != null)
		{
			Connect.CallStatic("attachProperties", json);
		}
	}

	public static void TrackSession()
	{
		Connect.CallStatic("trackSession");
	}

	public static void TrackSession(string properties)
	{
		JsonObject json = new JavaScriptSerializer.Deserialize<JsonObject>(properties);
		if (json != null)
		{
			Connect.CallStatic("trackSession", json);
		}	
	}

	public static void TrackLevel(int level)
	{
		Connect.CallStatic("trackLevel", level);
	}

	public static void TrackLevel(int level, string properties)
	{
		JsonObject json = new JavaScriptSerializer.Deserialize<JsonObject>(properties);
		if (json != null)
		{
			Connect.CallStatic("trackLevel", level, json);
		}
	}

	public static void TrackEvent(string _event)
	{
		Connect.CallStatic("trackEvent", _event);
	}

	public static void TrackEvent(string _event, string properties)
	{
		JsonObject json = new JavaScriptSerializer.Deserialize<JsonObject>(properties);
		if (json != null)
		{
			Connect.CallStatic("trackEvent", _event, json);
		}
	}

	public static void TrackPayment(string payment)
	{
		JsonObject json = paymentWithPaymentProcessor(payment);
		if (json != null)
		{
			Connect.CallStatic("trackPayment", json);
		}	
	}

	public static void TrackPayment(string payment, string properties)
	{
		JsonObject jsonPayment = paymentWithPaymentProcessor(payment);
		JsonObject jsonProperties = new JavaScriptSerializer.Deserialize<JsonObject>(properties);
		if (jsonPayment != null && jsonProperties != null)
		{
			Connect.CallStatic("trackPayment", jsonPayment, jsonProperties);
		}
	}

	public static void TrackGameState(string state, string properties)
	{
		JsonObject json = new JavaScriptSerializer.Deserialize<JsonObject>(properties);
		if (json != null)
		{
			Connect.CallStatic("trackGameState", state, json);
		}	
	}

	public static void TrackOptions(string options, string commandId)
	{
		JsonObject json = new JavaScriptSerializer.Deserialize<JsonObject>(options);
		if (json != null)
		{
			Connect.CallStatic("trackOptions", commandId, json);
		}	
	}

	public static void TrackOptions(string options, string commandId, string code, string message)
	{
		JsonObject json = new JavaScriptSerializer.Deserialize<JsonObject>(options);
		if (json != null)
		{
			Connect.CallStatic("trackOptionsError", commandId, json, code, message);
		}	
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
