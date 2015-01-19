using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveTarget : MonoBehaviour {

	private void Start () {
		// Register UpdateTarget() in the RPC wrapper.
		RPCWrapper.RegisterMethod (UpdateTarget);

		DontDestroyOnLoad (gameObject);
	}

	public void UpdateTarget (Vector3 newPositionInScreen) {
		// Convert coordinates from [-1, 1] to [0, screenDimension].
		newPositionInScreen.x++;
		newPositionInScreen.y++;
		newPositionInScreen.x *= Screen.width / 2.0f;
		newPositionInScreen.y *= Screen.height / 2.0f;

		// Screen space to world space.
		Vector3 newPosition = Camera.main.ScreenToWorldPoint (newPositionInScreen);
		newPosition.z = transform.position.z; // Keep initial z position.
		transform.position = newPosition; // Apply new position.
	}
}
