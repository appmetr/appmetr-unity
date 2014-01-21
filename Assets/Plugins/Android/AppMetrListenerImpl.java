package com.appmetr.unity;

import org.json.JSONObject;
import com.appmetr.android.AppMetrListener;
import com.unity3d.player.UnityPlayer;

public class AppMetrListenerImpl implements AppMetrListener
{
	private final static String TAG = "AppMetrListenerImpl";

	@Override
	public void executeCommand(final JSONObject command) throws Throwable
	{
		UnityPlayer.UnitySendMessage("AppMetrCommandListener", "OnExecuteCommand", command.toString());
	}
}
