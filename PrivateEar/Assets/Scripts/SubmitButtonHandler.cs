using UnityEngine;

namespace PrivateEar {
	public class SubmitButtonHandler : MonoBehaviour {
		public SubmitButton Button { get; private set; }
		bool _allMatchedLastFrame;
		bool _firstFrame;

		private void Awake() {
			Button = GetComponentInChildren<SubmitButton>();
		}

		private void OnEnable() { _firstFrame = true;  }

		private void Update() {
			if (Button) {
				bool allMatched = GameMaster.Instance.IsAllMatched;
				if (_firstFrame || _allMatchedLastFrame != allMatched) {
					if (allMatched && !Button.Active) Button.Appear();
					else if (!allMatched && Button.Active) Button.Disappear();
				}
				_firstFrame = false;
				_allMatchedLastFrame = allMatched;
			}
		}
	}
}