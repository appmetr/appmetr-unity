#if UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppmetrPluginDefault
{
	public static void SetupWithToken(string token) {}

	public static void TrackSession() {}

	public static void TrackSession(IDictionary<string, object> properties) {}

	public static void TrackLevel(int level) {}

	public static void TrackLevel(int level, IDictionary<string, object> properties) {}

	public static void TrackEvent(string eventName) {}

	public static void TrackEvent(string eventName, IDictionary<string, object> properties) {}

	public static void TrackPayment(IDictionary<string, object> payment) {}

	public static void TrackPayment(IDictionary<string, object> payment, IDictionary<string, object> properties) {}
	
	public static void AttachProperties() {}

	public static void AttachProperties(IDictionary<string, object> properties) {}

	public static void TrackOptions(string commandId, IDictionary<string, object> options) {}

	public static void TrackOptionsError(string commandId, IDictionary<string, object> options, string code, string message) {}

	public static void TrackExperimentStart(string experiment, string groupId) {}

	public static void TrackExperimentEnd(string experiment) {}

	public static void Identify(string userId) {}

	public static void Flush() {}

	public static string GetInstanceIdentifier() { return ""; }
}
#endif
