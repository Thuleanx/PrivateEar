using UnityEngine;

namespace PrivateEar {
	public class MarkerLink : MonoBehaviour {
		public Marker Marker { get; private set;  }
		public LineRenderer LineRenderer { get; private set; }
		public CObject CrimeObject => Marker == null ? null : Marker.MatchedObject;

		[SerializeField] Vector2 offsetMarkerSS;
		[SerializeField] Vector2 offsetCObjectWS;

		private void Awake() {
			Marker = GetComponentInParent<Marker>();
			LineRenderer = GetComponent<LineRenderer>();
		}

		private void Update() {
			if (Marker && CrimeObject && (Marker.hover || CrimeObject.hover) && !InputManager.Instance.IsDragging) {
				// recalculate positionning
				if (!LineRenderer.enabled) {
					Vector2 offsetMarkerWS = Camera.main.ScreenToWorldPoint(offsetMarkerSS) - Camera.main.ScreenToWorldPoint(Vector2.zero);
					LineRenderer.SetPositions(new Vector3[]{offsetMarkerWS + (Vector2) Marker.transform.position, 
						offsetCObjectWS + (Vector2) CrimeObject.CrimeMarker.transform.position});
				}
				LineRenderer.enabled = true;
			} else {
				LineRenderer.enabled = false;
			}
		}
	}
}