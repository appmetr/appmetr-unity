using UnityEngine;
using System;

/// <summary>
/// Class provides access to the event command is received from the server.
/// </summary>
public class AppMetrCommandListener : AndroidJavaProxy
{
	// Fired when get a remote command from the server
	public static event Action<string> OnCommand;

	// Realize android interface
	public AppMetrCommandListener() : base("com.appmetr.android.AppMetrListener") {
	}
	
	void executeCommand(AndroidJavaObject command)
	{
		var handler = OnCommand;
		if (handler != null && command != null)
		{
			handler(command.Call<string>("toString"));
		}
	}
}
