using UnityEngine;
using System.Collections;

public class AppmetrPluginDefault : AppmetrWrapper
{
	public static void SetupWithToken(string token) {}

	public static void SetupWithUserID(string userID) {}

	public static void AttachProperties(string properties) {}

	public static void TrackSession() {}

	public static void TrackSession(string properties) {}

	public static void TrackLevel(int level) {}

	public static void TrackLevel(int level, string properties) {}

	public static void TrackEvent(string _event) {}

	public static void TrackEvent(string _event, string properties) {}

	public static void TrackPayment(string payment) {}

	public static void TrackPayment(string payment, string properties) {}

	public static void TrackGameState(string state, string properties) {}

	public static void TrackOptions(string options, string commandId) {}

	public static void TrackOptions(string options, string commandId, string code, string message) {}

	public static void TrackExperimentStart(string experiment, string group) {}

	public static void TrackExperimentEnd(string experiment) {}

	public static void PullCommands() {}

	public static void Flush() {}

	public static void SetDebugLoggingEnabled(bool debugLoggingEnabled) {}
}
