using UnityEngine;
using UnityEngine.Events;

namespace PrivateEar {
	public class SubmitButton : MonoBehaviour {
		public bool Active => isActiveAndEnabled;
		public UnityEvent OnWin;

		public FailMessage FailMessageGenerator;
		[HideInInspector]
		public UnityEvent OnClicked;

		public void Appear() {
			gameObject.SetActive(true);
		}

		public void Disappear() {
			gameObject.SetActive(false);
		}

		public void OnClick() {
			OnClicked?.Invoke();
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