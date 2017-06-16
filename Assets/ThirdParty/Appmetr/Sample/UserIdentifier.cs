using UnityEngine;

namespace Appmetr.Unity.Sample
{
	/// <summary>
	/// Outputs user identifier for debug info
	/// </summary>
	public class UserIdentifier : MonoBehaviour {

		void Start()
		{
			string userId = AppMetr.GetInstanceIdentifier();
			GetComponent<GUIText>().text += userId;
		}

		void Update() {
			if (Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
		}
	}
}

