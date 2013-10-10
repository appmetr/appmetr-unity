using UnityEngine;
using System;

/// <summary>
/// Class provides access to the event command is received from the server.
/// </summary>
[ExecuteInEditMode()]
public class AppMetrCommandListener : MonoBehaviour
{
	// Fired when get a remote command from the server
	public static event Action<string> OnCommand;
	
	void Awake()
	{
		transform.gameObject.name = "AppMetrCommandListener";
	}
	
	void OnExecuteCommand(string command)
	{
		var handler = OnCommand;
		if (handler != null)
		{
			handler(command);
		}
	}
}
