using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using System;
using System.Collections;
using PrivateEar.Utils;

namespace PrivateEar {
	public class TutorialManager : MonoBehaviour {
		[SerializeField] Light2D globalLight;
		[SerializeField] Light2D spotLight;
		[SerializeField] float dimDuration = 2f;
		[SerializeField] GameObject raycastBlocker;

		private void Start() {
			raycastBlocker.SetActive(false);
			Tutorial();
		}

		public void Tutorial() {
			StopAllCoroutines();
			StartCoroutine(
				WaitForPlayerClickCObject(
					() => {
						spotLight.gameObject.SetActive(false);
						globalLight.intensity = 1f;
					}
				)
			);
		}

		public IEnumerator WaitForPlayerClickCObject(Action OnComplete) {
			CObject obj = FindObjectOfType<CObject>();

			obj.InteractOverride = true;
			BlockAllInteractions();

			Timer dimming = dimDuration;
			while (dimming) {
				globalLight.intensity = Mathf.Lerp(1f, 0.5f, 1 - dimming.TimeLeft / dimming.Duration);
				yield return null;
			}

			spotLight.transform.position = obj.transform.position;
			spotLight.gameObject.SetActive(true);

			yield return WaitForEvent(obj.OnClicked);

			obj.InteractOverride = false;
			AllowInteractions();

			OnComplete?.Invoke();
		}

		public IEnumerator WaitForEvent<T>(UnityEvent<T> unityEvent) {
			bool waiting = true;
			UnityAction<T> onAnyCrimeObjectClicked = (T obj) => {
				waiting = false;
			};
			unityEvent.AddListener(onAnyCrimeObjectClicked);
			while (waiting) yield return null;
			unityEvent.RemoveListener(onAnyCrimeObjectClicked);
		}

		public void BlockAllInteractions() {
			CObject.BlockingInteract++;
			raycastBlocker.gameObject.SetActive(true);
		}

		public void AllowInteractions() {
			CObject.BlockingInteract--;
			raycastBlocker.gameObject.SetActive(false);
		}
	}
}