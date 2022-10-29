using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;

namespace PrivateEar {
	public class ZoomedPreview : MonoBehaviour {
		[SerializeField, Required] GameObject zoomedPreviewObj;
		[SerializeField, Required] Image crimeObjectImage;
		[SerializeField, Required] TMP_Text flavourText;
		[SerializeField] SlideIn slider;
		[SerializeField] EventTrigger trigger;

		bool exiting;

		private void Start() {
			foreach (CObject obj in FindObjectsOfType<CObject>())
				obj.OnClicked.AddListener(ActivateZoomedView);
			if (zoomedPreviewObj.activeInHierarchy) zoomedPreviewObj.SetActive(false);
		}

		private void ActivateZoomedView(CObject obj) {
			CObject.CanInteract = false;
			zoomedPreviewObj.SetActive(true);

			crimeObjectImage.sprite = obj.Sprite.sprite;
			flavourText.text = obj.description;
			slider.slideIn(() => trigger.enabled = true);
		}

		public void DeactivateZoomedView() {
			trigger.enabled = false;
			slider.slideOut(() => {
				CObject.CanInteract = true;
				zoomedPreviewObj.SetActive(false);
			});
		}
	}
}