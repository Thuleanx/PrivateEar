using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using FMODUnity;

namespace PrivateEar {
	[RequireComponent(typeof(RectTransform))]
	public class SpectrogramManager : MonoBehaviour {
		public static SpectrogramManager Instance;

		public bool playing;
		[ReorderableList, SerializeField] List<Marker> allMarkers = new List<Marker>();
		FMOD.Studio.EventInstance instance;
		[SerializeField, Required] RectTransform timelineMarker;

		public RectTransform rectTransform {get; private set; }
		public Canvas Canvas {get; private set; }

		private void Awake() {
			Instance = this;
			rectTransform = GetComponent<RectTransform>();
			Canvas = GetComponentInParent<Canvas>();
		}

		private void Start() {
			foreach (var marker in allMarkers) marker.OnClicked.AddListener(PlayMarker);
			allMarkers.Sort((x, y) => x.transform.position.x < y.transform.position.x ? -1 : 1);
			timelineMarker.gameObject.SetActive(false);
		}

		void PlayMarker(Marker marker) {
			playing = true;
			StartCoroutine(_PlayMarker(marker));
		}

		IEnumerator _PlayMarker(Marker marker) {
			marker.sfxInstance.start();

			int index = allMarkers.FindIndex(0, allMarkers.Count, (x) => (x == marker));

			Vector2 nxtMarkerPos = Vector2.zero;
			if (index == allMarkers.Count - 1) {
				Vector3[] corners = new Vector3[4];
				rectTransform.GetWorldCorners(corners);
				nxtMarkerPos = (corners[2] + corners[3])/2;
			} else nxtMarkerPos = allMarkers[index+1].transform.position;

			SetMarkersInteratable(false);
			timelineMarker.gameObject.SetActive(true);
			while (IsPlaying(marker.sfxInstance)) {
				int timelinePosInt = 0;
				marker.sfxInstance.getTimelinePosition(out timelinePosInt);
				float timelinePosition = (float) timelinePosInt / GetTrackDuration(marker.sfxInstance);

				Vector2 timelineMarkerPos = Vector2.Lerp(allMarkers[index].transform.position, nxtMarkerPos, timelinePosition);
				timelineMarker.transform.position = timelineMarkerPos;

				yield return null;
			}
			timelineMarker.gameObject.SetActive(false);

			SetMarkersInteratable(true);

			playing = false;
		}

		bool IsPlaying(FMOD.Studio.EventInstance instance) {
			FMOD.Studio.PLAYBACK_STATE state;
			instance.getPlaybackState(out state);
			return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
		}
		public int GetTrackDuration(FMOD.Studio.EventInstance track) {
			FMOD.Studio.EventDescription description;
			if (track.getDescription(out description) != FMOD.RESULT.OK) {
				Debug.LogError(string.Format("Cannot seem to get duration of track"));
				return -1;
			}
			int length;
			if (description.getLength(out length) != FMOD.RESULT.OK)
				return -1;
			return length;
		}
		public void SetMarkersInteratable(bool interactable) {
			foreach (Marker marker in allMarkers) marker.SetInteractable(interactable);
		}

		public void TurnOnMarkers() {
			foreach (Marker marker in allMarkers) {
				marker.GetComponentInChildren<Vibrate>().StartVibrate();
			}
		}
	}
}