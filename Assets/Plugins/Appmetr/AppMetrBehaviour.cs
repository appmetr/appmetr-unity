using UnityEngine;
using System.Collections;

/// <summary>
/// The MonoBehaviour class.
/// Listen application lifecircle events
/// </summary>
public class AppMetrBehaviour : MonoBehaviour {

	/// <summary>
	/// Application identifier in AppMetr service
	/// </summary>
	[SerializeField]
	private string token = "demo_token";

	/// <summary>
	/// Is this game object don't be destroyed by scene reload
	/// and has single instance
	/// </summary>
	public bool  Undeletable = false;

	void Awake() {
		if(Undeletable) {
			DontDestroyOnLoad(transform.gameObject);
			if (FindObjectsOfType(GetType()).Length > 1) {
				Destroy(gameObject);
			}
		}
	}

	void Start()
	{
		AppMetr.Setup(token);
	}

	void OnApplicationPause(bool pauseStatus) {
		AppMetr.OnPause(pauseStatus);
	}

	void OnApplicationQuit() {
		AppMetr.OnPause(true);
	}
}
