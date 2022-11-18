using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace PrivateEar {
	[RequireComponent(typeof(VideoPlayer))]
	public class VideoPlayerSupport : MonoBehaviour {
		public VideoPlayer Player {get; private set; }
		[SerializeField] UnityEvent onVideoEnd;

		void Awake() {
			Player = GetComponent<VideoPlayer>();
		}

		void Start() {
			Player.loopPointReached += OnVideoEnd;
		}

		void OnVideoEnd(VideoPlayer player) {
			onVideoEnd?.Invoke();
		}
	}
}