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

/// <summary>
/// The main class.
/// Provides access to the functions of the library.
/// </summary>
public class AppmetrPlugin
{
	/// <summary>
	/// Setting up plugin.
	/// </summary>
	/// <param name="token">
	/// Parameter which is needed for data upload.
	/// </param>
	public static void Setup(string token)
	{
		AppmetrPlatformPlugin.SetupWithToken(token);
	}
	
	/// <summary>
	/// Method for tracking game event as "track session" without parameters.
	/// </summary>
	public static void TrackSession()
	{
		AppmetrPlatformPlugin.TrackSession();
	}
	
	/// <summary>
	/// Method for tracking game event as "track session" with parameters.
	/// </summary>
	/// <param name="properties">
	/// Properties for event.
	/// </param>
	public static void TrackSession(IDictionary<string, string> properties)
	{
		AppmetrPlatformPlugin.TrackSession(properties);
	}
	
	/// <summary>
	/// Method for tracking game event as "track level" with parameters.
	/// </summary>
	/// <param name="level">
	/// Parameter required for this event. Displays player's level.
	/// </param>
	public static void TrackLevel(int level)
	{
		AppmetrPlatformPlugin.TrackLevel(level);
	}
	
	/// <summary>
	/// Method for tracking game event as "track level" with parameter "level" and additional parameters.
	/// </summary>
	/// <param name="level">
	/// Parameter which is needed for data upload.
	/// </param>
	/// <param name="properties">
	/// Aadditional parameter for this event.
	/// </param>
	public static void TrackLevel(int level, IDictionary<string, string> properties)
	{
		AppmetrPlatformPlugin.TrackLevel(level, properties);
	}
	
	/// <summary>
	/// Method for tracking game event as "track event" with parameter "event".
	/// </summary>
	/// <param name="_event">
	/// Required field. Displays event's named.
	/// </param>
	public static void TrackEvent(string _event)
	{
		AppmetrPlatformPlugin.TrackEvent(_event);
	}
	
	/// <summary>
	/// Method for tracking game event as "track event" with parameters "event","value" and "properties".
	/// </summary>
	/// <param name="_event">
	/// Required field. Displays event's named.
	/// </param>
	/// <param name="properties">
	/// Additional parameters for event.
	/// </param>
	public static void TrackEvent(string _event, IDictionary<string, string> properties)
	{
		AppmetrPlatformPlugin.TrackEvent(_event, properties);
	}
	
	/// <summary>
	/// Method must be called when player does any payment.
	/// </summary>
	/// <param name="payment">
	/// Required fields "psUserSpentCurrencyCode", "psUserSpentCurrencyAmount", "psReceivedCurrencyCode" and "psReceivedCurrencyAmount".
	/// </param>
	public static void TrackPayment(IDictionary<string, string> payment)
	{
		AppmetrPlatformPlugin.TrackPayment(payment);
	}
	
	/// <summary>
	/// Method must be called when player does any payment.
	/// </summary>
	/// <param name="payment">
	/// Required fields "psUserSpentCurrencyCode", "psUserSpentCurrencyAmount", "psReceivedCurrencyCode" and "psReceivedCurrencyAmount".
	///	If these fields are not found library throws an exception.
	/// </param>
	/// <param name="properties">
	/// Additional information for event.
	/// </param>
	public static void TrackPayment(IDictionary<string, string> payment, IDictionary<string, string> properties)
	{
		AppmetrPlatformPlugin.TrackPayment(payment, properties);
	}
}
