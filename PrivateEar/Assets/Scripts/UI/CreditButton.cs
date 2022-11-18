using UnityEngine;

namespace PrivateEar {
	public class CreditButton : MonoBehaviour {
		[SerializeField] SlideIn creditPage;

		public void OnClick() {
			creditPage.gameObject.SetActive(true);
			creditPage.slideIn();
		}
	}
}