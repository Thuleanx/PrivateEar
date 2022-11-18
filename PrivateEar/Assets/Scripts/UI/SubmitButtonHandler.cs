using UnityEngine;

namespace PrivateEar {
	public class SubmitButtonHandler : MonoBehaviour {
		public SubmitButton Button { get; private set; }
		bool _allMatchedLastFrame;
		bool _firstFrame;
		bool _submitAppear;

		private void Awake() {
			Button = GetComponentInChildren<SubmitButton>();
		}

		private void OnEnable() { 
			_firstFrame = true;  
			_submitAppear = false;
		}

		void Start() {
			foreach (var marker in FindObjectsOfType<Marker>())
				marker.OnCObjectAssigned.AddListener(()=>{
					if (GameMaster.Instance.IsAllMatched)
						_submitAppear = true;
				});
		}

		private void Update() {
			if (Button) {
				bool allMatched = GameMaster.Instance.IsAllMatched;
				if (_firstFrame || _allMatchedLastFrame != allMatched || _submitAppear) {
					if (allMatched && !Button.Active) Button.Appear();
					else if (!allMatched && Button.Active) Button.Disappear();
					_submitAppear = false;
				}
				_firstFrame = false;
				_allMatchedLastFrame = allMatched;
			}
		}
	}
}