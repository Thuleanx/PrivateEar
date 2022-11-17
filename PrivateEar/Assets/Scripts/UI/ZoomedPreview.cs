using UnityEngine;
using UnityEngine.Events;
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
		[Space]
		public UnityEvent OnObjectClicked;
		public UnityEvent OnDeactivate;
		public bool Active;
	
		CObject focusObj;
		bool exiting;
		int index = 0;

		private void Start() {
			foreach (CObject obj in FindObjectsOfType<CObject>())
				obj.OnClicked.AddListener(ActivateZoomedView);
			if (zoomedPreviewObj.activeInHierarchy) zoomedPreviewObj.SetActive(false);
		}

		public void OnObjectClick() {
			index = (index+1) % focusObj.CloseupSprites.Count;
			crimeObjectImage.sprite = focusObj.CloseupSprites[index];
			OnObjectClicked?.Invoke();
		}

		private void ActivateZoomedView(CObject obj) {
			Active = true;
			CObject.BlockingInteract++;
			zoomedPreviewObj.SetActive(true);
			focusObj = obj;

			index = 0;
			crimeObjectImage.sprite = obj.CloseupSprites[index];

			flavourText.text = obj.description;
			slider.slideIn(() => trigger.enabled = true);
		}

		public void DeactivateZoomedView() {
			Active = false;
			trigger.enabled = false;
			OnDeactivate?.Invoke();
			slider.slideOut(() => {
				// should only be called once per deactivation
				CObject.BlockingInteract--;
				zoomedPreviewObj.SetActive(false);
			});
		}
	}
}