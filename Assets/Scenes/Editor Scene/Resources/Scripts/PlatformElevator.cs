using UnityEngine;
using System.Collections;

public class PlatformElevator : MonoBehaviour {

	[Range(0, 10)]
	[SerializeField] private float speed = 1;
	[Range(0, 1000)]
	[SerializeField] private float maxDelta = 5;
	[SerializeField] private float distancePlayer = 15;

	// Reference towards the player and the world object.
	private GameObject player = null;
	
	private GameObject cr = null;

	private bool movingUp = false;
	private bool movingDown = false;
	
	private Vector3 initialPosition;
	
	private bool comboPerformed = false;
	private bool comboLocked = false;

	private float coef = 0.9f;
	
	public void ValidateCombo (bool ok) {
		if (ok) {
			comboPerformed = true;
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
			}
				else if(movingUp) {
					if (transform.localPosition.y - (initialPosition.y) >= maxDelta - coef) {
						comboPerformed = false;
						comboLocked = false; 
					}
				float newDeltaY = speed * Time.deltaTime;
					transform.Translate (0, newDeltaY, 0);
				}
			} else {
				if(limit()) {
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

		}
		return ok;
	}

	private bool detectPlayer() {

		float dist = Vector3.Distance(transform.localPosition, player.transform.position);
		if (dist <= distancePlayer) {
			return true;
		}
		else {
			return false;
		}

	}

	// Get a reference to the player. The player must have the tag "Player". Only works for one player.
	private void GetReferenceToPlayer () {
		string playerTag = "Player";
		player = GameObject.FindGameObjectWithTag (playerTag);
		if (player == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"" + playerTag + "\".");
	}
	
}
