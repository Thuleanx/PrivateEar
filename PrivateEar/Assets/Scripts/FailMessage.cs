using UnityEngine;
using System.Collections;

namespace PrivateEar {
	public class FailMessage : MonoBehaviour {
		public void GenerateFailMessage() {
			gameObject.SetActive(true);
			StartCoroutine(FailMessageLifetime());
		}

		IEnumerator FailMessageLifetime() {
			yield return new WaitForSeconds(2f);
			gameObject.SetActive(false);
		}
	}
}