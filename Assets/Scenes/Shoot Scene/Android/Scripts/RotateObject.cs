using UnityEngine;
using System.Collections;

// Rotate an object permanently.
public class RotateObject : MonoBehaviour {

	[SerializeField] private float speed = 100;

	void Update () {
		transform.Rotate (speed * Time.deltaTime * Vector3.forward);
	}
}
