using UnityEngine;
using UnityEngine.Events;

namespace PrivateEar {
	public class SubmitButton : MonoBehaviour {
		public bool Active => isActiveAndEnabled;
		public UnityEvent OnWin;

		public FailMessage FailMessageGenerator;

		public void Appear() {
			gameObject.SetActive(true);
		}

		public void Disappear() {
			gameObject.SetActive(false);
		}

		public void OnClick() {
			if (GameMaster.Instance.IsCorrectMatching) {
				OnWin?.Invoke();
				Disappear();
			} else {
				FailMessageGenerator?.GenerateFailMessage();
				Disappear();
			}
		}
	}
}