#if UNITY_IOS
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Appmetr.Unity.Json;

namespace Appmetr.Unity.Impl
{
	public class AppmetrPluginIos
	{
		#region	Interface to native implementation

		[DllImport("__Internal")]
		private static extern void _setupWithToken(string token, string commandListenerName);
	
		[DllImport("__Internal")]
		private static extern void _trackSession();
	
		[DllImport("__Internal")]
		private static extern void _trackSessionWithProperties(string properties);
	
		[DllImport("__Internal")]
		private static extern void _trackLevel(int level);
	
		[DllImport("__Internal")]
		private static extern void _trackLevelWithProperties(int level, string properties);
	
		[DllImport("__Internal")]
		private static extern void _trackEvent(string eventName);
	
		[DllImport("__Internal")]
		private static extern void _trackEventWithProperties(string eventName, string properties);
	
		[DllImport("__Internal")]
		private static extern void _trackPayment(string payment);
	
		[DllImport("__Internal")]
		private static extern void _trackAdsEvent(string eventName);

		[DllImport("__Internal")]
		private static extern void _trackPaymentWithProperties(string payment, string properties);
	
		[DllImport("__Internal")]
		private static extern void _attachPropertiesNull();
	
		[DllImport("__Internal")]
		private static extern void _attachProperties(string properties);
	
		[DllImport("__Internal")]
		private static extern void _trackOptions(string commandId, string options);
	
		[DllImport("__Internal")]
		private static extern void _trackOptionsWithErrorCode(string commandId, string options, string code, string message);
	
		[DllImport("__Internal")]
		private static extern void _trackExperimentStart(string experiment, string groupId);
	
		[DllImport("__Internal")]
		private static extern void _trackExperimentEnd(string experiment);

		[DllImport("__Internal")]
		private static extern bool _verifyPayment(string productId, string transactionId, string receipt, string privateKey);
	
		[DllImport("__Internal")]
		private static extern void _trackState(string state);
	
		[DllImport("__Internal")]
		private static extern void _identify(string userId);
	
		[DllImport("__Internal")]
		private static extern void _flush();
	
		[DllImport("__Internal")]
		private static extern string _instanceIdentifier();
	
		#endregion

		#region Declarations for non-native

		private static string ToJson(IDictionary<string, object> properties) 
		{
			return Serializer.Serialize(properties);
		}

		private static string ToJson(IDictionary<string, object>[] properties) 
		{
			var propertiesList = new List<object>(properties);
			return Serializer.Serialize(propertiesList);
		}

		public static void SetupWithToken(string token, string commandListenerName)
		{
			_setupWithToken(token, commandListenerName);
		}

		public static void OnPause() {}

		public static void OnResume() {}

		public static void TrackSession()
		{
			_trackSession();
		}

		public static void TrackSession(IDictionary<string, object> properties)
		{
			_trackSessionWithProperties(ToJson(properties));
		}

		public static void TrackLevel(int level)
		{
			_trackLevel(level);
		}

		public static void TrackLevel(int level, IDictionary<string, object> properties)
		{
			_trackLevelWithProperties(level, ToJson(properties));
		}

		public static void TrackEvent(string _event)
		{
			_trackEvent(_event);
		}

		public static void TrackEvent(string eventName, IDictionary<string, object> properties)
		{
			_trackEventWithProperties(eventName, ToJson(properties));
		}

		public static void TrackPayment(IDictionary<string, object> payment)
		{
			_trackPayment(ToJson(payment));
		}

		public static void TrackAdsEvent(string eventName)
		{
			_trackAdsEvent(eventName);
		}

		public static void TrackPayment(IDictionary<string, object> payment, IDictionary<string, object> properties)
		{
			_trackPaymentWithProperties(ToJson(payment), ToJson(properties));
		}
	
		public static void AttachProperties()
		{
			_attachPropertiesNull();
		}
	
		public static void AttachProperties(IDictionary<string, object> properties)
		{
			_attachProperties(ToJson(properties));
		}
	
		public static void TrackOptions(string commandId, IDictionary<string, object>[] options)
		{
			_trackOptions(commandId, ToJson(options));
		}
	
		public static void TrackOptionsError(string commandId, IDictionary<string, object>[] options, string code, string message)
		{
			_trackOptionsWithErrorCode(commandId, ToJson(options), code, message);
		}

		public static void TrackExperimentStart(string experiment, string groupId)
		{
			_trackExperimentStart(experiment, groupId);
		}

		public static void TrackExperimentEnd(string experiment)
		{
			_trackExperimentEnd(experiment);
		}

		public static bool VerifyIosPayment(string productId, string transactionId, string receipt, string privateKey) 
		{ 
			return _verifyPayment(productId, transactionId, receipt, privateKey); 
		}

		public static bool VerifyAndroidPayment(string purchaseInfo, string signature, string privateKey) 
		{ 
			return false; 
		}

		public static void TrackState(IDictionary<string, object> state) 
		{
			_trackState(ToJson(state));
		}

		public static void Identify(string userId)
		{
			_identify(userId);
		}

		public static void Flush()
		{
			_flush();
		}

		public static string GetInstanceIdentifier()
		{
			return _instanceIdentifier();
		}
	
		#endregion
	}
}
#endif
