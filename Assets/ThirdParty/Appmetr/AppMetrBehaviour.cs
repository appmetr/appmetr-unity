using UnityEngine;
using System;

namespace Appmetr.Unity
{
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
		private string _token = "demo_token";

		/// <summary>
		/// Is this game object don't be destroyed by scene reload
		/// and has single instance
		/// </summary>
		public bool  SingleUnloadableInstance;

		/// <summary>
		/// Register AppMetrCommandListener or not on setup
		/// </summary>
		[SerializeField]
		private bool _useRemoteCommands;

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
			AppMetr.Setup(_token, _useRemoteCommands ? gameObject.name : null);
		}

		void OnApplicationPause(bool pauseStatus) {
			AppMetr.OnPause(pauseStatus);
		}

		void OnApplicationQuit() {
			AppMetr.OnPause(true);
		}

		public void OnAppMetrCommand(string command)
		{
			var handler = OnCommand;
			if (handler != null)
			{
				handler(command);
			}
		}
	}
}