using UnityEngine;
using System;

/// <summary>
/// Class provides access to the event command is received from the server.
/// </summary>
public class AppMetrCommandListener : MonoBehaviour
{
	// Fired when get a remote command from the server
	public static event Action<string> OnCommand;
	
	void OnExecuteCommand(string command)
	{
		var handler = OnCommand;
		if (handler != null)
		{
			handler(command);
		}
	}
}
