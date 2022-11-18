using UnityEngine;
using UnityEngine.Events;
using System;
using DG.Tweening;

namespace PrivateEar {
	[RequireComponent(typeof(RectTransform))]
	public class SlideIn : MonoBehaviour {

		public RectTransform RectTransform {get; private set; }

		[SerializeField] Vector2 offset;
		[SerializeField, Range(0,10f)] float slideDuration;
		[SerializeField] Ease easing;
		[SerializeField] UnityEvent onSlideinFinish;

		Vector2 initialAnchor;

		private void Awake() {
			RectTransform = GetComponent<RectTransform>();
			initialAnchor = RectTransform.anchoredPosition;
		}

		public void slideIn(Action OnComplete) {
			Vector2 initialPos = offset + initialAnchor;
			DOTween.KillAll();
			RectTransform.anchoredPosition = initialPos;
			RectTransform.DOAnchorPos(initialAnchor, slideDuration).SetEase(easing).OnComplete(
				() => {
					OnComplete?.Invoke();
					onSlideinFinish?.Invoke();
				}
			);
		}

		public void slideIn() => slideIn(null);

		public void slideOut(Action OnComplete) {
			DOTween.KillAll();
			RectTransform.DOAnchorPos(offset + initialAnchor, slideDuration).SetEase(easing).OnComplete(
				() => OnComplete?.Invoke()
			);
		}

		public void slideOut() => slideOut(null);
	}
}