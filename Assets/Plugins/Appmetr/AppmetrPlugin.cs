using UnityEngine;
using System.Collections;

#if UNITY_ANDROID && !UNITY_EDITOR
using AppmetrPlatformPlugin = AppmetrPluginAndroid;
#elif UNITY_IPHONE && !UNITY_EDITOR
using AppmetrPlatformPlugin = AppmetrPluginIOS;
#else
using AppmetrPlatformPlugin = AppmetrPluginDefault;
#endif

public class AppmetrPlugin : AppmetrWrapper
{
	public static void SetupWithToken(string token)
	{
		AppmetrPlatformPlugin.SetupWithToken(token);
	}

	public static void SetupWithUserID(string userID)
	{
		AppmetrPlatformPlugin.SetupWithUserID(userID);
	}
	
	public static void AttachProperties(string properties)
	{
		AppmetrPlatformPlugin.AttachProperties(properties);
	}
	
	public static void TrackSession()
	{
		AppmetrPlatformPlugin.TrackSession();
	}
	
	public static void TrackSession(string properties)
	{
		AppmetrPlatformPlugin.TrackSession(properties);
	}
	
	public static void TrackLevel(int level)
	{
		AppmetrPlatformPlugin.TrackLevel(level);
	}
	
	public static void TrackLevel(int level, string properties)
	{
		AppmetrPlatformPlugin.TrackLevel(level, properties);
	}
	
	public static void TrackEvent(string _event)
	{
		AppmetrPlatformPlugin.TrackEvent(_event);
	}
	
	public static void TrackEvent(string _event, string properties)
	{
		AppmetrPlatformPlugin.TrackEvent(_event, properties);
	}
	
	public static void TrackPayment(string payment)
	{
		AppmetrPlatformPlugin.TrackPayment(payment);
	}
	
	public static void TrackPayment(string payment, string properties)
	{
		AppmetrPlatformPlugin.TrackPayment(payment, properties);
	}
	
	public static void TrackGameState(string state, string properties)
	{
		AppmetrPlatformPlugin.TrackGameState(state, properties);
	}
	
	public static void TrackOptions(string options, string commandId)
	{
		AppmetrPlatformPlugin.TrackOptions(options, commandId);
	}
	
	public static void TrackOptions(string options, string commandId, string code, string message)
	{
		AppmetrPlatformPlugin.TrackOptions(options, commandId, code, message);
	}
	
	public static void TrackExperimentStart(string experiment, string group)
	{
		AppmetrPlatformPlugin.TrackExperimentStart(experiment, group);
	}
	
	public static void TrackExperimentEnd(string experiment)
	{
		AppmetrPlatformPlugin.TrackExperimentEnd(experiment);
	}
	
	public static void PullCommands()
	{
		AppmetrPlatformPlugin.PullCommands();
	}
	
	public static void Flush()
	{
		AppmetrPlatformPlugin.Flush();
	}
}
