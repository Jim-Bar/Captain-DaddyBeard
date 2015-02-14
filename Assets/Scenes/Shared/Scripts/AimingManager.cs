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
	[Tooltip("Show the calibration button only when paused")]
	[SerializeField] private bool showOnlyWhenPaused = false; // If true, only show the calibration button when time scale equals 0.
	[Tooltip("Picture of the calibration nutton")]
	[SerializeField] private Texture2D buttonCalibration = null;
	[Tooltip("Area where to display information about calibration")]
	[SerializeField] private Texture2D infoPanel = null; // The lower panel where to display "Aim at the upper left, ...".
	[Tooltip("Shoot button picture")]
	[SerializeField] private Texture2D buttonNormal = null;
	[Tooltip("Shoot button picture when pressed")]
	[SerializeField] private Texture2D buttonPressed = null;
	[Tooltip("Picture of the target")]
	[SerializeField] private Texture2D target = null;
	[Tooltip("Font to use for the explanations about calibration")]
	[SerializeField] private Font infoFont = null;

	private WebCamTexture cameraStream = null; // Video stream from the device camera.
	private bool calibrating = true;
	private GUIStyle infoStyle;
	private GUIStyle buttonStyle;
	private int buttonSize = 0;

	/*
	 * Rotation pointing to the upper left corner, lower right corner, and center of the screen respectively.
	 * Kept static to avoid recalibration between scenes.
	 */
	private static Quaternion upperLeftCalibration = Quaternion.identity; // World space.
	private static Quaternion lowerRightCalibration = Quaternion.identity; // World space.
	private static Quaternion localUpperLeftRotation = Quaternion.identity; // Screen center space.
	private static Quaternion localLowerRightRotation = Quaternion.identity; // Screen center space.
	private static Quaternion centerCalibration = Quaternion.identity; // World space.

	// Are the calibrations achieved ? Kept static to avoid recalibration between scenes.
	private static bool upperLeftCalibrationDone = false;
	private static bool lowerRightCalibrationDone = false;

	// Coordinates of the point targetted by the tablet on the screen (from -1 to 1). The z coordinate is useless (but RPC only supports Vector3).
	private Vector3 point = Vector3.zero;
	private Vector3 lastPoint = Vector3.zero; // For the last frame.

	private void Start () {
		// Set if the calibration happens now or not.
		calibrating = !upperLeftCalibrationDone || !lowerRightCalibrationDone;
		RPCWrapper.RPC ("SetVisible", RPCMode.Server, !calibrating);

		// Enable gyroscope.
		Input.gyro.enabled = true;

		// Initialize camera.
		cameraStream = new WebCamTexture ();
		if (calibrating)
			cameraStream.Play ();

		// Check the fields.
		if (buttonCalibration == null)
			Debug.LogError (GetType ().Name + " : A texture is missing for the calibration button (field empty)");
		if (infoPanel == null)
			Debug.LogError (GetType ().Name + " : A texture is missing for the info panel (field empty)");
		if (buttonNormal == null)
			Debug.LogError (GetType ().Name + " : A texture is missing for the fire button (field empty)");
		if (buttonPressed == null)
			Debug.LogError (GetType ().Name + " : A texture is missing for the fire button (field empty)");
		if (target == null)
			Debug.LogError (GetType ().Name + " : A texture is missing for the target (field empty)");
		if (infoFont == null)
			Debug.LogError (GetType ().Name + " : A font is missing for the info panel (field empty)");

		// Initialize styles and contents.
		infoStyle = new GUIStyle ();
		infoStyle.font = infoFont;
		infoStyle.fontSize = Screen.width / 30;
		infoStyle.normal.textColor = Color.black;
		infoStyle.alignment = TextAnchor.LowerCenter;
		buttonStyle = new GUIStyle (infoStyle);
		buttonStyle.alignment = TextAnchor.MiddleCenter;
		buttonStyle.normal.background = buttonNormal;
		buttonStyle.active.background = buttonPressed;
		buttonSize = Screen.width / 6;
	}
	
	private void Update () {
		if (!calibrating)
		{
			// Map to [-1, 1] * [-1, 1].
			Quaternion localRotation = Quaternion.Inverse(centerCalibration) * Input.gyro.attitude;
			point.x = 2 * (localRotation.y - localLowerRightRotation.y) / (localUpperLeftRotation.y - localLowerRightRotation.y) - 1;
			point.y = 2 * (localRotation.x - localUpperLeftRotation.x) / (localLowerRightRotation.x - localUpperLeftRotation.x) - 1;
			LowPassFilter();
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
			// Draw the panel.
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), infoPanel, ScaleMode.StretchToFill);
			string infoText = "Visez le coin " + (!upperLeftCalibrationDone ? "supérieur gauche" : "inférieur droit") +  " de l'écran et tirez";
			GUI.Label (new Rect (0, 0, Screen.width, Screen.height), infoText, infoStyle);

			// Draw the target.
			GUI.DrawTexture(new Rect(Screen.width / 2 - buttonSize / 2, Screen.height / 2 - buttonSize / 2, buttonSize, buttonSize), target, ScaleMode.StretchToFill);

			// Draw the button and calibrate.
			if (GUI.Button (new Rect (Screen.width - buttonSize - 50, Screen.height / 2 - buttonSize / 2, buttonSize, buttonSize), "Feu !", buttonStyle))
			{
				if (!upperLeftCalibrationDone)
				{
					upperLeftCalibration = Input.gyro.attitude;
					upperLeftCalibrationDone = true;
				}
				else if (!lowerRightCalibrationDone)
				{
					lowerRightCalibration = Input.gyro.attitude;
					lowerRightCalibrationDone = true;
					ComputeCenterCalibration ();
				}
				else
					Debug.LogError (GetType ().Name + " : Unexpected case in calibration encountered");
			}
		}
		else if (!showOnlyWhenPaused || (showOnlyWhenPaused && Time.timeScale == 0)) // Recalibrate button (in game, only when pause, i.e when time scale equals 0).
			if (GUI.Button (new Rect (0, 0, 200 * Screen.height / 1104, 200 * Screen.height / 1104), buttonCalibration, GUIStyle.none)) // Adapt button size to screen.
			{
				upperLeftCalibrationDone = false;
				lowerRightCalibrationDone = false;
				calibrating = true;
				RPCWrapper.RPC ("SetVisible", RPCMode.Server, !calibrating);
				if (!cameraStream.isPlaying) // If the camera is not already playing and we want to recalibrate.
				    cameraStream.Play ();

				// Plan a recalibration for the movement scene too.
				GyroManager.Recalibrate ();
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
	private void ComputeCenterCalibration () {
		centerCalibration = Quaternion.Slerp(upperLeftCalibration, lowerRightCalibration, 0.5f);
		localUpperLeftRotation = Quaternion.Inverse(upperLeftCalibration) * centerCalibration;
		localLowerRightRotation = Quaternion.Inverse(lowerRightCalibration) * centerCalibration;
		calibrating = false;
		RPCWrapper.RPC ("SetVisible", RPCMode.Server, !calibrating);
		if (!alwaysDisplayCamera)
			cameraStream.Stop ();
	}

	private void LowPassFilter () {
		float deltaTime = Time.deltaTime;
		float a = deltaTime / (0.5f + deltaTime);
		point = (1 - a) * point + a * lastPoint;
	}
}
