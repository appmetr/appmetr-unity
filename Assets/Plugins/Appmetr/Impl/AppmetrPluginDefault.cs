using UnityEngine;
using System.Collections;

public class AppmetrPluginDefault : AppmetrWrapper
{
	public static void SetupWithToken(string token) {}

	public static void AttachProperties(string properties) {}

	public static void TrackSession() {}

	public static void TrackSession(string properties) {}

	public static void TrackLevel(int level) {}

	public static void TrackEvent(string _event) {}

	public static void TrackEvent(string _event, string properties) {}

	public static void TrackPayment(string payment) {}

	public static void TrackPayment(string payment, string properties) {}

	public static void TrackOptions(string options, string commandId) {}

	public static void TrackOptions(string options, string commandId, string code, string message) {}
}
