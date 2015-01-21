using UnityEngine;
using System.Collections;

/*
 * Add this script to an object to have it follow the camera.
 */
public class ObjectFollowCamera : MonoBehaviour {

	[Tooltip("Will the object follows the camera on the X axis ?")]
	[SerializeField] private bool followX = true;
	[Tooltip("Will the object follows the camera on the Y axis ?")]
	[SerializeField] private bool followY = true;

	void Update () {
		float newX = followX ? Camera.main.transform.position.x : transform.position.x;
		float newY = followY ? Camera.main.transform.position.y : transform.position.y;
		transform.position = new Vector3 (newX, newY, transform.position.z);
	}
}
