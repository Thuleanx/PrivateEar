using UnityEngine;
using UnityEngine.EventSystems;		
using NaughtyAttributes;

namespace Prototype {
	public class Marker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
		public static Marker activeMarker;
		[Required] public CrimeObject CorrectLink;
		[Required] public LineRenderer linkLine;
		[Required] public Transform anchor;
		[SerializeField] ParticleSystem sys;
		CrimeObject _linkedObject;
		public CrimeObject linkedObject {
			get => _linkedObject;
			set {
				if (_linkedObject && _linkedObject.linkedMarker) _linkedObject.linkedMarker = null;
				linkLine.gameObject.SetActive(value != null);
				if (value) {
					value.linkedMarker?.Unlink();
					value.linkedMarker = this;
					linkLine.SetPositions(new Vector3[]{ (Vector2) anchor.position, (Vector2) value.transform.position});
				}
				if (Correct && !sys.isPlaying) sys.Play();
				if (!Correct && sys.isPlaying) sys.Stop();
				_linkedObject = value;
			}
		}
		public bool hover { get; private set; }

		private void Update() {
		}

		public bool Correct => CorrectLink == _linkedObject;

		public void OnBeginDrag(PointerEventData eventData) {
			activeMarker = this;
			linkLine.gameObject.SetActive(true);
		}

		public void OnDrag(PointerEventData eventData) {
			linkLine.SetPositions(new Vector3[]{ (Vector2) anchor.position, (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition)});
		}

		public void OnEndDrag(PointerEventData eventData) {
			if (CrimeObject.activeCrimeObject) Link(CrimeObject.activeCrimeObject);
			else Link(null);
		}

		public void Link(CrimeObject obj) => linkedObject = obj;
		public void Unlink() => linkedObject = null;

		public void OnPointerExit(PointerEventData eventData) => hover = true;
		public void OnPointerEnter(PointerEventData eventData) => hover = false;
	}
}