using UnityEngine;

namespace Prototype {
	public class CrimeObject : MonoBehaviour {
		public static CrimeObject activeCrimeObject;
		[SerializeField] GameObject outline;
		public Marker linkedMarker;
		private void OnMouseEnter() {
			activeCrimeObject = this;
			outline?.SetActive(true);
		}

		private void OnMouseExit() {
			if (activeCrimeObject == this)
				activeCrimeObject = null;
			outline?.SetActive(false);
		}
	}
}