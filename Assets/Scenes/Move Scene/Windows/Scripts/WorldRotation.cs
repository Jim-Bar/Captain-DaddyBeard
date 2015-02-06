using UnityEngine;
using System.Collections;

/*
 * Manage : 
 * - World orientation
 * - Jump
 * 
 * Windows only.
 */
public class WorldRotation : MonoBehaviour {

	[Tooltip("The force with which the player jumps")] [Range(500, 2000)]
	[SerializeField] private float jumpForce = 1200;
	[Tooltip("The step below which a jump attempt is detected")] [Range(0.001f, 0.01f)]
	[SerializeField] private float jumpDetection = 0.0025f;
	[Tooltip("A value of 1 is normal, 0.5 is half the normal sensibility, ...")] [Range(0.1f, 1)]
	[SerializeField] private float rollSensibility = 0.75f;
	[Tooltip("When the angle is in the dead zone, the angle will be set to zero")] [Range(0, 5)]
	[SerializeField] private int deadZone = 1;
	[Tooltip("Strength of the low pass filter. Lower means a higher strength")] [Range(0.01f, 0.1f)]
	[SerializeField] private float rollLowPassFilter = 0.05f;
	[Tooltip("Layer of the objects of the world (note that all world's objects must have this layer)")]
	[SerializeField] private LayerMask worldLayer;

	//rajout juste pour un test
	[SerializeField] private GameObject testOk = null;
	private GameObject test;

	// Reference towards the player and the world object.
	private GameObject player = null;
	private GameObject ground = null;

	// Roll and jump.
	private float roll = 0, lastRoll = 0;
	private bool isGrounded = true;
	
	void Start () {
		RPCWrapper.RegisterMethod(UpdateRoll);
		RPCWrapper.RegisterMethod(JumpPlayer);
		//RPCWrapper.RegisterMethod(ComboTest);
		
		GetReferenceToPlayer ();
		GetReferenceToWorld ();
	}

	//methode temporaire utilisé pour un test 
	public void ComboTest (bool ok) {
		if (ok) {

			Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y+2, 0 );
			test = Instantiate (testOk, pos, Quaternion.identity) as GameObject;

			Destroy(test, 5.0f);

		}


	}

	// Modify world rotation based on the device rotation.
	public void UpdateRoll (int deviceRoll) {
		// Reduce roll sensibility, and negate the roll (is it the opposite on the device).
		roll = - deviceRoll / (2.0f - rollSensibility);

		// Apply dead zone.
		if (Mathf.Abs (roll) < deadZone)
			roll = 0;

		// Find the point on the ground below the player.
		RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector3.down, Mathf.Infinity, worldLayer);
		if (hit.collider != null)
		{
			roll = LowPassFilter (roll, lastRoll);
			ground.transform.RotateAround(hit.point, Vector3.forward, roll - lastRoll);
			lastRoll = roll;
		}
	}

	// Have the character jump if the player gave an impulsion strong enough.
	public void JumpPlayer (Vector3 dir) {
		if (isGrounded && dir.y < - jumpDetection) {
			// Jump perpendicular to the direction of the ground.
			player.rigidbody2D.AddForce(Quaternion.Euler(0, 0, roll) * Vector2.up * jumpForce, ForceMode2D.Force);
			isGrounded = false; // The player jumps so he is not touching the ground anymore.

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
			//if (Network.connections.Length > 0) {
				//Debug.Log("Combo"); 
				//RPCWrapper.RPC ("ComboTask", RPCMode.Others, arg);
			//}
				
		}		
	}

	// Mark the player as touching the ground if he does touch an object with a layer 'worldLayer'.
	private void OnCollisionEnter2D (Collision2D collision) {
		if (1 << collision.collider.gameObject.layer == worldLayer.value)
			isGrounded = true;
	}

	// Mark the player as not touching the ground if he is touching nothing (fall from a platform, ...).
	private void OnCollisionExit2D (Collision2D collision) {
		isGrounded = false;
	}

	// Low pass filter.
	private float LowPassFilter (float value, float lastValue) {
		float deltaTime = Time.deltaTime;
		float a = deltaTime / (rollLowPassFilter + deltaTime);
		return (1 - a) * value + a * lastValue;
	}

	// Get a reference to the player. The player must have the tag "Player". Only works for one player.
	private void GetReferenceToPlayer () {
		string playerTag = "Player";
		player = GameObject.FindGameObjectWithTag (playerTag);
		if (player == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"" + playerTag + "\".");
	}
	
	// Get a reference to the world. The world must have the tag "World".
	private void GetReferenceToWorld () {
		string worldTag = "World";
		ground = GameObject.FindGameObjectWithTag (worldTag);
		if (ground == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"" + worldTag + "\".");
	}
}
