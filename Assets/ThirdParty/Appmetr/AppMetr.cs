using System;
using System.Collections.Generic;

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
		public static void TrackLevel(int level, IDictionary<string, object> properties)
		{
			AppmetrPlatformPlugin.TrackLevel(level, properties);
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
		/// Method for tracking advertising events.
		/// </summary>
		/// <param name="eventName">
		///	Name of the event to track.
		/// </param>
		public static void TrackAdsEvent(string eventName)
		{
			AppmetrPlatformPlugin.TrackAdsEvent(eventName);
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
		/// Registering start of experiment.
		/// </summary>
		/// <param name='experiment'>
		/// Experiment.
		/// </param>
		/// <param name='groupId'>
		/// Group identifier.
		/// </param>
		public static void TrackExperimentStart(string experiment, string groupId)
		{
			AppmetrPlatformPlugin.TrackExperimentStart(experiment, groupId);
		}

		/// <summary>
		/// Registering end of experiment.
		/// </summary>
		/// <param name='experiment'>
		/// Experiment.
		/// </param>
		public static void TrackExperimentEnd(string experiment)
		{
			AppmetrPlatformPlugin.TrackExperimentEnd(experiment);
		}

		/// <summary>
		/// Verify payment for iOS platform
		/// </summary>
		/// <param name='productId'>
		/// Product ID.
		/// </param>
		/// <param name='transactionId'>
		/// Transaction ID.
		/// </param>
		/// <param name='receipt'>
		/// Base64 encoded receipt.
		/// </param>
		/// <param name='privateKey'>
		/// AppMetr private key.
		/// </param>
		public static bool VerifyIosPayment(string productId, string transactionId, string receipt, string privateKey)
		{
			return AppmetrPlatformPlugin.VerifyIosPayment(productId, transactionId, receipt, privateKey);
		}

		/// <summary>
		/// Verify payment for Android platform
		/// </summary>
		/// <param name='purchaseInfo'>
		/// Purchase original JSON.
		/// </param>
		/// <param name='signature'>
		/// Purchase signature.
		/// </param>
		/// <param name='privateKey'>
		/// AppMetr private key.
		/// </param>
		/// <param name="callback">
		/// Callback for returning result
		/// </param>
		public static void VerifyAndroidPayment(string purchaseInfo, string signature, string privateKey, Action<bool> callback)
		{
			AppmetrPlatformPlugin.VerifyAndroidPayment(purchaseInfo, signature, privateKey, callback);
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
	}
}
