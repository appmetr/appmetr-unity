import org.json.JSONObject;
import android.util.Log;
import com.appmetr.android.AppMetr;
import com.appmetr.android.AppMetrListener;

public class AppMetrIntegration implements AppMetrListener
{
	private static final String TAG = "AppMetrIntegration";

	private static Exception msLastCommandError;

	public static final int INVALID_COMMAND = 1;
	public static final int UNSATISFIED_CONDITION = 2;

	public static void connect()
	{
		try
		{
			AppMetr.getInstance().setListener(new AppMetrIntegration());
		}
		catch(final Throwable t)
		{
			Log.e(TAG, "connect failed", t);
		}
	}

	public static void disconnect()
	{
		try
		{
			AppMetr.getInstance().setListener(null);
		}
		catch(final Throwable t)
		{
			Log.e(TAG, "disconnect", t);
		}
	}

	@Override
	public void executeCommand(final JSONObject command) throws Throwable
	{
		msLastCommandError = null;

		// .. onExecuteCommand(command.toString());

		if (msLastCommandError != null)
		{
			throw msLastCommandError;
		}
	}

	public static void setLastCommandError(final int type, final String message)
	{
	}
}
