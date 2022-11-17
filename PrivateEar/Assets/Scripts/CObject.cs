using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace PrivateEar {
	[RequireComponent(typeof(Collider2D))]
	public class CObject : MonoBehaviour {
		public static CObject HoveredObject = null;
		public static int BlockingInteract = 0;
		public bool InteractOverride = false;

		bool interactible => BlockingInteract == 0 || InteractOverride;

		[SerializeField, ReadOnly] Marker _matchedMarker;
		[TextArea] public string description;

		public SpriteRenderer Sprite { get; private set; }
		public CrimeMarker CrimeMarker { get; private set; }

		public bool hover { get; private set; }

		[Header("Sprite")]
		[SerializeField] string spriteOutlineFieldName = "_Outline";
		[SerializeField] float outlineThicknessOnHover;

		public UnityEvent<CObject> OnClicked;

		public Marker MatchedMarker {
			get => _matchedMarker;
			set { 
				_matchedMarker = value;
				CrimeMarker.gameObject.SetActive(false);
				// configure crime marker object
				CrimeMarker.gameObject.SetActive(_matchedMarker != null);
				CrimeMarker.AssignSprite(_matchedMarker.CustomSprite);
			}
		}

		private void Awake() {
			Sprite = GetComponentInChildren<SpriteRenderer>();
			CrimeMarker = GetComponentInChildren<CrimeMarker>();
			CrimeMarker.gameObject.SetActive(false);
			Sprite.material = new Material(Sprite.material);
			Sprite.material.SetFloat(spriteOutlineFieldName, 0f);
		}

		public void Start() {
			GameMaster.Instance?.RegisterObject(this);
		}

		public void OnHoverEnter() {
			if (interactible) {
				HoveredObject = this;
				Sprite.material.SetFloat(spriteOutlineFieldName, outlineThicknessOnHover);
				hover = true;
			}
		}

		public void OnHoverExit() {
			Sprite.material.SetFloat(spriteOutlineFieldName, 0f);
			if (HoveredObject == this) HoveredObject = null;
			hover = false;
		}

		private void OnMouseEnter() => OnHoverEnter();
		private void OnMouseOver() {
			if (interactible && !hover) OnHoverEnter();
			if (!interactible && hover) OnHoverExit();
		}
		private void OnMouseExit() => OnHoverExit();

		private void OnMouseDown() {
			if (interactible) {
				// when object is clicked
				OnClicked?.Invoke(this);
				OnHoverExit();
			}
		}
	}
}