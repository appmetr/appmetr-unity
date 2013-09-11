﻿#if UNITY_IPHONE
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AppmetrPluginIOS
{
	#region	Interface to native implementation

	[DllImport("__Internal")]
	private static extern void _setKeyValue(string key, string value);

	[DllImport("__Internal")]
	private static extern void _setKeyValueOptional(string key, string value);
	
	[DllImport("__Internal")]
	private static extern void _setupWithToken(string token);
	
	[DllImport("__Internal")]
	private static extern void _attachProperties();
	
	[DllImport("__Internal")]
	private static extern void _trackSession();
	
	[DllImport("__Internal")]
	private static extern void _trackSessionWithProperties();
	
	[DllImport("__Internal")]
	private static extern void _trackLevel(int level);
	
	[DllImport("__Internal")]
	private static extern void _trackLevelWithProperties(int level);
	
	[DllImport("__Internal")]
	private static extern void _trackEvent(string _event);
	
	[DllImport("__Internal")]
	private static extern void _trackEventWithProperties(string _event);
	
	[DllImport("__Internal")]
	private static extern void _trackPayment();
	
	[DllImport("__Internal")]
	private static extern void _trackPaymentWithProperties();
	
	[DllImport("__Internal")]
	private static extern void _trackOptions(string commandId);
	
	[DllImport("__Internal")]
	private static extern void _trackOptions(string commandId, string code, string message);
	
	#endregion

	#region Declarations for non-native

	public static void SetupWithToken(string token)
	{
		_setupWithToken(token);
	}

	public static void AttachProperties(IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in properties)
		{
			_setKeyValue(pair.Key, pair.Value);
		}
		_attachProperties();
	}

	public static void TrackSession()
	{
		_trackSession();
	}

	public static void TrackSession(IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in properties)
		{
			_setKeyValue(pair.Key, pair.Value);
		}
		_trackSessionWithProperties();
	}

	public static void TrackLevel(int level)
	{
		_trackLevel(level);
	}

	public static void TrackLevel(int level, IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in properties)
		{
			_setKeyValue(pair.Key, pair.Value);
		}
		_trackLevelWithProperties(level);
	}

	public static void TrackEvent(string _event)
	{
		_trackEvent(_event);
	}

	public static void TrackEvent(string _event, IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in properties)
		{
			_setKeyValue(pair.Key, pair.Value);
		}
		_trackEventWithProperties(_event);
	}

	public static void TrackPayment(IDictionary<string, string> payment)
	{
		foreach (KeyValuePair<string, string> pair in payment)
		{
			_setKeyValue(pair.Key, pair.Value);
		}
		_trackPayment();
	}

	public static void TrackPayment(IDictionary<string, string> payment, IDictionary<string, string> properties)
	{
		foreach (KeyValuePair<string, string> pair in payment)
		{
			_setKeyValue(pair.Key, pair.Value);
		}
		foreach (KeyValuePair<string, string> pair in properties)
		{
			_setKeyValueOptional(pair.Key, pair.Value);
		}
		_trackPaymentWithProperties();
	}
	
	public static void TrackOptions(IDictionary<string, string> options, string commandId)
	{
		foreach (KeyValuePair<string, string> pair in options)
		{
			_setKeyValue(pair.Key, pair.Value);
		}
		_trackOptions(commandId);
	}
	
	public static void TrackOptions(IDictionary<string, string> options, string commandId, string code, string message)
	{
		foreach (KeyValuePair<string, string> pair in options)
		{
			_setKeyValue(pair.Key, pair.Value);
		}
		_trackOptions(commandId, code, message);
	}
	
	#endregion
}
#endif