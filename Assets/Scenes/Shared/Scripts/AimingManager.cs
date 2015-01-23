using UnityEngine;
using System.Collections;

/*
 * Compute the location  of the target on the screen (in the range [-1, 1]).
 * The calibration is conserved accross scenes.
 * 
 * Android only.
 */
public class AimingManager : MonoBehaviour {
	
	[Tooltip("Always display camera, not only when calibrating")]
	[SerializeField] private bool alwaysDisplayCamera = false; // If false, only display camera when calibrating.
	[Tooltip("Display debugging information")]
	[SerializeField] private bool debugMode = false; // If true, display calibration information on the screen.
	[Tooltip("Should the calibration phase be enabled at the beginning ?")]
	[SerializeField] private bool beginWithCalibration = true;

	private WebCamTexture cameraStream = null; // Video stream from the device camera.
	private bool calibrating = true;

	/*
	 * Rotation pointing to the upper left corner, lower right corner, and center of the screen respectively.
	 * Kept static to avoid recalibration between scenes.
	 */
	private static Quaternion upperLeftCalibration = Quaternion.identity; // World space.
	private static Quaternion lowerRightCalibration = Quaternion.identity; // World space.
	private static Quaternion localUpperLeftRotation = Quaternion.identity; // Screen center space.
	private static Quaternion localLowerRightRotation = Quaternion.identity; // Screen center space.
	private static Quaternion centerCalibration = Quaternion.identity; // World space.

	// Are the calibrations achieved ?
	private bool upperLeftCalibrationDone = false;
	private bool lowerRightCalibrationDone = false;

	// Coordinates of the point targetted by the tablet on the screen (from -1 to 1). The z coordinate is useless (but RPC only supports Vector3).
	private Vector3 point = Vector3.zero;
	private Vector3 lastPoint = Vector3.zero; // For the last frame.

	private void Start () {
		// Enable gyroscope.
		Input.gyro.enabled = true;

		// Initialize camera.
		cameraStream = new WebCamTexture ();
		cameraStream.Play ();

		// Set if the calibration happens now or not.
		calibrating = beginWithCalibration;
	}
	
	private void Update () {
		if (!calibrating)
		{
			// Map to [-1, 1] * [-1, 1].
			Quaternion localRotation = Quaternion.Inverse(centerCalibration) * Input.gyro.attitude;
			point.x = 2 * (localRotation.y - localLowerRightRotation.y) / (localUpperLeftRotation.y - localLowerRightRotation.y) - 1;
			point.y = 2 * (localRotation.x - localUpperLeftRotation.x) / (localLowerRightRotation.x - localUpperLeftRotation.x) - 1;
			lowPassFilter();
			point.x = Mathf.Min(1, Mathf.Max(-1, point.x));
			point.y = Mathf.Min(1, Mathf.Max(-1, point.y));
			lastPoint.x = point.x;
			lastPoint.y = point.y;

			// Update point position on the server.
			if (upperLeftCalibrationDone && lowerRightCalibrationDone && Network.connections.Length > 0)
				RPCWrapper.RPC ("UpdateTarget", RPCMode.Server, point);
		}
	}

	private void OnGUI() {
		if (cameraStream.isPlaying) // Display camera.
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), cameraStream, ScaleMode.ScaleAndCrop);

		if (calibrating) // Calibration buttons.
		{
			if (!upperLeftCalibrationDone && GUILayout.Button ("Upper left calibration", GUILayout.Height(150)))
			{
				upperLeftCalibration = Input.gyro.attitude;
				upperLeftCalibrationDone = true;
				if (lowerRightCalibrationDone)
					ComputeCenterCalibration ();
			}
			if (!lowerRightCalibrationDone && GUILayout.Button ("Lower right calibration", GUILayout.Height(150)))
			{
				lowerRightCalibration = Input.gyro.attitude;
				lowerRightCalibrationDone = true;
				if (upperLeftCalibrationDone)
					ComputeCenterCalibration ();
			}
		}
		else // Recalibrate button.
			if (GUILayout.Button ("Recalibrate", GUILayout.Height(150)))
			{
				upperLeftCalibrationDone = false;
				lowerRightCalibrationDone = false;
				calibrating = true;
				RPCWrapper.RPC ("SetTargetActive", RPCMode.Server, !calibrating);
				if (!cameraStream.isPlaying) // If the camera is not already playing and we want to recalibrate.
				    cameraStream.Play ();
			}

		if (debugMode)
		{
			GUILayout.Label("Upper left : " + (upperLeftCalibrationDone ? localUpperLeftRotation.ToString() : "None"));
			GUILayout.Label("Lower right : " + (lowerRightCalibrationDone ? localLowerRightRotation.ToString() : "None"));
			GUILayout.Label("Calibration : " + (upperLeftCalibrationDone && lowerRightCalibrationDone ? (Quaternion.Inverse(centerCalibration) * Input.gyro.attitude).ToString() : "None"));
			if (upperLeftCalibrationDone && lowerRightCalibrationDone)
				GUILayout.Label("(x, y) = (" + point.x + ", " + point.y + ")");
		}
	}

	// Make the average between the two corner (compute center), and calculate position of the corners in the screen space. Stop the camera, not needed anymore.
	private void ComputeCenterCalibration() {
		centerCalibration = Quaternion.Slerp(upperLeftCalibration, lowerRightCalibration, 0.5f);
		localUpperLeftRotation = Quaternion.Inverse(upperLeftCalibration) * centerCalibration;
		localLowerRightRotation = Quaternion.Inverse(lowerRightCalibration) * centerCalibration;
		calibrating = false;
		RPCWrapper.RPC ("SetTargetActive", RPCMode.Server, !calibrating);
		if (!alwaysDisplayCamera)
			cameraStream.Stop ();
	}

	private void lowPassFilter() {
		float deltaTime = Time.deltaTime;
		float a = deltaTime / (0.5f + deltaTime);
		point = (1 - a) * point + a * lastPoint;
	}
}
