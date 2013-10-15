package com.appmetr.android.impl;

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
import com.appmetr.android.impl.AppMetrListenerImpl;

public class AppMetrImpl
{
	private final static String TAG = "AppMetrImpl";
	
	private static Map<String, Object> keyMap = new HashMap<String, Object>();
	private static Map<String, Object> keyOptionalMap = new HashMap<String, Object>();
	
	private static void removeKeys()
	{
		keyMap.clear();
		keyOptionalMap.clear();
	}
	
    public static void setKeyValue(String key, Object value)
	{
		keyMap.put(key, value);
    }
	
    public static void setKeyValueOptional(String key, Object value)
	{
		keyOptionalMap.put(key, value);
	}
	
	public static void setup(String token, Context context)
	{
		try
		{
			AppMetr.setup(token, context, new AppMetrListenerImpl());
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

    public static void trackSessionWithProperties(String properties)
	{
		try
		{
			AppMetr.trackSession(new JSONObject(properties));
		}
		catch (JSONException e)
		{
			Log.e(TAG, e.getMessage());
		}
	}

    public static void trackLevel(int level)
	{
		AppMetr.trackLevel(level);
    }

    public static void trackLevelWithProperties(int level, String properties)
	{
		try
		{
			AppMetr.trackLevel(level, new JSONObject(properties));
 		}
		catch (JSONException e)
		{
			Log.e(TAG, e.getMessage());
		}
   }

    public static void trackEvent(String event)
	{
		AppMetr.trackEvent(event);
    }

    public static void trackEventWithProperties(String event, String properties)
	{
		try
		{
			AppMetr.trackEvent(event, new JSONObject(properties));
  		}
		catch (JSONException e)
		{
			Log.e(TAG, e.getMessage());
		}
   }

    public static void trackPayment(String payment)
	{
		try
		{
			AppMetr.trackPayment(new JSONObject(payment));
		}
		catch (DataFormatException e)
		{
			Log.e(TAG, e.getMessage());
		}
		catch (JSONException e)
		{
			Log.e(TAG, e.getMessage());
		}
    }

    public static void trackPaymentWithProperties(String payment, String properties)
	{
		try
		{
			AppMetr.trackPayment(new JSONObject(payment), new JSONObject(properties));
		}
		catch (DataFormatException e)
		{
			Log.e(TAG, e.getMessage());
		}
		catch (JSONException e)
		{
			Log.e(TAG, e.getMessage());
		}
    }

    public static void attachPropertiesNull()
	{
		AppMetr.attachProperties();
	}

    public static void attachProperties(String properties)
	{
		try
		{
			AppMetr.attachProperties(new JSONObject(properties));
		}
		catch (JSONException e)
		{
			Log.e(TAG, e.getMessage());
		}
	}

    public static void trackOptions(String options, String commandId)
	{
    }

    public static void trackOptions(String options, String commandId, String errorCode, String errorMessage)
	{
	}

    public static void trackExperimentStart(String experiment, String groupId)
	{
		AppMetr.trackExperimentStart(experiment, groupId);
    }

    public static void trackExperimentEnd(String experiment)
	{
		AppMetr.trackExperimentEnd(experiment);
    }

    public static void identify(String userId)
	{
		AppMetr.identify(userId);
    }

    public static void flush()
	{
		AppMetr.flush();
    }

    private static JSONObject paymentWithPaymentProcessor(Map<String, Object> payment) throws JSONException
	{
		JSONObject ret = new JSONObject(payment);
        if (!ret.has("processor"))
		{
            ret.put("processor", "google_checkout");
        }
        return ret;
    }
}
