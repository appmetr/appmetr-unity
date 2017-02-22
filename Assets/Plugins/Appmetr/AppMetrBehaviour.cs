using UnityEngine;
using System;

/// <summary>
/// The MonoBehaviour class.
/// Listen application lifecircle events
/// </summary>
public class AppMetrBehaviour : MonoBehaviour {

	/// <summary>
	/// Fired when get a remote command from the server
	/// </summary>
	public static event Action<string> OnCommand;

	/// <summary>
	/// Application identifier in AppMetr service
	/// </summary>
	[SerializeField]
	private string token = "demo_token";

	/// <summary>
	/// Is this game object don't be destroyed by scene reload
	/// and has single instance
	/// </summary>
	public bool  SingleUnloadableInstance = false;

	/// <summary>
	/// Register AppMetrCommandListener or not on setup
	/// </summary>
	[SerializeField]
	private bool UseRemoteCommands = false;

	void Awake() {
		if(SingleUnloadableInstance) {
			if (FindObjectsOfType(GetType()).Length > 1) {
				Destroy(gameObject);
			} else {
				DontDestroyOnLoad(transform.gameObject);
			}
		}
	}

	void Start()
	{
		AppMetr.Setup(token, UseRemoteCommands ? gameObject.name : null);
	}

	void OnApplicationPause(bool pauseStatus) {
		AppMetr.OnPause(pauseStatus);
	}

	void OnApplicationQuit() {
		AppMetr.OnPause(true);
	}

	void OnAppMetrCommand(string command)
	{
		var handler = OnCommand;
		if (handler != null)
		{
			handler(command);
		}
	}
}
