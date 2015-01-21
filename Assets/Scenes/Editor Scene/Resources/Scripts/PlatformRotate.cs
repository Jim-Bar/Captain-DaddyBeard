using UnityEngine;
using System.Collections;

/*
 * Rotate a platform.
 * 
 * /!\ DO NOT USE THIS SCRIPT WITH THE PLATFORM MOVE SCRIPT /!\
 */
public class PlatformRotate : MonoBehaviour {

	[Range(0, 1000)]
	[SerializeField] private float speed = 5;
	[SerializeField] private bool reverse = false;

	private void Start () {
		PlatformMove platformMove = GetComponent<PlatformMove> ();
		if (platformMove != null && platformMove.enabled)
		{
			Debug.LogError (GetType ().Name + " : This script cannot be used with " + platformMove.GetType ().Name);
			enabled = false;
		}
	}

	private void Update () {
		if (!reverse)
			transform.Rotate (speed * Time.deltaTime * Vector3.back);
		else
			transform.Rotate (- speed * Time.deltaTime * Vector3.back);
	}
}
