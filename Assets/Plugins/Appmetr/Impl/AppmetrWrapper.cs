#define USE_EXCEPTION

using UnityEngine;
using System;
using System.Collections;

public class AppmetrWrapper
{
	public static int GENERIC_ERROR = 0;
	public static int INVALID_COMMAND = 1;
	public static int UNSATISFIED_CONDITION = 2;

	public static void onExecuteCommand(string command)
	{
#if USE_EXCEPTION
		try
		{
			decodeCommand(command);
		}
		catch(ArgumentException e)
		{
			setLastCommandError(GENERIC_ERROR, e.ToString());
		}
#else
		decodeCommand(command);
#endif		
	}
	
	private static void decodeCommand(string command)
	{
		Hashtable value = (Hashtable)MiniJSON.jsonDecode(command);
		if (value != null)
		{
			onExecuteAppmetrCommand(value);
		}
	}
	
	private static void onExecuteAppmetrCommand(Hashtable command)
	{
		string type = (string)command["type"];
	}
	
	private static void setLastCommandError(int type, string message) {}
}
