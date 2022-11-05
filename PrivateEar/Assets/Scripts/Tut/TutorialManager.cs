using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using System;
using System.Collections;
using System.Collections.Generic;
using PrivateEar.Utils;
using DG.Tweening;
using NaughtyAttributes;

namespace PrivateEar {
	public class TutorialManager : MonoBehaviour {
		[SerializeField] GameObject raycastBlocker;
		[SerializeField] GameObject markerSpotlightPrefab;
		[SerializeField] ZoomedPreview zoomedPreview;

		[Header("Tutorial Lights")]
		[SerializeField, Required] Light2D globalLight;
		[SerializeField, Required] Light2D cobjectSpotLight;
		[SerializeField, Required] Light2D zoomedPreviewObjectHighlight;
		[SerializeField, Required] Light2D zoomedPreviewCloseHighlight;

		[Header("Lighting Animations")]
		[SerializeField] float dimDuration = 2f;
		[SerializeField] float spotLightPopDuration = .5f;

		private void Start() {
			raycastBlocker.SetActive(false);
			Tutorial();
		}

		public void Tutorial() {
			StopAllCoroutines();
			StartCoroutine(
				WaitForPlayerClickMarker(WaitForPlayerClickCObject(WaitForZoomedPreviewTutorial(null)))
			);
		}

		public IEnumerator WaitForPlayerClickCObject(IEnumerator OnComplete) {
			// grab the object
			CObject obj = FindObjectOfType<CObject>();

			// block interactions with everything else but the object
			obj.InteractOverride = true;
			BlockAllInteractions();

			// dim the light
			yield return WaitForDimGlobalLight();

			// turns on the spotlight
			cobjectSpotLight.transform.position = obj.transform.position;
			cobjectSpotLight.intensity = 0;
			cobjectSpotLight.gameObject.SetActive(true);
			FadeinPopupLight(cobjectSpotLight);
			// yield return new WaitForSeconds(spotLightPopDuration);

			yield return WaitForEvent(obj.OnClicked);

			obj.InteractOverride = false;
			AllowInteractions();

			cobjectSpotLight.gameObject.SetActive(false);
			globalLight.intensity = 1f;

			yield return OnComplete;
		}
		public IEnumerator WaitForPlayerClickMarker(IEnumerator OnComplete) {
			List<Light2D> markerSpotlights = new List<Light2D>();
			List<UnityEvent> markerClickedEvents = new List<UnityEvent>();

			BlockAllCObjectInteractions();
			yield return WaitForDimGlobalLight();

			foreach (var marker in FindObjectsOfType<Marker>()) {
				// Vector2 offset = - (Vector2) Camera.main.ScreenToWorldPoint(marker.mouseHoverOffsetSS + (Vector2) Camera.main.WorldToScreenPoint(Vector3.zero));
				Vector2 offset = Vector2.down * 0.75f;
				GameObject spotLightObj = Instantiate(markerSpotlightPrefab, (Vector2) marker.markerObj.transform.position + offset, 
					Quaternion.identity);
				Light2D spotLight = spotLightObj.GetComponent<Light2D>();
				markerSpotlights.Add(spotLight);
				FadeinPopupLight(spotLight);
				markerClickedEvents.Add(marker.OnClicked);
			}

			yield return WaitForEvents(markerClickedEvents);

			foreach (var markerSpotlight in markerSpotlights) 
				Destroy(markerSpotlight.gameObject);
			globalLight.intensity = 1;

			UnblockCObjectInteractions();

			yield return OnComplete;
		}
		public IEnumerator WaitForZoomedPreviewTutorial(IEnumerator OnComplete) {
			yield return WaitForDimGlobalLight();

			zoomedPreviewObjectHighlight.gameObject.SetActive(true);
			FadeinPopupLight(zoomedPreviewObjectHighlight);
			yield return WaitForEvents(new List<UnityEvent>(){zoomedPreview.OnObjectClicked, zoomedPreview.OnDeactivate});
			zoomedPreviewObjectHighlight.gameObject.SetActive(false);

			if (!zoomedPreview.Active) {
				// do it again
				globalLight.intensity = 1;
				yield return WaitForPlayerClickCObject(WaitForZoomedPreviewTutorial(OnComplete));
			} else {
				zoomedPreviewCloseHighlight.gameObject.SetActive(true);
				FadeinPopupLight(zoomedPreviewCloseHighlight);
				yield return WaitForEvent(zoomedPreview.OnDeactivate);
				zoomedPreviewCloseHighlight.gameObject.SetActive(false);

				globalLight.intensity = 1;

				yield return OnComplete;
			}
		}
		public IEnumerator WaitForPlayerDragDropMarker(IEnumerator OnComplete) {
			yield return OnComplete;
		}

		public IEnumerator WaitForPlayerConfirmChoices(IEnumerator OnComplete) {
			yield return OnComplete;
		}

		public IEnumerator WaitForDimGlobalLight() {
			DOVirtual.Float(1, 0.5f, dimDuration, (x) => globalLight.intensity = x);
			yield return new WaitForSeconds(dimDuration);
		}

		public IEnumerator WaitForEvents(List<UnityEvent> unityEvents) {
			bool waiting = true;
			UnityAction onEventInvoked = () => {
				waiting = false;
			};
			foreach (var e in unityEvents) e.AddListener(onEventInvoked);
			while (waiting) yield return null;
			foreach (var e in unityEvents) e.RemoveListener(onEventInvoked);
		}
		public IEnumerator WaitForEvents<T>(List<UnityEvent<T>> unityEvents) {
			bool waiting = true;
			UnityAction<T> onEventInvoked = (T obj) => {
				waiting = false;
			};
			foreach (var e in unityEvents) e.AddListener(onEventInvoked);
			while (waiting) yield return null;
			foreach (var e in unityEvents) e.RemoveListener(onEventInvoked);
		}
		public IEnumerator WaitForEvent<T>(UnityEvent<T> unityEvent) {
			yield return WaitForEvents(new List<UnityEvent<T>>{unityEvent});
		}
		public IEnumerator WaitForEvent(UnityEvent unityEvent) {
			yield return WaitForEvents(new List<UnityEvent>{unityEvent});
		}

		public void FadeinPopupLight(Light2D light) {
			light.intensity = 0;
			DOVirtual.Float(0, 1, spotLightPopDuration, (x) => light.intensity = x);
		}
		public void BlockAllCanvasInteractions() => raycastBlocker.gameObject.SetActive(true);
		public void UnblockCanvasInteractions() => raycastBlocker.gameObject.SetActive(false);
		public void BlockAllCObjectInteractions() => CObject.BlockingInteract++;
		public void UnblockCObjectInteractions() => CObject.BlockingInteract--;

		public void BlockAllInteractions() {
			BlockAllCanvasInteractions();
			BlockAllCObjectInteractions();
		}

		public void AllowInteractions() {
			UnblockCanvasInteractions();
			UnblockCObjectInteractions();
		}
	}
}