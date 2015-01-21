using UnityEngine;
using System.Collections;

/*
 * Add this script to an object to have it follow the camera.
 */
public class ObjectFollowCamera : MonoBehaviour {
	
	void Update () {
		transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
	}
}
