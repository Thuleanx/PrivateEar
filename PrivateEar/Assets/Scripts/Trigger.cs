using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {
	public UnityEvent onEnable;
	public UnityEvent onStart;

	void OnEnable() {
		onEnable?.Invoke();
	}

	void Start() {
		onStart?.Invoke();
	}
}