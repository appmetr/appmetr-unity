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
#if !UNITY_EDITOR
			yield return new WaitUntil(() => AppMetr.GetDeviceKey() != null);
#endif
			string userId = AppMetr.GetInstanceIdentifier();
			string deviceKey = AppMetr.GetDeviceKey() ?? "null";
			GetComponent<GUIText>().text += string.Format("UID: {0}\nDevice key: {1}", userId, deviceKey);
			yield break;
		}

		void Update() {
			if (Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
		}
	}
}

