using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID && !UNITY_EDITOR
using AppmetrPlatformPlugin = Appmetr.Unity.Impl.AppmetrPluginAndroid;
#elif UNITY_IOS && !UNITY_EDITOR
using AppmetrPlatformPlugin = Appmetr.Unity.Impl.AppmetrPluginIos;
#elif UNITY_STANDALONE && !UNITY_EDITOR
using AppmetrPlatformPlugin = Appmetr.Unity.Impl.AppmetrPluginStandalone;
#else
using AppmetrPlatformPlugin = Appmetr.Unity.Impl.AppmetrPluginDefault;
#endif

namespace Appmetr.Unity
{
	/// <summary>
	/// The main class.
	/// Provides access to the functions of the library.
	/// </summary>
	public static class AppMetr
	{
		/// <summary>
		/// Setting up plugin.
		/// </summary>
		/// <param name="token">
		/// Parameter which is needed for data upload.
		/// </param>
		public static void Setup(string token)
		{
			AppmetrPlatformPlugin.SetupWithToken(token, null);
		}
		
		/// <summary>
		/// Sets mandatory plugin parameters
		/// </summary>
		/// <param name="token">Deploy token as assigned on backend</param>
		/// <param name="platform">Platform or store name. Changes behaviour of 
		/// certain events, especially payment tracking.</param>
		public static void Setup(string token, string platform)
		{
			AppmetrPlatformPlugin.SetupWithToken(token, platform);
		}


		/// <summary>
		/// Set application status.
		/// </summary>
		/// <param name="pause">
		/// Application status. True - if application paused and false otherwise
		/// </param>
		public static void OnPause(bool pause)
		{
			if (pause)
			{
				AppmetrPlatformPlugin.OnPause();
			}
			else
			{
				AppmetrPlatformPlugin.OnResume();
			}
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
		public static void TrackSession(IDictionary<string, object> properties)
		{
			AppmetrPlatformPlugin.TrackSession(properties);
		}

		/// <summary>
		/// Method for tracking game event as "track event" with parameter "event".
		/// </summary>
		/// <param name="eventName">
		/// Required field. Displays event's named.
		/// </param>
		public static void TrackEvent(string eventName)
		{
			AppmetrPlatformPlugin.TrackEvent(eventName);
		}

		/// <summary>
		/// Method for tracking game event as "track event" with parameters "event","value" and "properties".
		/// </summary>
		/// <param name="eventName">
		/// Required field. Displays event's named.
		/// </param>
		/// <param name="properties">
		/// Additional parameters for event.
		/// </param>
		public static void TrackEvent(string eventName, IDictionary<string, object> properties)
		{
			AppmetrPlatformPlugin.TrackEvent(eventName, properties);
		}

		/// <summary>
		/// Method must be called when player does any payment.
		/// </summary>
		/// <param name="payment">
		/// Required fields "psUserSpentCurrencyCode", "psUserSpentCurrencyAmount", "psReceivedCurrencyCode" and "psReceivedCurrencyAmount".
		/// </param>
		public static void TrackPayment(IDictionary<string, object> payment)
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
		public static void TrackPayment(IDictionary<string, object> payment, IDictionary<string, object> properties)
		{
			AppmetrPlatformPlugin.TrackPayment(payment, properties);
		}

		/// <summary>
		/// Method for attaching only built-in user properties.
		/// </summary>
		public static void AttachProperties()
		{
			AppmetrPlatformPlugin.AttachProperties();
		}

		/// <summary>
		/// Method for attaching user properties.
		/// </summary>
		/// <param name='properties'>
		/// Properties.
		/// </param>
		public static void AttachProperties(IDictionary<string, object> properties)
		{
			AppmetrPlatformPlugin.AttachProperties(properties);
		}

		/// <summary>
		/// Method for tracking user state.
		/// </summary>
		/// <param name='state'>
		/// state.
		/// </param>
		public static void TrackState(IDictionary<string, object> state)
		{
			AppmetrPlatformPlugin.TrackState(state);
		}

		/// <summary>
		/// Identify user.
		/// </summary>
		/// <param name='userId'>
		/// User identifier.
		/// </param>
		public static void Identify(string userId)
		{
			AppmetrPlatformPlugin.Identify(userId);
		}
		
		/// <summary>
		/// Method for attaching properties to separate entity instead of user
		/// </summary>
		/// <param name='entityName'>
		/// Name of entity to attach.
		/// </param>
		/// <param name='entityValue'>
		/// Identity value of entity.
		/// </param>
		/// <param name='properties'>
		/// Attributes to attach.
		/// </param>
		public static void AttachEntityAttributes(string entityName, string entityValue, IDictionary<string, object> properties)
		{
			if (string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(entityValue))
			{
				Debug.LogError("AttachEntityAttributes has empty entity name or value. Aborted");
				return;
			}
			if (properties == null || properties.Count == 0)
			{
				Debug.LogError("AttachEntityAttributes has empty properties dictionary. Aborted");
				return;
			}
			AppmetrPlatformPlugin.AttachEntityAttributes(entityName, entityValue, properties);
		}

		/// <summary>
		/// Force flush events on server. Flushing execute in new thread.
		/// </summary>
		public static void Flush()
		{
			AppmetrPlatformPlugin.Flush();
		}
		
		/// <summary>
		/// Force flush events to disk. Flushing execute in new thread.
		/// </summary>
		public static void FlushLocal()
		{
			AppmetrPlatformPlugin.FlushLocal();
		}

		/// <summary>
		/// Return full instance identifier.
		/// </summary>
		public static string GetInstanceIdentifier()
		{
			return AppmetrPlatformPlugin.GetInstanceIdentifier();
		}
		
		/// <summary>
		/// Return a set of device ids, encoded in a query string
		/// </summary>
		public static string GetDeviceKey()
		{
			return AppmetrPlatformPlugin.GetDeviceKey();
		}

		public const string AppmetrPropertyTimestamp = "timestamp";
	}
}
