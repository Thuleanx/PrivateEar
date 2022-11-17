using UnityEngine;
using DG.Tweening;

namespace PrivateEar {
	public class CrimeMarker : MonoBehaviour {
		[SerializeField] Vector2 initialDropOffset;
		[SerializeField] Ease initialDropEase;
		[SerializeField, Range(0, 2f)] float initialDropDuration = 0.5f;

		public SpriteRenderer Sprite { get; private set;  }

		private void Awake() {
			Sprite = GetComponentInChildren<SpriteRenderer>();
		}

		private void OnEnable() {
			Sprite.transform.localPosition = initialDropOffset;
			Sprite.transform.DOLocalMove(new Vector2(0,0), initialDropDuration).SetEase(initialDropEase);
		}

		public void AssignSprite(Sprite sprite) {
			Sprite.sprite = sprite;
		}
	}
}