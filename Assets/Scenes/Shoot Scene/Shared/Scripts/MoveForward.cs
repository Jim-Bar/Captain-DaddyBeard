using UnityEngine;
using System.Collections;

/*
 * Move the attached object to the right at speed 'atSpeed'.
 */
public class MoveForward : MonoBehaviour {

	[SerializeField] private float atSpeed = 1;

	void Update () {
		transform.Translate (atSpeed * Time.deltaTime, 0, 0);
	}
}
