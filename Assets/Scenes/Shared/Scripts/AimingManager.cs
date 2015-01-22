using UnityEngine;
using System.Collections;

/*
 * Compute the location  of the target on the screen (in the range [-1, 1]).
 * Automatically enabled/disable itself when needed.
 * 
 * Android only.
 */
public class AimingManager : MonoBehaviour {

	[Tooltip("Names of the scenes where the target is used")]
	[SerializeField] private string[] scenesWhereActive;
	[Tooltip("Always display camera, not only when calibrating")]
	[SerializeField] private bool alwaysDisplayCamera = false; // If false, only display camera when calibrating.
	[Tooltip("Display debugging information")]
	[SerializeField] private bool debugMode = false; // If true, display calibration information on the screen.

	private WebCamTexture cameraStream = null; // Video stream from the device camera.
	private bool calibrating = true; // Are we calibrating right now ?

	// Rotation pointing to the upper left corner, lower right corner, and center of the screen respectively.
	private Quaternion upperLeftCalibration = Quaternion.identity; // World space.
	private Quaternion lowerRightCalibration = Quaternion.identity; // World space.
	private Quaternion localUpperLeftRotation = Quaternion.identity; // Screen center space.
	private Quaternion localLowerRightRotation = Quaternion.identity; // Screen center space.
	private Quaternion centerCalibration = Quaternion.identity; // World space.

	// Are the calibrations achieved ?
	private bool upperLeftCalibrationDone = false;
	private bool lowerRightCalibrationDone = false;

	// Coordinates of the point targetted by the tablet on the screen (from -1 to 1). The z coordinate is useless (but RPC only supports Vector3).
	private Vector3 point = Vector3.zero;
	private Vector3 lastPoint = Vector3.zero; // For the last frame.

	private void Start () {
		// Keep the manager alive throught scences.
		DontDestroyOnLoad(transform.gameObject);

		// Enable gyroscope.
		Input.gyro.enabled = true;

		// Initialize camera.
		cameraStream = new WebCamTexture ();
		cameraStream.Play ();
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

	private void OnLevelWasLoaded (int level) {
		// Enable the script if it is required in the current scene. Otherwise disable it.
		foreach (string scene in scenesWhereActive)
			if (scene.Equals (Application.loadedLevelName))
			{
				enabled = true;
				return;
			}

		enabled = false;
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
