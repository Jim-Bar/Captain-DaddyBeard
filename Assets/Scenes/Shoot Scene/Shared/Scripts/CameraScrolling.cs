using UnityEngine;
using System.Collections;

public class CameraScrolling : MonoBehaviour {

	[SerializeField] private float scrollingSpeed = 1;
	
	// Update is called once per frame
	void Update () {
		transform.Translate (scrollingSpeed * Time.deltaTime, 0, 0);
	}
}
