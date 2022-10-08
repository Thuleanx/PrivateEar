using UnityEngine;

namespace PrivateEar {
	public class SubmitButton : MonoBehaviour {
		public bool Active => isActiveAndEnabled;

		public FailMessage FailMessageGenerator;

		public void Appear() {
			gameObject.SetActive(true);
		}

		public void Disappear() {
			gameObject.SetActive(false);
		}

		public void OnClick() {
			if (GameMaster.Instance.IsCorrectMatching) {
				Disappear();
				// transition
			} else {
				FailMessageGenerator?.GenerateFailMessage();
				Disappear();
			}
		}
	}
}