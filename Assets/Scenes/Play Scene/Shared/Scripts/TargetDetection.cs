using UnityEngine;
using System.Collections;

public class TargetDetection : MonoBehaviour {

	#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

	// The target is on the object.
	private bool targetted = false;

	private void OnTriggerEnter2D (Collider2D other) {
		targetted = true;
	}

	private void OnTriggerExit2D (Collider2D other) {
		targetted = false;
	}

	// Return true if the object will be destroyed.
	public bool ShootButtonPressed () {
		if (targetted)
			Network.Destroy (gameObject);

		return targetted;
	}

	#endif
}
