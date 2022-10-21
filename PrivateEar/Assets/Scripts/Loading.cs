using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

using PrivateEar.Utils;

namespace PrivateEar {
	public class Loading : MonoBehaviour {
		public SceneReference Next;
		void Update() {
			try {
				if (FMODUnity.RuntimeManager.HaveMasterBanksLoaded) {
					Debug.Log("Master Bank Loaded");
					Next.LoadScene();
				} else {
					Debug.Log("Master Bank Not Yet Loaded " + FMODUnity.RuntimeManager.AnySampleDataLoading());
				}
			} catch (Exception err) {
				Debug.Log(err);
			}

		}
	}
}
