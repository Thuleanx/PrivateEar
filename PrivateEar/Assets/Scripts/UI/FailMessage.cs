using UnityEngine;
using System.Collections;
using NaughtyAttributes;
using DG.Tweening;

namespace PrivateEar {
	[RequireComponent(typeof(CanvasGroup))]
	public class FailMessage : MonoBehaviour {
		public CanvasGroup CanvasGroup {get; private set; }
		public RectTransform RectTransform {get; private set; }

		[SerializeField, Range(0,3)] float fadeinDuration = 0.5f;
		[SerializeField, Range(0,4)] float lingerDuration = 2f;
		[SerializeField, Range(0,3)] float fadeoutDuration = 0.5f;
		[SerializeField] float floatingSpeed = 30;
		Vector3 originalPos;

		void Awake() {
			CanvasGroup = GetComponent<CanvasGroup>();
			RectTransform = GetComponent<RectTransform>();

			originalPos = RectTransform.anchoredPosition3D;
		}

		public void GenerateFailMessage() {
			gameObject.SetActive(true);
			StopAllCoroutines();
			RectTransform.anchoredPosition = originalPos;
			CanvasGroup.alpha = 0;

			Sequence sequence = DOTween.Sequence();
			float totalDuration = fadeinDuration + lingerDuration + fadeoutDuration;
			sequence.Append(CanvasGroup.DOFade(1, fadeinDuration));
			sequence.AppendInterval(lingerDuration);
			sequence.Append(CanvasGroup.DOFade(1, fadeoutDuration));
			sequence.Insert(0, RectTransform.DOAnchorPos(originalPos + Vector3.up * floatingSpeed * totalDuration, totalDuration));

			sequence.OnComplete(() => {
				gameObject.SetActive(false);
			});
		}
	}
}