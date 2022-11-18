using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using FMODUnity;
using UnityEngine.EventSystems;
using NaughtyAttributes;

namespace PrivateEar {
	/// <summary>
	/// Marker objects
	/// </summary>
	public class Marker : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler  {
		public Canvas canvas { get; private set;  }

		[SerializeField, Required] CObject correctMatching;
		[SerializeField] EventReference sfxReference;
		[HideInInspector]public FMOD.Studio.EventInstance sfxInstance;
		[field:SerializeField] public Sprite CustomSprite {get; private set;}

		[SerializeField, ReadOnly] CObject _matchedObj;

		// juice objects
		[Required] public RectTransform markerObj;
		Button markerButton;
		public Vector2 mouseHoverOffsetSS;
		Vector2 markerObjPos;

		[SerializeField] public UnityEvent<Marker> OnClicked;
		[SerializeField] public UnityEvent<Marker> OnDragBegin;

		[Space]
		public UnityEvent OnCObjectAssigned;

		public CObject MatchedObject {
			get => _matchedObj;
			set {
				if (_matchedObj) _matchedObj.MatchedMarker = null;
				if (value && value.MatchedMarker) value.MatchedMarker._matchedObj = null;
				if (value) value.MatchedMarker = this;
				_matchedObj = value;
			}
		}
		public bool hover;
		public bool Interactible => markerButton.interactable;
		public bool dragging;

		private void Awake() { 
			canvas = GetComponentInParent<Canvas>();
			markerButton = markerObj.GetComponentInChildren<Button>();
			sfxInstance = FMODUnity.RuntimeManager.CreateInstance(sfxReference);
		}
		private void Start() {
			GameMaster.Instance?.RegisterMarker(this);
			markerObjPos = markerObj.anchoredPosition;
		}
		public void SetInteractable(bool interactible) {
			markerButton.interactable = interactible;
			if (!interactible) ForceEndDrag();
		}

		public void OnClick() {
			OnClicked?.Invoke(this);
		}

		public void OnBeginDrag(PointerEventData eventData) {
			if (Interactible) {
				Vector2 pointerPosWS = canvas.worldCamera.ScreenToWorldPoint(eventData.position / canvas.scaleFactor + mouseHoverOffsetSS);
				markerObj.position = pointerPosWS;
				InputManager.Instance.DraggingCnt++;
				dragging = true;
				OnDragBegin?.Invoke(this);
			}
		}

		public void OnDrag(PointerEventData eventData) {
			if (Interactible) {
				markerObj.anchoredPosition += eventData.delta / canvas.scaleFactor;
			}
		}

		public void OnEndDrag(PointerEventData eventData) {
			ForceEndDrag();
		}

		void ForceEndDrag() {
			if (dragging) {
				InputManager.Instance.DraggingCnt--;
				if (CObject.HoveredObject) {
					// then we have a matching
					MatchedObject = CObject.HoveredObject;
					markerObj.anchoredPosition = markerObjPos;
					OnCObjectAssigned?.Invoke();
				} else {
					// then we didn't drop it on any matchable element
					MatchedObject = null;
					markerObj.anchoredPosition = markerObjPos;
				}
				dragging = false;
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

		public void OnPointerExit(PointerEventData eventData) => hover = false;
		public void OnPointerEnter(PointerEventData eventData) => hover = true;
	}
}