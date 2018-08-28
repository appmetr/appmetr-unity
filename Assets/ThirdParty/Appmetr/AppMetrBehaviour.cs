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
		/// Application identifier in AppMetr service
		/// </summary>
		[SerializeField]
		private string _token = "demo_token";

		/// <summary>
		/// Is this game object don't be destroyed by scene reload
		/// and has single instance
		/// </summary>
		public bool  SingleUnloadableInstance;

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
			AppMetr.Setup(_token);
		}

		void OnApplicationPause(bool pauseStatus) {
			AppMetr.OnPause(pauseStatus);
		}

		void OnApplicationQuit() {
			AppMetr.OnPause(true);
		}
	}
}