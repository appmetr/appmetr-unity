using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_ANDROID && !UNITY_EDITOR
using AppmetrPlatformPlugin = AppmetrPluginAndroid;
#elif UNITY_IOS && !UNITY_EDITOR
using AppmetrPlatformPlugin = AppmetrPluginIOS;
#else
using AppmetrPlatformPlugin = AppmetrPluginDefault;
#endif

public class AppmetrPlugin
{
	public static void Setup(string token)
	{
		AppmetrPlatformPlugin.SetupWithToken(token);
	}
	
	public static void AttachProperties(IDictionary<string, string> properties)
	{
		AppmetrPlatformPlugin.AttachProperties(properties);
	}
	
	public static void TrackSession()
	{
		AppmetrPlatformPlugin.TrackSession();
	}
	
	public static void TrackSession(IDictionary<string, string> properties)
	{
		AppmetrPlatformPlugin.TrackSession(properties);
	}
	
	public static void TrackLevel(int level)
	{
		AppmetrPlatformPlugin.TrackLevel(level);
	}
	
	public static void TrackLevel(int level, IDictionary<string, string> properties)
	{
		AppmetrPlatformPlugin.TrackLevel(level, properties);
	}
	
	public static void TrackEvent(string _event)
	{
		AppmetrPlatformPlugin.TrackEvent(_event);
	}
	
	public static void TrackEvent(string _event, IDictionary<string, string> properties)
	{
		AppmetrPlatformPlugin.TrackEvent(_event, properties);
	}
	
	public static void TrackPayment(IDictionary<string, string> payment)
	{
		AppmetrPlatformPlugin.TrackPayment(payment);
	}
	
	public static void TrackPayment(IDictionary<string, string> payment, IDictionary<string, string> properties)
	{
		AppmetrPlatformPlugin.TrackPayment(payment, properties);
	}
	
	public static void TrackOptions(IDictionary<string, string> options, string commandId)
	{
		AppmetrPlatformPlugin.TrackOptions(options, commandId);
	}
	
	public static void TrackOptions(IDictionary<string, string> options, string commandId, string code, string message)
	{
		AppmetrPlatformPlugin.TrackOptions(options, commandId, code, message);
	}
}
