#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Appmetr.Unity.Json;

namespace Appmetr.Unity.Impl
{
	public class AppmetrPluginIos
	{
		#region	Interface to native implementation

		[DllImport("__Internal")]
		private static extern void _setupWithToken(string token);
	
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
		private static extern void _trackPaymentWithProperties(string payment, string properties);
	
		[DllImport("__Internal")]
		private static extern void _attachPropertiesNull();
	
		[DllImport("__Internal")]
		private static extern void _attachProperties(string properties);
	
		[DllImport("__Internal")]
		private static extern void _trackExperimentStart(string experiment, string groupId);
	
		[DllImport("__Internal")]
		private static extern void _trackExperimentEnd(string experiment);
	
		[DllImport("__Internal")]
		private static extern void _trackState(string state);
	
		[DllImport("__Internal")]
		private static extern void _identify(string userId);
	
		[DllImport("__Internal")]
		private static extern void _flush();

		[DllImport("__Internal")]
		private static extern void _flushLocal();
	
		[DllImport("__Internal")]
		private static extern string _instanceIdentifier();

		[DllImport("__Internal")]
		private static extern string _deviceKey();

		[DllImport("__Internal")]
		private static extern void _attachEntityAttributes(string entityName, string entityValue, string properties);
	
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

		public static void SetupWithToken(string token, string platform)
		{
			_setupWithToken(token);
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

		public static void TrackExperimentStart(string experiment, string groupId)
		{
			_trackExperimentStart(experiment, groupId);
		}

		public static void TrackExperimentEnd(string experiment)
		{
			_trackExperimentEnd(experiment);
		}

		public static void TrackState(IDictionary<string, object> state) 
		{
			_trackState(ToJson(state));
		}

		public static void Identify(string userId)
		{
			_identify(userId);
		}

		public static void AttachEntityAttributes(string entityName, string entityValue,
			IDictionary<string, object> properties)
		{
			_attachEntityAttributes(entityName, entityValue, ToJson(properties));
		}

		public static void Flush()
		{
			_flush();
		}

		public static void FlushLocal()
		{
			_flushLocal();
		}

		public static string GetInstanceIdentifier()
		{
			return _instanceIdentifier();
		}

		public static string GetDeviceKey()
        {
            return _deviceKey();
        }
	
		#endregion
	}
}
#endif
