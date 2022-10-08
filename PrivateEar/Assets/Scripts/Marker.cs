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
		[SerializeField, Required] CObject correctMatching;
		[SerializeField] EventReference sound;

		[SerializeField, ReadOnly] CObject _matchedObj;
		public CObject MatchedObject {
			get => _matchedObj;
			set {
				if (_matchedObj) _matchedObj.MatchedMarker = null;
				if (value && value.MatchedMarker) value.MatchedMarker._matchedObj = null;
				if (value) value.MatchedMarker = this;
				_matchedObj = value;
			}
		}

		private void Start() {
			GameMaster.Instance?.RegisterMarker(this);
		}

		public void OnClick() {
			try {
				RuntimeManager.PlayOneShot(sound);
			} catch (Exception e) {
				Debug.LogError(e);
			}
		}

		public void OnBeginDrag(PointerEventData eventData) {
		}

		public void OnDrag(PointerEventData eventData) {
		}

		public void OnEndDrag(PointerEventData eventData) {
			if (CObject.HoveredObject) 	MatchedObject = CObject.HoveredObject;
			else 						MatchedObject = null;
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