using UnityEngine;
using System.Collections;

public class GyroManager : MonoBehaviour {
	
	private int roll = 0;
	private bool fisrtCalibrationDone = false;
	private Quaternion calibration = Quaternion.identity;
	private const int marginToRecalibrate = 120; // Variation in degrees beyong which a calibration is made.

	void Start () {
		Input.gyro.enabled = true;
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
			RPCWrapper.RPC ("JumpPlayer", RPCMode.Others, dir);
	}
		
	private void Calibrate ()
	{
		calibration = Quaternion.Inverse (Input.gyro.attitude);
		if (calibration.w != 0 || calibration.x != 0 || calibration.y != 0 || calibration.z != 0)
			fisrtCalibrationDone = true;
	}
}
