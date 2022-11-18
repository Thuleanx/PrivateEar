using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using PrivateEar.Utils;

namespace PrivateEar {
	public class Vibrate : MonoBehaviour {
		[SerializeField, MinMaxSlider(0, 1)] Vector2 delay;
		[SerializeField] float duration = 0.5f;
		[SerializeField] Ease scaleEase;

		Vector3 originalScale;
		Vector3 originalPosition;
		Sequence sequence;

		void Awake() {
			originalPosition = transform.position;
			originalScale = transform.localScale;
		}

		void Start() {
			transform.localScale = Vector3.zero;
		}

		public void StartVibrate() {
			sequence?.Kill();
			sequence = DOTween.Sequence();
			
			float vdelay = Random.Range(delay.x, delay.y);

			transform.localScale = Vector3.zero;
			sequence.AppendInterval(vdelay);
			sequence.Append(transform.DOScale(originalScale, duration).SetEase(scaleEase));
			sequence.Insert(vdelay, transform.DOShakeRotation(duration));
		}
	}
}