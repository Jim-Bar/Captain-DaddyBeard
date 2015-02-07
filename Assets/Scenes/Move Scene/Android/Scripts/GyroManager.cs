using UnityEngine;
using System.Collections;

/*
 * Android only.
 * 
 * Calibrate only when the device is facing up.
 */
public class GyroManager : MonoBehaviour {

	[SerializeField] private bool autoRecalibrationEnabled = false;

	private static bool firstCalibrationDone = false;
	private static Quaternion calibration = Quaternion.identity;
	private int roll = 0;
	private const int marginToRecalibrate = 120; // Variation in degrees beyong which a calibration is made.

	void Start () {
		Input.gyro.enabled = true;
	}

	void Update () {

		// Calibrate if not done yet.
		if (!firstCalibrationDone)
			Calibrate ();

		if (firstCalibrationDone)
		{
			Vector3 eulerAngles = (calibration * Input.gyro.attitude).eulerAngles;

			// Get "roll" (strictly speaking, it is not exactly the roll).
			roll = (int) (eulerAngles.y > 180 ? eulerAngles.y - 360 : eulerAngles.y); // Maps roll to [-180, 180].

			// Recalibrate if the device orientation changed too much.
			if (autoRecalibrationEnabled &&
			    (Mathf.Abs(eulerAngles.x > 180 ? eulerAngles.x - 360 : eulerAngles.x) >= 2 * marginToRecalibrate
			 	|| Mathf.Abs(eulerAngles.z > 180 ? eulerAngles.z - 360 : eulerAngles.z) >= marginToRecalibrate))
				Calibrate ();
		}

		if (Time.timeScale == 1) {
						// Update the roll on the server side.
						if (Network.connections.Length > 0)
								RPCWrapper.RPC ("UpdateRoll", RPCMode.Others, roll);

						// Get input by accelerometer.
						Vector3 dir = Vector3.zero;
						dir.y = -Input.acceleration.y;
						if (dir.sqrMagnitude > 1)
								dir.Normalize ();
		
						dir *= Time.deltaTime;
						if (Network.connections.Length > 0)
								RPCWrapper.RPC ("JumpPlayer", RPCMode.Others, dir);
				}
	}

	// Calibrate only when the device is held parallel to the ground with the screen facing upwards.
	private void Calibrate ()
	{
		if (Input.deviceOrientation == DeviceOrientation.FaceUp)
		{
			calibration = Quaternion.Inverse (Input.gyro.attitude);
			if (calibration.w != 0 || calibration.x != 0 || calibration.y != 0 || calibration.z != 0)
				firstCalibrationDone = true;
		}
	}
}
