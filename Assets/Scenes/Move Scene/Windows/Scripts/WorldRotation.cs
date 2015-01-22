using UnityEngine;
using System.Collections;

/*
 * Manage world orientation, and jump.
 * 
 * Windows only.
 */
public class WorldRotation : MonoBehaviour {

	[Tooltip("The force with which the player jumps")] [Range(1, 1000)]
	[SerializeField] private float jumpForce = 500;
	[Tooltip("The step below which a jump attempt is detected")] [Range(0.00001f, 0.001f)]
	[SerializeField] private float jumpDetection = 0.0001f;

	private GameObject sphere = null;
	private GameObject ground = null;
	private int roll = 0;

	private bool isGrounded = true;
	private int lastRoll = 0;
	private float epsilon = 0.1f;
	
	
	void Start () {
		RPCWrapper.RegisterMethod(UpdateRoll);
		RPCWrapper.RegisterMethod(JumpSphere);
		
		GetReferenceToPlayer ();
		GetReferenceToWorld ();
	}
	
	public void UpdateRoll (int newRoll)
	{
		roll = newRoll;
		Vector2 p = Vector2.zero;
		//ground.transform.eulerAngles = new Vector3(0, 0, -roll);
		RaycastHit2D hit = Physics2D.Raycast(sphere.transform.position, Vector3.down, Mathf.Infinity, 1 << LayerMask.NameToLayer("World"));
		if (hit)          
			p = hit.point;
		
		ground.transform.RotateAround(p, Vector3.forward, -(roll-lastRoll));
		lastRoll = roll;
		
	}
	
	public void JumpSphere (Vector3 dir){
		
		//Vector3 ju = new Vector3(0,10,0);
		isGrounded = CanJump ();
		Debug.Log ("IsGrounded : " + isGrounded);
		if (dir.y < - jumpDetection && isGrounded) {
			isGrounded=false;
			sphere.rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
		}		
	}
	
	public bool CanJump() {
		//if (isGrounded) {
		//sphere.rigidbody2D.velocity = new Vector3(0, sphere.rigidbody2D.velocity.y, 0);
		//}
		bool jump;
		
		RaycastHit2D hit = Physics2D.Raycast(sphere.transform.position, Vector3.down, sphere.transform.localScale.y / 2 + epsilon, 1 << LayerMask.NameToLayer("World"));
		
		if (hit) {
			jump = true;
		} else {
			jump = false;
		}
		
		//Debug.DrawRay(sphere.transform.position, Vector3.down, Color.red);
		return jump;
	}

	// Get a reference to the player. The player must have the tag "Player". Only works for one player.
	private void GetReferenceToPlayer () {
		string playerTag = "Player";
		sphere = GameObject.FindGameObjectWithTag (playerTag);
		if (sphere == null)
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
