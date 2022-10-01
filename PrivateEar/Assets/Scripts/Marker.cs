using System;
using UnityEngine;
using FMODUnity;

namespace PrivateEar {
	/// <summary>
	/// Marker objects
	/// </summary>
	public class Marker : MonoBehaviour {
		[SerializeField] EventReference sound;

		public void OnClick() {
			try {
				RuntimeManager.PlayOneShot(sound);
			} catch (Exception e) {
				Debug.LogError(e);
			}
		}
	}
}