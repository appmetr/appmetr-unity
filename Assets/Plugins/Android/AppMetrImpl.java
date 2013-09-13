//package com.appmetr.android.impl;

import android.util.Log;
import com.appmetr.android.AppMetr;
import com.appmetr.android.integration.AppMetrHelper;
import com.appmetr.android.AppMetrListener;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import java.util.Map;
import java.util.HashMap;
import java.util.zip.DataFormatException;
import java.lang.SecurityException;
import android.content.Context;
import com.unity3d.player.UnityPlayer;

public class AppMetrImpl
{
	private final static String TAG = "AppMetrImpl";
	
	private static Map<String, String> keyMap = new HashMap<String, String>();
	private static Map<String, String> keyOptionalMap = new HashMap<String, String>();

	private static void removeKeys()
	{
		keyMap.clear();
		keyOptionalMap.clear();
	}
	
    public static void setKey(String key, String value)
	{
		keyMap.put(key, value);
    }
	
    public static void setKeyOptional(String key, String value)
	{
		keyOptionalMap.put(key, value);
	}
	
	public static void setup(String token, Context context)
	{
		try
		{
			//AppMetr.setup(token, context, null);
			AppMetr.setup(token, context, new AppMetrListener()	{
				@Override
				public void executeCommand(JSONObject command) throws Throwable {
					UnityPlayer.UnitySendMessage("AppMetrWrapper", "onExecuteCommand", command.toString());
				}
			});
		}	
		catch (DataFormatException e)
		{
			Log.e(TAG, e.getMessage());
		}
		catch (SecurityException e)
		{
			Log.e(TAG, e.getMessage());
		}
	}

    public static void trackSession()
	{
		AppMetr.trackSession();
    }

    public static void trackSessionWithProperties()
	{
		AppMetr.trackSession(new JSONObject(keyMap));
		removeKeys();
	}

    public static void trackLevel(int level)
	{
		AppMetr.trackLevel(level);
    }

    public static void trackLevelWithProperties(int level)
	{
		AppMetr.trackLevel(level, new JSONObject(keyMap));
		removeKeys();
    }

    public static void trackEvent(String event)
	{
		AppMetr.trackEvent(event);
    }

    public static void trackEventWithProperties(String event)
	{
		AppMetr.trackEvent(event, new JSONObject(keyMap));
		removeKeys();
    }

    public static void trackPayment()
	{
		try
		{
			AppMetr.trackPayment(paymentWithPaymentProcessor(keyMap));
		}
		catch (DataFormatException e)
		{
			Log.e(TAG, e.getMessage());
		}
		catch (JSONException e)
		{
			Log.e(TAG, e.getMessage());
		}
		removeKeys();
    }

    public static void trackPaymentWithProperties()
	{
		try
		{
			AppMetr.trackPayment(paymentWithPaymentProcessor(keyMap), new JSONObject(keyOptionalMap));
		}
		catch (DataFormatException e)
		{
			Log.e(TAG, e.getMessage());
		}
		catch (JSONException e)
		{
			Log.e(TAG, e.getMessage());
		}
		removeKeys();
    }

    public static void trackOptions(String commandId)
	{
    }

    public static void trackOptions(String commandId, String errorCode, String errorMessage)
	{
	}

    private static JSONObject paymentWithPaymentProcessor(Map<String, String> payment) throws JSONException
	{
		JSONObject ret = new JSONObject(payment);
        if (!ret.has("processor"))
		{
            ret.put("processor", "google_checkout");
        }
        return ret;
    }
}
