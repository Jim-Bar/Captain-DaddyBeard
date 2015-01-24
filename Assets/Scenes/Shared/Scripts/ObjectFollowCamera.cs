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
	
	[Tooltip("Follows with an offset on the X axis")] [Range(-10, 10)]
	[SerializeField] private float xOffset = 0;
	[Tooltip("Follows with an offset on the Y axis")] [Range(-10, 10)]
	[SerializeField] private float yOffset = 0;

	void Update () {
		float newX = followX ? Camera.main.transform.position.x + xOffset : transform.position.x;
		float newY = followY ? Camera.main.transform.position.y + yOffset : transform.position.y;
		transform.position = new Vector3 (newX, newY, transform.position.z);
	}
}
