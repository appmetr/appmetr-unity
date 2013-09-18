﻿using UnityEngine;
using System;

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
