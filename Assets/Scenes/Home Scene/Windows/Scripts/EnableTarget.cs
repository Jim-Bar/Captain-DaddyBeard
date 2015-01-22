using UnityEngine;
using System.Collections;

/*
 * Enable or disable the scripts DisplayTarget and MoveTarget when the target is needed or not.
 * Also say MoveTarget to send back target position in the scene of shoot (for the zoom).
 * 
 * Windows only.
 */
public class EnableTarget : MonoBehaviour {

	private MoveTarget moveTargetScript = null;
	private DisplayTarget displayTargetScript = null;

	[Tooltip("Names of the scenes where the target is used")]
	[SerializeField] private string[] scenesWhereActive;
	
	[Tooltip("Name of the scene of shoot")]
	[SerializeField] private string shootScene;

	private void Start () {
		moveTargetScript = GetComponent<MoveTarget> ();
		if (moveTargetScript == null)
			Debug.LogError (GetType ().Name + " : No component Move Target found");

		displayTargetScript = GetComponent<DisplayTarget> ();
		if (displayTargetScript == null)
			Debug.LogError (GetType ().Name + " : No component Display Target found");

		DontDestroyOnLoad (gameObject);
	}

	private void OnLevelWasLoaded (int level) {
		// Enable the script if it is required in the current scene. Otherwise disable it.
		foreach (string scene in scenesWhereActive)
			if (scene.Equals (Application.loadedLevelName))
		{
			moveTargetScript.enabled = true;
			displayTargetScript.SetTargetActive (true);

			// Let Move Target sends the target position back to the Android device(s).
			if (shootScene.Equals (Application.loadedLevelName))
				moveTargetScript.SendPositionToAndroid = true;
			else
				moveTargetScript.SendPositionToAndroid = false;

			return;
		}

		moveTargetScript.enabled = false;
		displayTargetScript.SetTargetActive (false);
	}
}
