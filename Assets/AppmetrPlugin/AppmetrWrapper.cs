using UnityEngine;
using System.Collections;
using System.Json;

public class AppmetrWrapper : MonoBehaviour
{
	public static int GENERIC_ERROR = 0;
	public static int INVALID_COMMAND = 1;
	public static int UNSATISFIED_CONDITION = 2;

	public static void onExecuteCommand(string command)
	{
		try
		{
			JsonValue value = JsonValue.Parse(command);
			if (value != null)
			{
				onExecuteAppmetrCommand(value);
			}
		}
		catch(ArgumentException e)
		{
			setLastCommandError(GENERIC_ERROR, e.ToString());
		}
	}
	
	private static void onExecuteAppmetrCommand(JsonValue command)
	{
		JsonValue typeValue = command["type"];
		if (typeValue == null)
		{
			throw new ArgumentException("Invalid type of command");
		}

		string type = typeValue.ToString();
	}
	
	private static void setLastCommandError(int type, string message) {}
}
