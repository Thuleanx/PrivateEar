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
		[SerializeField, Required] Light2D confirmButtonHighlight;

		[Header("Lighting Animations")]
		[SerializeField] float dimDuration = 2f;
		[SerializeField] float spotLightPopDuration = .5f;

		[Header("Arrow Indicator")]
		[SerializeField] SpriteRenderer arrow;
		[SerializeField, Range(0, 2)] float endOffset = 1;
		
		[Header("Tutorial Text")]
		[SerializeField] GameObject introTutorialText;
		[SerializeField] GameObject markerTutorialText;
		[SerializeField] GameObject objectClickTutorialText;
		[SerializeField] GameObject objectZoomTutorialText;
		[SerializeField] GameObject backToSceneTutorialText;
		[SerializeField] GameObject dragTutorialText;
		[SerializeField] GameObject confirmTutorialText;
		[SerializeField] GameObject pauseTutorialText;

		private void Start() {
			raycastBlocker.SetActive(false);
			Tutorial();
		}

		public void Tutorial() {
			StopAllCoroutines();
			StartCoroutine(
				WaitForPlayerClickMarker(
				WaitForPlayerClickCObject(
				WaitForZoomedPreviewTutorial(
				WaitForPlayerDragDropMarker(
				WaitForPlayerConfirmChoices(null)
				))))
			);
		}

		public IEnumerator WaitForPlayerClickCObject(IEnumerator OnComplete) {
			// grab the object
			CObject obj = FindObjectOfType<CObject>();

			// block interactions with everything else but the object
			obj.InteractOverride = true;
			BlockAllInteractions();
			DisableAllMarkers();

			// dim the light
			yield return WaitForDimGlobalLight();

			objectClickTutorialText.gameObject.SetActive(true);

			// turns on the spotlight
			cobjectSpotLight.transform.position = obj.transform.position;
			cobjectSpotLight.intensity = 0;
			cobjectSpotLight.gameObject.SetActive(true);
			FadeinPopupLight(cobjectSpotLight);
			// yield return new WaitForSeconds(spotLightPopDuration);

			yield return WaitForEvent(obj.OnClicked);

			objectClickTutorialText.gameObject.SetActive(false);
			obj.InteractOverride = false;
			AllowInteractions();
			EnableAllMarkers();

			cobjectSpotLight.gameObject.SetActive(false);
			globalLight.intensity = 1f;

			yield return OnComplete;
		}
		public IEnumerator WaitForPlayerClickMarker(IEnumerator OnComplete) {
			List<Light2D> markerSpotlights = new List<Light2D>();
			List<UnityEvent<Marker>> markerClickedEvents = new List<UnityEvent<Marker>>();

			BlockAllCObjectInteractions();
			yield return WaitForDimGlobalLight();
			markerTutorialText.gameObject.SetActive(true);

			foreach (var marker in FindObjectsOfType<Marker>()) {
				Light2D spotLight = InstantiateMarkerLight(marker);
				markerSpotlights.Add(spotLight);
				FadeinPopupLight(spotLight);
				markerClickedEvents.Add(marker.OnClicked);
			}

			yield return WaitForEvents(markerClickedEvents);
			markerTutorialText.gameObject.SetActive(false);
			yield return null; // wait for at least 1 frame, kinda bad design but whatever
			while (SpectrogramManager.Instance.playing) yield return null;

			foreach (var markerSpotlight in markerSpotlights) 
				Destroy(markerSpotlight.gameObject);
			globalLight.intensity = 1;

			UnblockCObjectInteractions();

			yield return OnComplete;
		}
		public IEnumerator WaitForZoomedPreviewTutorial(IEnumerator OnComplete) {
			yield return WaitForDimGlobalLight();

			objectZoomTutorialText.gameObject.SetActive(true);
			zoomedPreviewObjectHighlight.gameObject.SetActive(true);
			FadeinPopupLight(zoomedPreviewObjectHighlight);
			yield return WaitForEvents(new List<UnityEvent>(){zoomedPreview.OnObjectClicked, zoomedPreview.OnDeactivate});
			zoomedPreviewObjectHighlight.gameObject.SetActive(false);
			objectZoomTutorialText.gameObject.SetActive(false);

			if (!zoomedPreview.Active) {
				// do it again
				globalLight.intensity = 1;
				yield return WaitForPlayerClickCObject(WaitForZoomedPreviewTutorial(OnComplete));
			} else {
				// they click on the object
				backToSceneTutorialText.gameObject.SetActive(true);
				// zoomedPreviewCloseHighlight.gameObject.SetActive(true);
				// FadeinPopupLight(zoomedPreviewCloseHighlight);
				yield return WaitForEvent(zoomedPreview.OnDeactivate);
				// zoomedPreviewCloseHighlight.gameObject.SetActive(false);
				backToSceneTutorialText.gameObject.SetActive(false);

				globalLight.intensity = 1;

				yield return OnComplete;
			}
		}
		public IEnumerator WaitForPlayerDragDropMarker(IEnumerator OnComplete) {
			Marker marker = FindObjectOfType<Marker>();
			CObject cobject = FindObjectOfType<CObject>();
			BlockAllCObjectInteractions();
			cobject.InteractOverride = true;

			yield return WaitForDimGlobalLight();

			DisableAllMarkers();
			marker.SetInteractable(true);

			dragTutorialText.gameObject.SetActive(true);
			cobjectSpotLight.gameObject.SetActive(true);
			cobjectSpotLight.transform.position = cobject.transform.position;
			Light2D markerSpotlight = InstantiateMarkerLight(marker);
			arrow.gameObject.SetActive(true);

			FadeinPopupLight(cobjectSpotLight);
			FadeinPopupLight(markerSpotlight);

			Vector2 markerToObjectDisplacement = (cobject.transform.position - markerSpotlight.transform.position);

			Quaternion markerToObjectRotation = Quaternion.LookRotation(Vector3.forward, markerToObjectDisplacement.normalized);
			arrow.transform.position = markerSpotlight.transform.position + (Vector3) (endOffset * markerToObjectDisplacement.normalized);
			arrow.transform.rotation = markerToObjectRotation;
			arrow.size = new Vector2(arrow.size.x, markerToObjectDisplacement.magnitude - 2*endOffset);

			yield return WaitForEvent(marker.OnCObjectAssigned);

			dragTutorialText.gameObject.SetActive(false);
			arrow.gameObject.SetActive(false);
			Destroy(markerSpotlight.gameObject);
			cobjectSpotLight.gameObject.SetActive(false);

			EnableAllMarkers();
			UnblockCObjectInteractions();
			cobject.InteractOverride = false;
			globalLight.intensity = 1;

			yield return OnComplete;
		}
		public IEnumerator WaitForPlayerConfirmChoices(IEnumerator OnComplete) {
			while (!GameMaster.Instance.IsAllMatched) yield return null;
			BlockAllCObjectInteractions();

			confirmTutorialText.gameObject.SetActive(true);
			confirmButtonHighlight.gameObject.SetActive(true);
			FadeinPopupLight(confirmButtonHighlight);

			yield return WaitForDimGlobalLight();
			DisableAllMarkers();

			// wait for player to confirm choice
			SubmitButton submitButton = FindObjectOfType<SubmitButton>();
			yield return WaitForEvent(submitButton.OnClicked);

			confirmTutorialText.gameObject.SetActive(false);
			confirmButtonHighlight.gameObject.SetActive(false);

			globalLight.intensity = 1;
			UnblockCObjectInteractions();
			EnableAllMarkers();
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

		public Light2D InstantiateMarkerLight(Marker marker) {
			// Vector2 offset = - (Vector2) Camera.main.ScreenToWorldPoint(marker.mouseHoverOffsetSS + (Vector2) Camera.main.WorldToScreenPoint(Vector3.zero));
			Vector2 offset = Vector2.up* 0.1f;
			GameObject spotLightObj = Instantiate(markerSpotlightPrefab, (Vector2) marker.markerObj.transform.position + offset, 
				Quaternion.identity);
			return spotLightObj.GetComponent<Light2D>();
		}
		public void FadeinPopupLight(Light2D light) {
			light.intensity = 0;
			DOVirtual.Float(0, 1, spotLightPopDuration, (x) => light.intensity = x);
		}
		public void BlockAllCanvasInteractions() => raycastBlocker.gameObject.SetActive(true);
		public void UnblockCanvasInteractions() => raycastBlocker.gameObject.SetActive(false);
		public void BlockAllCObjectInteractions() => CObject.BlockingInteract++;
		public void UnblockCObjectInteractions() => CObject.BlockingInteract--;

		public void DisableAllMarkers() {
			foreach (Marker otherMarker in FindObjectsOfType<Marker>()) otherMarker.SetInteractable(false);
		}
		public void EnableAllMarkers() {
			foreach (Marker otherMarker in FindObjectsOfType<Marker>()) otherMarker.SetInteractable(true);
		}
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