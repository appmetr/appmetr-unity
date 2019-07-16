using System.Collections;
using UnityEngine;

namespace Appmetr.Unity.Sample
{
	/// <summary>
	/// Outputs user identifier for debug info
	/// </summary>
	public class UserIdentifier : MonoBehaviour {
		IEnumerator Start()
		{
			string userId = AppMetr.GetInstanceIdentifier();
			string deviceKey = AppMetr.GetDeviceKey() ?? "null";
			GetComponent<GUIText>().text += string.Format("UID: {0}\nDevice key: {1}", userId, deviceKey);
			Debug.Log("Device key 1: " + deviceKey);
			yield return new WaitForSeconds(4);
			deviceKey = AppMetr.GetDeviceKey() ?? "null";
			Debug.Log("Device key 2: " + deviceKey);
		}

		void Update() {
			if (Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
		}
	}
}

