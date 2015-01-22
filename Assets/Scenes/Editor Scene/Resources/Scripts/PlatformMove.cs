using UnityEngine;
using System.Collections;

/*
 * Move a platform forward and backwards.
 * 
 * /!\ DO NOT USE THIS SCRIPT WITH THE PLATFORM ROTATE SCRIPT /!\
 */
public class PlatformMove : MonoBehaviour {

	[Range(0, 10)]
	[SerializeField] private float speed = 1;
	[Tooltip("Will the platform move horizontally ?")]
	[SerializeField] private bool alongX = true;
	[Tooltip("The platform goes first to the right")]
	[SerializeField] private bool beginRight = true;
	[Tooltip("Will the platform move vertically ?")]
	[SerializeField] private bool alongY = false;
	[Tooltip("The platform goes first up")]
	[SerializeField] private bool beginUp = true;
	[Range(-1000, 0)]
	[SerializeField] private float minDelta = -5;
	[Range(0, 1000)]
	[SerializeField] private float maxDelta = 5;

	private Vector3 initialPosition;
	private bool goingRight = true;
	private bool goingUp = true;

	private void Start () {
		initialPosition = transform.localPosition;
		goingRight = beginRight;
		goingUp = beginUp;

		PlatformRotate platformRotate = GetComponent<PlatformRotate> ();
		if (platformRotate != null && platformRotate.enabled)
		{
			Debug.LogError (GetType ().Name + " : This script cannot be used with " + platformRotate.GetType ().Name);
			enabled = false;
		}
	}

	private void Update () {
		float newDeltaX = alongX ? speed * Time.deltaTime : 0;
		float newDeltaY = alongY ? speed * Time.deltaTime : 0;

		if (!goingRight)
			newDeltaX = - newDeltaX;
		if (!goingUp)
			newDeltaY = - newDeltaY;

		transform.Translate (newDeltaX, newDeltaY, 0);

		if (transform.localPosition.x - initialPosition.x >= maxDelta)
		{
			transform.Translate (maxDelta - (transform.localPosition.x - initialPosition.x), 0, 0);
			goingRight = false;
		}
		else if (transform.localPosition.x - initialPosition.x <= minDelta)
		{
			transform.Translate (minDelta - (transform.localPosition.x - initialPosition.x), 0, 0);
			goingRight = true;
		}
		if (transform.localPosition.y - initialPosition.y >= maxDelta)
		{
			transform.Translate (0, maxDelta - (transform.localPosition.y - initialPosition.y), 0);
			goingUp = false;
		}
		else if (transform.localPosition.y - initialPosition.y <= minDelta)
		{
			transform.Translate (0, minDelta - (transform.localPosition.y - initialPosition.y), 0);
			goingUp = true;
		}
	}
}
