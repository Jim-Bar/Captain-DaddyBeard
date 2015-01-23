using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * From relative coordinates ([-1, 1]), compute the position of the target.
 * Return the computed position to the Android devices for the shoot scene (needed for the zoom).
 * 
 * Windows only.
 */
public class MoveTarget : MonoBehaviour {

	[Tooltip("Target position on Android is needed in the scene of shoot")]
	[SerializeField] private bool sendPositionToAndroid = false;

	private void Start () {
		// Register UpdateTarget() in the RPC wrapper.
		RPCWrapper.RegisterMethod (UpdateTarget);
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

		if (sendPositionToAndroid)
			RPCWrapper.RPC ("UpdateCamera", RPCMode.Others, newPosition);
	}


}
