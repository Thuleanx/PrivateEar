using UnityEngine;
using NaughtyAttributes;

namespace PrivateEar {
	public class InputManager : MonoBehaviour {
		public static InputManager Instance = null;

		// if an object begins dragging, add 1 to this counter. At the end, subtract 1 from this counter
		[ReadOnly] public int DraggingCnt = 0;
		public bool IsDragging => DraggingCnt > 0;

		private void Awake() {
			Instance = this;
		}
	}
}