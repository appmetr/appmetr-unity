using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AppmetrPluginIOS
{
	#region	Interface to native implementation
	
	[DllImport("__Internal")]
	private static extern void _setupWithToken(string token);
	
	[DllImport("__Internal")]
	private static extern void _attachProperties(string properties);
	
	[DllImport("__Internal")]
	private static extern void _trackSession();
	
	[DllImport("__Internal")]
	private static extern void _trackSessionWithProperties(string properties);
	
	[DllImport("__Internal")]
	private static extern void _trackLevel(int level);
	
	[DllImport("__Internal")]
	private static extern void _trackEvent(string _event);
	
	[DllImport("__Internal")]
	private static extern void _trackEvent(string _event, string properties);
	
	[DllImport("__Internal")]
	private static extern void _trackPayment(string payment);
	
	[DllImport("__Internal")]
	private static extern void _trackPayment(string payment, string properties);
	
	[DllImport("__Internal")]
	private static extern void _trackOptions(string options, string commandId);
	
	[DllImport("__Internal")]
	private static extern void _trackOptions(string options, string commandId, string code, string message);
	
	#endregion

	#region Declarations for non-native

	public static void SetupWithToken(string token)
	{
		_setupWithToken(token);
	}

	public static void AttachProperties(IDictionary<string, string> properties)
	{
		string value = MiniJSON.jsonEncode(properties);
		_attachProperties(value);
	}

	public static void TrackSession()
	{
		_trackSession();
	}

	public static void TrackSession(IDictionary<string, string> properties)
	{
		string value = MiniJSON.jsonEncode(properties);
		_trackSessionWithProperties(value);
	}

	public static void TrackLevel(int level)
	{
		_trackLevel(level);
	}

	public static void TrackEvent(string _event)
	{
		_trackEvent(_event);
	}

	public static void TrackEvent(string _event, IDictionary<string, string> properties)
	{
		string value = MiniJSON.jsonEncode(properties);
		_trackEvent(_event, value);
	}

	public static void TrackPayment(IDictionary<string, string> payment)
	{
		string value = MiniJSON.jsonEncode(payment);
		_trackPayment(value);
	}

	public static void TrackPayment(IDictionary<string, string> payment, IDictionary<string, string> properties)
	{
		string paymentValue = MiniJSON.jsonEncode(payment);
		string propertiesValue = MiniJSON.jsonEncode(properties);
		_trackPayment(paymentValue, propertiesValue);
	}
	
	public static void TrackOptions(IDictionary<string, string> options, string commandId)
	{
		string value = MiniJSON.jsonEncode(options);
		_trackOptions(value, commandId);
	}
	
	public static void TrackOptions(IDictionary<string, string> options, string commandId, string code, string message)
	{
		string value = MiniJSON.jsonEncode(options);
		_trackOptions(value, commandId, code, message);
	}
	
	#endregion
}
