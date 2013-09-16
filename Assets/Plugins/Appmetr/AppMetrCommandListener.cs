using UnityEngine;
using System;

public class AppMetrCommandListener : MonoBehaviour
{
	public event Action<string> OnCommand;
	
	void OnExecuteCommand(string command)
	{
		Debug.Log("~~~~~~~~~~~~ Execute command:\n " + command);
		
		var handler = OnCommand;
		if (handler != null)
		{
			handler(command);
		}
	}
}
