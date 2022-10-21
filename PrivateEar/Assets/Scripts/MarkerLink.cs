using UnityEngine;

namespace PrivateEar {
	public class MarkerLink : MonoBehaviour {
		public Marker Marker { get; private set;  }

		private void Awake() {
			Marker = GetComponentInParent<Marker>();
		}

		private void Update() {
		}

	}
}