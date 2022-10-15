using UnityEngine;
using NaughtyAttributes;

namespace PrivateEar {
	[RequireComponent(typeof(Collider2D))]
	public class CObject : MonoBehaviour {
		public static CObject HoveredObject = null;

		[SerializeField, ReadOnly] Marker _matchedMarker;

		public Marker MatchedMarker {
			get => _matchedMarker;
			set { _matchedMarker = value; }
		}

		public void Start() {
			GameMaster.Instance?.RegisterObject(this);
		}

		public void OnHoverEnter() {
			HoveredObject = this;
		}

		public void OnHoverExit() {
			if (HoveredObject == this) HoveredObject = null;
		}

		private void OnMouseEnter() => OnHoverEnter();
		private void OnMouseExit() => OnHoverExit();
	}
}