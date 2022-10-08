using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.Events;

namespace PrivateEar {
	public class GameMaster : MonoBehaviour {
		public static GameMaster Instance;

		[ReadOnly] public List<Marker> allMarkers;

		private void Awake() {
			Instance = this;
		}

		public void RegisterMarker(Marker marker) { allMarkers.Add(marker);  }
		public void RegisterObject(CObject obj) { }

		public bool IsAllMatched {
			get {
				foreach (Marker marker in allMarkers) 
					if (!marker.IsMatched) return false; 
				return true;  
			}
		}
		public bool IsCorrectMatching {
			get {
				foreach (Marker marker in allMarkers)
					if (!marker.IsCorrectMatch)
						return false;
				return true;
			}
		}
	}
}