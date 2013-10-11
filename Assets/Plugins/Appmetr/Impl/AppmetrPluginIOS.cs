#if UNITY_IOS
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AppmetrPluginIOS
{
	#region	Interface to native implementation

	[DllImport("__Internal")]
	private static extern void _setKeyValueString(string key, string value);

	[DllImport("__Internal")]
	private static extern void _setKeyValueFloat(string key, float value);

	[DllImport("__Internal")]
	private static extern void _setKeyValueInt(string key, int value);

	[DllImport("__Internal")]
	private static extern void _setKeyValueStringOptional(string key, string value);

	[DllImport("__Internal")]
	private static extern void _setKeyValueFloatOptional(string key, float value);

	[DllImport("__Internal")]
	private static extern void _setKeyValueIntOptional(string key, int value);
	
	[DllImport("__Internal")]
	private static extern void _setupWithToken(string token);
	
	[DllImport("__Internal")]
	private static extern void _trackSession();
	
	[DllImport("__Internal")]
	private static extern void _trackSessionWithProperties();
	
	[DllImport("__Internal")]
	private static extern void _trackLevel(int level);
	
	[DllImport("__Internal")]
	private static extern void _trackLevelWithProperties(int level);
	
	[DllImport("__Internal")]
	private static extern void _trackEvent(string eventName);
	
	[DllImport("__Internal")]
	private static extern void _trackEventWithProperties(string eventName);
	
	[DllImport("__Internal")]
	private static extern void _trackPayment();
	
	[DllImport("__Internal")]
	private static extern void _trackPaymentWithProperties();
	
	[DllImport("__Internal")]
	private static extern void _attachProperties();
	
	[DllImport("__Internal")]
	private static extern void _trackOptions(string commandId);
	
	[DllImport("__Internal")]
	private static extern void _trackOptionsWithErrorCode(string commandId, string code, string message);
	
	[DllImport("__Internal")]
	private static extern void _trackExperimentStart(string experiment, string groupId);
	
	[DllImport("__Internal")]
	private static extern void _trackExperimentEnd(string experiment);
	
	[DllImport("__Internal")]
	private static extern void _identify(string userId);
	
	[DllImport("__Internal")]
	private static extern void _flush();
	
	#endregion

	#region Declarations for non-native

	public static void SetupWithToken(string token)
	{
		_setupWithToken(token);
	}

	public static void TrackSession()
	{
		_trackSession();
	}

	public static void TrackSession(IDictionary<string, object> properties)
	{
		addProperties(properties);
		_trackSessionWithProperties();
	}

	public static void TrackLevel(int level)
	{
		_trackLevel(level);
	}

	public static void TrackLevel(int level, IDictionary<string, object> properties)
	{
		addProperties(properties);
		_trackLevelWithProperties(level);
	}

	public static void TrackEvent(string _event)
	{
		_trackEvent(_event);
	}

	public static void TrackEvent(string eventName, IDictionary<string, object> properties)
	{
		addProperties(properties);
		_trackEventWithProperties(eventName);
	}

	public static void TrackPayment(IDictionary<string, object> payment)
	{
		addProperties(payment);
		_trackPayment();
	}

	public static void TrackPayment(IDictionary<string, object> payment, IDictionary<string, object> properties)
	{
		addProperties(payment);
		addPropertiesOptional(properties);
		_trackPaymentWithProperties();
	}

	public static void AttachProperties(IDictionary<string, object> properties)
	{
		addProperties(properties);
		_attachProperties();
	}
	
	public static void TrackOptions(IDictionary<string, object> options, string commandId)
	{
		addProperties(options);
		_trackOptions(commandId);
	}
	
	public static void TrackOptions(IDictionary<string, object> options, string commandId, string code, string message)
	{
		addProperties(options);
		_trackOptionsWithErrorCode(commandId, code, message);
	}

	public static void TrackExperimentStart(string experiment, string groupId)
	{
		_trackExperimentStart(experiment, groupId);
	}

	public static void TrackExperimentEnd(string experiment)
	{
		_trackExperimentEnd(experiment);
	}

	public static void Identify(string userId)
	{
		_identify(userId);
	}

	public static void Flush()
	{
		_flush();
	}
	
	private static void addProperties(IDictionary<string, object> properties)
	{
		foreach (KeyValuePair<string, object> pair in properties)
		{
			setKeyValue(pair.Key, pair.Value);
		}
	}
	
	private static void addPropertiesOptional(IDictionary<string, object> properties)
	{
		foreach (KeyValuePair<string, object> pair in properties)
		{
			setKeyValueOptional(pair.Key, pair.Value);
		}
	}
	
	private static void setKeyValue(string key, object value)
	{
		if (value.GetType() == typeof(string))
		{
			_setKeyValueString(key, value.ToString());
		}
		else if (value.GetType() == typeof(float))
		{
			_setKeyValueFloat(key, Convert.ToFloat(value));
		}
		else if (value.GetType() == typeof(int))
		{
			_setKeyValueInt(key, Convert.ToInt32(value));
		}
	}
	
	private static void setKeyValueOptional(string key, object value)
	{
		if (value.GetType() == typeof(string))
		{
			_setKeyValueStringOptional(key, value.ToString());
		}
		else if (value.GetType() == typeof(float))
		{
			_setKeyValueFloatOptional(key, Convert.ToFloat(value));
		}
		else if (value.GetType() == typeof(int))
		{
			_setKeyValueIntOptional(key, Convert.ToInt32(value));
		}
	}
	
	private static bool validatePaymentNumberValue(string key)
	{
		if (key == "psUserSpentCurrencyAmount" || key == "psReceivedCurrencyAmount" || key == "appCurrencyAmount")
			return true;
		return false;
	}
	
	#endregion
}
#endif
