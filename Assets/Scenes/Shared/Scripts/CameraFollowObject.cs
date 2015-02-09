using UnityEngine;
using System.Collections;

/*
 * Add this script to an object to have the camera follow it. 
 */
public class CameraFollowObject : MonoBehaviour {

	[Tooltip("Will the camera follows the object on the X axis ?")]
	[SerializeField] private bool followX = true;
	[Tooltip("Will the camera follows the object on the Y axis ?")]
	[SerializeField] private bool followY = true;

	[Tooltip("Follows with an offset on the X axis")] [Range(-10, 10)]
	[SerializeField] private float xOffset = 0;
	[Tooltip("Follows with an offset on the Y axis")] [Range(-10, 10)]
	[SerializeField] private float yOffset = 0;

	[Tooltip("Adjust with game resolution on the X axis")]
	[SerializeField] private bool adjustWithScreenX = false;
	[Tooltip("Adjust with game resolution on the Y axis")]
	[SerializeField] private bool adjustWithScreenY = false;

	private float xRealOffset = 0;
	private float yRealOffset = 0;

	void Update () {
		xRealOffset = adjustWithScreenX ? Screen.width * xOffset / 1920 : xOffset;
		yRealOffset = adjustWithScreenY ? Screen.height * yOffset / 1080 : yOffset;

		float newX = followX ? transform.position.x + xRealOffset : Camera.main.transform.position.x;
		float newY = followY ? transform.position.y + yRealOffset : Camera.main.transform.position.y;
		Camera.main.transform.position = new Vector3(newX, newY, Camera.main.transform.position.z);
	}
}
