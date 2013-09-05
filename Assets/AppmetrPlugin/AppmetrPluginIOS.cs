using UnityEngine;
using System.Collections;

public class AppmetrPluginIOS : AppmetrWrapper
{
	#region	Interface to native implementation
	
	[DllImport("__Internal")]
	private static extern void _setupWithToken(string token);
	
	[DllImport("__Internal")]
	private static extern void _setupWithUserID(string userID);
	
	[DllImport("__Internal")]
	private static extern void _attachProperties(string properties);
	
	[DllImport("__Internal")]
	private static extern void _trackSession();
	
	[DllImport("__Internal")]
	private static extern void _trackSessionWithProperties(string properties);
	
	[DllImport("__Internal")]
	private static extern void _trackLevel(int level);
	
	[DllImport("__Internal")]
	private static extern void _trackLevel(int level, string properties);
	
	[DllImport("__Internal")]
	private static extern void _trackEvent(string _event);
	
	[DllImport("__Internal")]
	private static extern void _trackEvent(string _event, string properties);
	
	[DllImport("__Internal")]
	private static extern void _trackPayment(string payment);
	
	[DllImport("__Internal")]
	private static extern void _trackPayment(string payment, string properties);
	
	[DllImport("__Internal")]
	private static extern void _trackGameState(string state, string properties);
	
	[DllImport("__Internal")]
	private static extern void _trackOptions(string options, string commandId);
	
	[DllImport("__Internal")]
	private static extern void _trackOptions(string options, string commandId, string code, string message);
	
	[DllImport("__Internal")]
	private static extern void _trackExperimentStart(string experiment, string group);
	
	[DllImport("__Internal")]
	private static extern void _trackExperimentEnd(string experiment);
	
	[DllImport("__Internal")]
	private static extern void _pullCommands();

	[DllImport("__Internal")]
	private static extern void _flush();
	
	[DllImport("__Internal")]
	private static extern void _setDebugLoggingEnabled(bool debugLoggingEnabled);
	
	#endregion

	#region Declarations for non-native

	public static void SetupWithToken(string token)
	{
		_setupWithToken(token);
	}

	public static void SetupWithUserID(string userID)
	{
		_setupWithUserID(userID);
	}

	public static void AttachProperties(string properties)
	{
		_attachProperties(properties);
	}

	public static void TrackSession()
	{
		_trackSession();
	}

	public static void TrackSession(string properties)
	{
		_trackSessionWithProperties(properties);
	}

	public static void TrackLevel(int level)
	{
		_trackLevel(level);
	}

	public static void TrackLevel(int level, string properties)
	{
		_trackLevel(level, properties);
	}

	public static void TrackEvent(string _event)
	{
		_trackEvent(_event);
	}

	public static void TrackEvent(string _event, string properties)
	{
		_trackEvent(level, properties);
	}

	public static void TrackPayment(string payment)
	{
		_trackPayment(payment);
	}

	public static void TrackPayment(string payment, string properties)
	{
		_trackPayment(payment, properties);
	}

	public static void TrackGameState(string state, string properties)
	{
		_trackGameState(state, properties);
	}
	
	public static void TrackOptions(string options, string commandId)
	{
		_trackOptions(options, commandId);
	}
	
	public static void TrackOptions(string options, string commandId, string code, string message)
	{
		_trackOptions(options, commandId, code, message);
	}
	
	public static void TrackExperimentStart(string experiment, string group)
	{
		_trackExperimentStart(experiment, group);
	}

	public static void TrackExperimentEnd(string experiment)
	{
		_trackExperimentEnd(experiment);
	}

	public static void PullCommands()
	{
		_pullCommands();
	}

	public static void Flush()
	{
		_flush();
	}

	public static void SetDebugLoggingEnabled(bool debugLoggingEnabled)
	{
		_setDebugLoggingEnabled(debugLoggingEnabled);
	}
	
	#endregion
	
	void Start()
	{
		TrackSession();
	}
}
