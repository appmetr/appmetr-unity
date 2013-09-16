package com.appmetr.android.impl;

import org.json.JSONObject;
import android.util.Log;
import com.appmetr.android.AppMetr;
import com.appmetr.android.AppMetrListener;
import com.unity3d.player.UnityPlayer;

public class AppMetrListenerImpl implements AppMetrListener
{
	private final static String TAG = "AppMetrListenerImpl";

	@Override
	public void executeCommand(final JSONObject command) throws Throwable
	{
		Log.i(TAG, "~~~~~~~~~~~~ Execute command:\n " + command.toString());
		UnityPlayer.UnitySendMessage("AppMetrCommandListener", "OnExecuteCommand", command.toString());
	}
}
