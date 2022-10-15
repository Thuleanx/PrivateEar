using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PrivateEar.Utils;

namespace PrivateEar {
	public class SceneChanger : MonoBehaviour {
		public float fadeSpeed = 1f;
		public GameObject blackScreen;
		public Canvas fadeCanvas;
		public SceneReference sceneReference;
		public bool isPause;

		static int FADEIN = 0, FADEOUT = 1, NEUTRAL = 2;
		int fadeState;

		public void Start() {
			if (isPause) { fadeState = NEUTRAL; }
			else
			{
				fadeState = FADEIN;
				fadeCanvas.GetComponent<CanvasGroup>().alpha = 1;
			}
			blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 1);
		}

		public void Update() {
			if (fadeState == FADEIN) FadeIn();
			if (fadeState == FADEOUT) FadeOut();
		}

		void FadeOut() {
			fadeCanvas.GetComponent<CanvasGroup>().alpha += fadeSpeed * Time.deltaTime;
			Debug.Log(fadeCanvas.GetComponent<CanvasGroup>().alpha);
			Debug.Log(Time.deltaTime);
			if (fadeCanvas.GetComponent<CanvasGroup>().alpha >= 1) {
				ChangeScene();
			}
		}

		void FadeIn() {
			fadeCanvas.GetComponent<CanvasGroup>().alpha -= fadeSpeed * Time.deltaTime;
			if (fadeCanvas.GetComponent<CanvasGroup>().alpha <= 0) fadeState = NEUTRAL;
		}

		public void TriggerFadeout() => fadeState = FADEOUT;

		void ChangeScene() {
			sceneReference.LoadScene();
		}
	}
}