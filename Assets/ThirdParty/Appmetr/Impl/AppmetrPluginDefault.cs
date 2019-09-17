#if UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID && !UNITY_STANDALONE)
using System;
using System.Collections.Generic;

namespace Appmetr.Unity.Impl
{
	public class AppmetrPluginDefault
	{
		public static void SetupWithToken(string token, string platform) {}

		public static void OnPause() {}

		public static void OnResume() {}

		public static void TrackSession() {}

		public static void TrackSession(IDictionary<string, object> properties) {}

		public static void TrackLevel(int level) {}

		public static void TrackLevel(int level, IDictionary<string, object> properties) {}

		public static void TrackEvent(string eventName) {}

		public static void TrackEvent(string eventName, IDictionary<string, object> properties) {}

		public static void TrackPayment(IDictionary<string, object> payment) {}

		public static void TrackPayment(IDictionary<string, object> payment, IDictionary<string, object> properties) {}

		public static void TrackAdsEvent(string eventName) {}
	
		public static void AttachProperties() {}

		public static void AttachProperties(IDictionary<string, object> properties) {}

		public static void TrackExperimentStart(string experiment, string groupId) {}

		public static void TrackExperimentEnd(string experiment) {}

		public static bool VerifyIosPayment(string productId, string transactionId, string receipt, string privateKey) { return false; }

		public static void VerifyAndroidPayment(string purchaseInfo, string signature, string privateKey, Action<bool> callback)
		{
			if (callback != null)
			{
				callback(false);
			}
		}

		public static void TrackState(IDictionary<string, object> state) {}

		public static void Identify(string userId) {}

		public static void AttachEntityAttributes(string entityName, string entityValue,
			IDictionary<string, object> properties)
		{
		}

		public static void Flush() {}
		
		public static void FlushLocal() {}

		public static string GetInstanceIdentifier() { return ""; }
		
		public static string GetDeviceKey() { return ""; }
	}
}
#endif