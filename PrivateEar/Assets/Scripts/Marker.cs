using System;
using UnityEngine;
using FMODUnity;
using UnityEngine.EventSystems;
using NaughtyAttributes;

namespace PrivateEar {
	/// <summary>
	/// Marker objects
	/// </summary>
	public class Marker : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler  {
		public Canvas canvas { get; private set;  }

		[SerializeField, Required] CObject correctMatching;
		[SerializeField] EventReference sound;

		[SerializeField, ReadOnly] CObject _matchedObj;

		// juice objects
		[SerializeField, Required] RectTransform markerObj;
		[SerializeField] Vector2 mouseHoverOffsetSS;
		Vector2 markerObjPos;


		public CObject MatchedObject {
			get => _matchedObj;
			set {
				if (_matchedObj) _matchedObj.MatchedMarker = null;
				if (value && value.MatchedMarker) value.MatchedMarker._matchedObj = null;
				if (value) value.MatchedMarker = this;
				_matchedObj = value;
			}
		}

		private void Awake() { canvas = GetComponentInParent<Canvas>(); }
		private void Start() {
			GameMaster.Instance?.RegisterMarker(this);
			markerObjPos = markerObj.anchoredPosition;
		}

		public void OnClick() {
			try {
				RuntimeManager.PlayOneShot(sound);
			} catch (Exception e) {
				Debug.LogError(e);
			}
		}

		public void OnBeginDrag(PointerEventData eventData) {
			Vector2 pointerPosWS = canvas.worldCamera.ScreenToWorldPoint(eventData.position / canvas.scaleFactor + mouseHoverOffsetSS);
			markerObj.position = pointerPosWS;
		}

		public void OnDrag(PointerEventData eventData) {
			markerObj.anchoredPosition += eventData.delta / canvas.scaleFactor;
		}

		public void OnEndDrag(PointerEventData eventData) {
			if (CObject.HoveredObject) {
				// then we have a matching
				MatchedObject = CObject.HoveredObject;
				markerObj.anchoredPosition = markerObjPos;
			} else {
				// then we didn't drop it on any matchable element
				MatchedObject = null;
				markerObj.anchoredPosition = markerObjPos;
			}
		}

		public bool IsCorrectMatch => _matchedObj == correctMatching;
		public bool IsMatched => _matchedObj;

		private void OnDrawGizmos() {
			Gizmos.color = Color.red;
			if (_matchedObj) Gizmos.DrawLine(transform.position, _matchedObj.transform.position);
		}
		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.green;
			if (correctMatching) Gizmos.DrawLine(transform.position, correctMatching.transform.position);
		}
	}
}