using UnityEngine;
using System.Collections;

/*
 * Manage world orientation, and jump.
 * 
 * Windows only.
 */
public class WorldRotation : MonoBehaviour {

	private GameObject sphere = null;
	private GameObject ground = null;
	private int roll = 0;
	
	private float speed = 700.0F;
	private bool isGrounded = true;
	private int lastRoll = 0;
	
	
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
		if (dir.y * speed < -4F && isGrounded) {
			//Debug.Log (" Before jump" + dir.y * speed);
			isGrounded=false;
			sphere.rigidbody2D.AddForce(new Vector2(0,500F), ForceMode2D.Force);
		}		
	}
	
	public bool CanJump() {
		//if (isGrounded) {
		//sphere.rigidbody2D.velocity = new Vector3(0, sphere.rigidbody2D.velocity.y, 0);
		//}
		bool jump;
		
		RaycastHit2D hit = Physics2D.Raycast(sphere.transform.position, Vector3.down, 1.0f, 1 << LayerMask.NameToLayer("World"));
		
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
