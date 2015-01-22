using UnityEngine;
using System.Collections;

public class GyroManager : MonoBehaviour {

	private GameObject sphere = null;
	private GameObject ground = null;
	private int roll = 0;

	#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

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
			sphere.rigidbody2D.AddForce(new Vector2(0,100F), ForceMode2D.Force);
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

	#elif UNITY_ANDROID

	private bool fisrtCalibrationDone = false;
	private Quaternion calibration = Quaternion.identity;
	private const int marginToRecalibrate = 20; // Variation in degrees beyong which a calibration is made.
	
	private bool jump = false;

	void Start () {
		Input.gyro.enabled = true;

		GetReferenceToPlayer ();
		GetReferenceToWorld ();
	}

	void Update () {

		Vector3 eulerAngles = (calibration * Input.gyro.attitude).eulerAngles;

		// Get "roll" (strictly speaking, it is not exactly the roll).
		roll = (int) (eulerAngles.y > 180 ? eulerAngles.y - 360 : eulerAngles.y); // Maps roll to [-180, 180].

		// Recalibrate if the device orientation changed too much.
		if (!fisrtCalibrationDone
		    || Mathf.Abs(eulerAngles.x > 180 ? eulerAngles.x - 360 : eulerAngles.x) >= 2 * marginToRecalibrate
		    || Mathf.Abs(eulerAngles.z > 180 ? eulerAngles.z - 360 : eulerAngles.z) >= marginToRecalibrate)
			Calibrate ();

		// Update the roll on the server side.
		if (Network.connections.Length > 0)
			RPCWrapper.RPC ("UpdateRoll", RPCMode.Others, roll);

		//get input by accelerometer

	
	
		Vector3 dir = Vector3.zero;
		dir.y = -Input.acceleration.y;
		//Debug.Log (" Add input acc" + dir.y);
		if (dir.sqrMagnitude > 1)
			dir.Normalize();
		
		dir *= Time.deltaTime;
		//Debug.Log (" before rpc" + dir.y);
		if (Network.connections.Length > 0)
			RPCWrapper.RPC ("JumpSphere", RPCMode.Others, dir);
	}
		
	private void Calibrate ()
	{
		calibration = Quaternion.Inverse (Input.gyro.attitude);
		if (calibration.w != 0 || calibration.x != 0 || calibration.y != 0 || calibration.z != 0)
			fisrtCalibrationDone = true;
	}
			
	#endif

	private void GetReferenceToPlayer () {
		sphere = GameObject.FindGameObjectWithTag ("Player");
		if (sphere == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"Player\"");
	}

	private void GetReferenceToWorld () {
		ground = GameObject.FindGameObjectWithTag ("World");
		if (ground == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"World\"");
	}
}
