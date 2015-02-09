using UnityEngine;
using System.Collections;

public class PlatformElevator : MonoBehaviour {

	[Range(0, 10)]
	[SerializeField] private float speed = 1;
	//[Tooltip("Will the platform move horizontally ?")]
	//[SerializeField] private bool alongX = true;
	//[Tooltip("The platform goes first to the right")]
	//[SerializeField] private bool beginRight = true;
	//[Tooltip("Will the platform move vertically ?")]
	//[SerializeField] private bool alongY = false;
	//[Tooltip("The platform goes first up")]
	//[SerializeField] private bool beginUp = true;
	//[Range(-1000, 0)]
	//[SerializeField] private float minDelta = -5;
	[Range(0, 1000)]
	[SerializeField] private float maxDelta = 5;
	[SerializeField] private float distancePlayer = 15;

	// Reference towards the player and the world object.
	private GameObject player = null;
	private GameObject world = null;

	private GameObject cr = null;

	private bool movingUp = false;
	private bool movingDown = false;
	
	private Vector3 initialPosition;
	private Vector3 worldInitialPosition;
	//private bool goingRight = true;
	//private bool goingUp = true;

	private bool comboPerformed = false;
	private bool comboLocked = false;

	private float coef = 0.9f;
	
	public void ValidateCombo (bool ok) {
		if (ok) {
			comboPerformed = true;
			Debug.Log ("combo done");

		}
	}

	// Use this for initialization
	void Start () {


		initialPosition = transform.localPosition;
		//goingRight = beginRight;
		//goingUp = beginUp;
		
		PlatformRotate platformRotate = GetComponent<PlatformRotate> ();
		if (platformRotate != null && platformRotate.enabled)
		{
			Debug.LogError (GetType ().Name + " : This script cannot be used with " + platformRotate.GetType ().Name);
			enabled = false;
		}

		cr = GameObject.Find("ComboCenter");
		GetReferenceToPlayer ();
		GetReferenceToWorld ();
		worldInitialPosition = world.transform.position;

	
	}
	
	// Update is called once per frame
	void Update () {


			if(comboPerformed==true) {
				if(movingDown) {
					if (transform.localPosition.y <= initialPosition.y + coef) {
						comboPerformed = false;
						comboLocked = false; 
					}
					float newDeltaY = speed * Time.deltaTime;
					transform.Translate (0, -newDeltaY, 0);
				Debug.Log ("moving down");
			}
				else if(movingUp) {
					if (transform.localPosition.y - (initialPosition.y) >= maxDelta - coef) {
						comboPerformed = false;
						comboLocked = false; 
					}
					Debug.Log ("moving up");
				float newDeltaY = speed * Time.deltaTime;
					transform.Translate (0, newDeltaY, 0);
				}
			} else {
				if(limit()) {
					Debug.Log("ok");
					if (detectPlayer() && !comboLocked) {
						performCombo();
					}
				}
			}
	}
	
	private void performCombo() {
		string arg;
		if (movingUp) {
			arg = "ArrowUp";
		}
		else {
			arg = "ArrowDown";
		}

		ComboRequest ct = cr.GetComponent<ComboRequest>();
		ct.AskCombo (this.gameObject, arg);
		comboLocked = true;
		Debug.Log ("combo ask");

	}

	private bool limit() {
		bool ok = false;
		//float wd = world.transform.position.y - worldInitialPosition.y;
		if (transform.localPosition.y - (initialPosition.y) >= maxDelta - coef) {
			ok = true;
			movingDown = true;
			movingUp = false;

		} else if(transform.localPosition.y <= initialPosition.y + coef) {
			ok = true;
			movingUp = true;
			movingDown = false;

		} else {
			Debug.Log("false");
		}
		return ok;
	}

	private bool detectPlayer() {

		//Debug.Log ("pos db : " + player.transform.position.x + " vs pos plat " + transform.localPosition.x);
		float dist = Vector3.Distance(transform.localPosition, player.transform.position);
		Debug.Log ("Dist :" + dist);
		if (dist <= distancePlayer) {
			return true;
		}
		else {
			return false;
		}
		/*if (player.transform.position.x >= transform.parent.parent.position.x + transform.localPosition.x - distancePlayer &&
		    player.transform.position.x <= transform.parent.parent.position.x + transform.localPosition.x + distancePlayer) {

			return true;
		} else {
			return false;
		}*/
	}

	// Get a reference to the player. The player must have the tag "Player". Only works for one player.
	private void GetReferenceToPlayer () {
		string playerTag = "Player";
		player = GameObject.FindGameObjectWithTag (playerTag);
		if (player == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"" + playerTag + "\".");
	}

	// Get a reference to the player. The player must have the tag "Player". Only works for one player.
	private void GetReferenceToWorld () {
		string WorldTag = "World";
		world = GameObject.FindGameObjectWithTag (WorldTag);
		if (world == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"" + WorldTag + "\".");
	}
}
