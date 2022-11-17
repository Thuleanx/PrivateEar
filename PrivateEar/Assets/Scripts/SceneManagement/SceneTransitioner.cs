using UnityEngine;
using NaughtyAttributes;
using PrivateEar.Utils;
using System.Collections;

namespace PrivateEar {
	public class SceneTransitioner : MonoBehaviour {
		[SerializeField] SceneReference sceneReference;
		public float Delay;

		public void Transition() { StartCoroutine(_Transition()); }
		IEnumerator _Transition() {
			yield return new WaitForSecondsRealtime(Delay);
			sceneReference.LoadScene();
		}
	}
}