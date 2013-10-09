﻿#if UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppmetrPluginDefault
{
	public static void SetupWithToken(string token) {}

	public static void TrackSession() {}

	public static void TrackSession(IDictionary<string, string> properties) {}

	public static void TrackLevel(int level) {}

	public static void TrackLevel(int level, IDictionary<string, string> properties) {}

	public static void TrackEvent(string _event) {}

	public static void TrackEvent(string _event, IDictionary<string, string> properties) {}

	public static void TrackPayment(IDictionary<string, string> payment) {}

	public static void TrackPayment(IDictionary<string, string> payment, IDictionary<string, string> properties) {}

	public static void AttachProperties(IDictionary<string, string> properties) {}

	public static void TrackOptions(IDictionary<string, string> options, string commandId) {}

	public static void TrackOptions(IDictionary<string, string> options, string commandId, string code, string message) {}
}
#endif
