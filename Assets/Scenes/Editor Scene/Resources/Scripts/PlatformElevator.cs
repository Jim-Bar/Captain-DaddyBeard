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
	[SerializeField] private float distancePlayer = 4;

	// Reference towards the player and the world object.
	private GameObject player = null;

	private bool movingUp = false;
	private bool movingDown = false;
	
	private Vector3 initialPosition;
	//private bool goingRight = true;
	//private bool goingUp = true;

	private bool comboPerformed = false;

	//RPC function 
	public void ElevatorCombo (bool ok) {
		if (ok) {
			Debug.Log("Ok combo receive"); 
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
		RPCWrapper.RegisterMethod(ElevatorCombo);

		GetReferenceToPlayer ();
	
	}
	
	// Update is called once per frame
	void Update () {


			if(comboPerformed==true) {
				if(movingDown) {
					Debug.Log("moving Down");
					float newDeltaY = speed * Time.deltaTime;
					transform.Translate (0, -newDeltaY, 0);
				}
				else if(movingUp) {
					Debug.Log("moving up");
					float newDeltaY = speed * Time.deltaTime;
					transform.Translate (0, newDeltaY, 0);
				}
			} 
			if(limit()) {
				Debug.Log("ok");
				comboPerformed = false;
				if (detectPlayer()) {
					performCombo();
				}
				
			}
	
	}
	
	private void performCombo() {
		System.Random rnd = new System.Random();
		int nb = rnd.Next(1, 4);
		string arg;
		switch (nb)
		{
		case 1:
			arg = "ArrowUp";
			break;
		case 2:
			arg = "ArrowDown";
			break;
		case 3:
			arg = "ArrowLeft";
			break;
		case 4:
			arg = "ArrowRight";
			break;
		default:
			arg = "ArrowRight";
			break;
		}
		if (Network.connections.Length > 0) {
			Debug.Log("Combo"); 
			RPCWrapper.RPC ("ComboTask", RPCMode.Others, arg);
		}

	}

	private bool limit() {
		bool ok = false;
		if (transform.localPosition.y - initialPosition.y >= maxDelta) {
			ok = true;
			movingDown = true;
			movingUp = false;

		} else if(transform.localPosition.y <= initialPosition.y) {
			ok = true;
			movingUp = true;
			movingDown = false;

		}	
		return ok;
	}

	private bool detectPlayer() {

		if (player.transform.position.x >= transform.localPosition.x - distancePlayer &&
						player.transform.position.x <= transform.localPosition.x + distancePlayer) {

			return true;
		} else {
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
