using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class AppmetrPluginAndroid : MonoBehaviour
{
	private static AndroidJavaObject currentActivity;
	
	private static AndroidJavaClass tapjoyConnect;
	private static AndroidJavaClass TapjoyConnect
	{
		get
		{
			if (tapjoyConnect == null)
			{
				Debug.Log("C#: Loading AppmetrPlugin");
			
				AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
				currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
				
				tapjoyConnect = new AndroidJavaClass("com.appmetr.android.AppMetr");
			}
			
			return tapjoyConnect;
		}
	}
	
	public static void SetupWithToken(string token)
	{
	}

	public static void SetupWithUserID(string userID)
	{
	}

	public static void AttachProperties(string properties)
	{
	}

	public static void TrackSession()
	{
		TapjoyConnect.CallStatic("trackSession");
	}

	public static void TrackSession(string properties)
	{
	}

	public static void TrackLevel(int level)
	{
		TapjoyConnect.CallStatic("trackLevel", level);
	}

	public static void TrackLevel(int level, string properties)
	{
	}

	public static void TrackEvent(string _event)
	{
		TapjoyConnect.CallStatic("trackEvent", _event);
	}

	public static void TrackEvent(string _event, string properties)
	{
	}

	public static void TrackPayment(string payment)
	{
	}

	public static void TrackPayment(string payment, string properties)
	{
	}

	public static void TrackGameState(string state, string properties)
	{
	}

	public static void TrackOptions(string options, string commandId)
	{
	}

	public static void TrackOptions(string options, string commandId, string code, string message)
	{
	}

	public static void TrackExperimentStart(string experiment, string group)
	{
		TapjoyConnect.CallStatic("trackExperimentStart", experiment, group);
	}

	public static void TrackExperimentEnd(string experiment)
	{
		TapjoyConnect.CallStatic("trackExperimentEnd", experiment);
	}

	public static void PullCommands()
	{
		TapjoyConnect.CallStatic("pullCommands");
	}

	public static void Flush()
	{
		TapjoyConnect.CallStatic("flush");
	}

	public static void SetDebugLoggingEnabled(bool debugLoggingEnabled)
	{
	}
}
