using UnityEngine;
using System.Collections;

/*
 * Add this script to an object to have the camera follow it. 
 */
public class CameraFollowObject : MonoBehaviour {

	void Update () {
		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
	}
}
