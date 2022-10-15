using UnityEngine;
using NaughtyAttributes;

namespace PrivateEar {
	[RequireComponent(typeof(Collider2D))]
	public class CObject : MonoBehaviour {
		public static CObject HoveredObject = null;

		[SerializeField, ReadOnly] Marker _matchedMarker;
		public SpriteRenderer Sprite { get; private set; }

		[Header("Sprite")]
		[SerializeField] string spriteOutlineFieldName = "_Outline";
		[SerializeField] float outlineThicknessOnHover;

		public Marker MatchedMarker {
			get => _matchedMarker;
			set { _matchedMarker = value; }
		}

		private void Awake() {
			Sprite = GetComponentInChildren<SpriteRenderer>();
			Sprite.material = new Material(Sprite.material);
			Sprite.material.SetFloat(spriteOutlineFieldName, 0f);
		}

		public void Start() {
			GameMaster.Instance?.RegisterObject(this);
		}

		public void OnHoverEnter() {
			HoveredObject = this;
			Sprite.material.SetFloat(spriteOutlineFieldName, outlineThicknessOnHover);
		}

		public void OnHoverExit() {
			Sprite.material.SetFloat(spriteOutlineFieldName, 0f);
			if (HoveredObject == this) HoveredObject = null;
		}

		private void OnMouseEnter() => OnHoverEnter();
		private void OnMouseExit() => OnHoverExit();
	}
}