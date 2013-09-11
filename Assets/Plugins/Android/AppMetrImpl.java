package com.appmetr.android;

import android.util.Log;
import com.appmetr.android.AppMetr;
import com.appmetr.android.integration.AppMetrHelper;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import java.util.Map;

public class AppMetrImpl
{
	private static Map keyMap = new Map();
	private static Map keyOptionalMap = new Map();
	
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
	
    public static void attachProperties()
	{
		AppMetr.attachProperties(new JSONObject(keyMap));
		removeKeys();
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
		AppMetr.trackPayment(new JSONObject(keyMap));
		removeKeys();
    }

    public static void trackPaymentWithProperties()
	{
		AppMetr.trackPayment(new JSONObject(keyMap), new JSONObject(keyOptionalMap));
		removeKeys();
    }

    public static void trackOptions(String commandId)
	{
    }

    public static void trackOptions(String commandId, String errorCode, String errorMessage)
	{
	}
}