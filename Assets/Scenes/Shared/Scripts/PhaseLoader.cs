using UnityEngine;
using System.Collections;

/*
 * Load a phase (shoot or deplacement), both Windows and Android.
 * /!\ ONLY ONE INSTANCE OF THIS CLASS SHOULD BE USED AT THE SAME TIME /!\
 * 
 * HOW TO USE THIS CLASS :
 * 1 - First of all, add this script to a game manager and fill the fields (Shoot Scene Name and Deplacement Scene Name)
 * 	   which are the names of the scenes used for shooting and deplacement.
 * 2 - A phase is identified by a type (shoot or deplacement), a level number and a phase number.
 *     For instance if I want to load the third phase (which is a deplacement phase) of the second level, I call :
 *     PhaseLoader.Load (PhaseLoader.Type.DEPLACEMENT, 2, 3);
 * Note : You can also split the loading in two parts : PhaseLoader.Prepare (...) then PhaseLoader.Load ().
 * 
 * How to create a phase and load it later :
 * 1 - In a scene (can be an empty one, like EditorScene...) create an object 'World XX YY' where XX is the level number and YY the phase number.
 * 2 - Add all the objects you want as children of 'World XX YY'.
 * 3 - Make a prefab of 'World XX YY' and put in the folder Scenes/Editor Scene/Resources/Phases.
 * 4 - You can now access it using the previously mentioned Load () method.
 */
public class PhaseLoader : MonoBehaviour {

	// Names of the scenes of shoot and deplacement.
	[Tooltip("Name of the scene of shoot")]
	[SerializeField] private string shootSceneName;
	[Tooltip("Name of the scene of deplacement")]
	[SerializeField] private string deplacementSceneName;
	
	private static PhaseLoader instance = null; // Reference towards the instance of this class.
	private static string phaseName = null; // Name of the next phase to load.
	private static Type phaseType = Type.SHOOT; // Type of the next phase to load.
	private static bool loadPhaseNextSceneLoading = false; // Will the phase loader load a phase on the next scene loading ?

	// Type of the phase to be loaded.
	public enum Type {
		SHOOT,
		DEPLACEMENT
	};

	private void Awake () {
		if (instance == null)
			instance = this;
		else
			Debug.LogError (GetType ().Name + " : Two instances of " + GetType ().Name + " running at the same time.");

		DontDestroyOnLoad (transform.gameObject);
	}

	/*
	 * Load a phase. Check if the phase exists (output an error message if not found).
	 * 'type' is the type of the phase loaded : PhaseLoader.Type.SHOOT or PhaseLoader.Type.DEPLACEMENT.
	 * 'level' is the number of the level.
	 * 'phase' is the number of the phase inside the level.
	 */
	public static void Load (Type type, int level, int phase) {
		Prepare (type, level, phase);
		Load ();
	}

	// Register phase data before loading it with Load ().
	public static void Prepare (Type type, int level, int phase) {
		CheckExist ();
		phaseName = "World " + level.ToString("D2") + " " + phase.ToString("D2");
		phaseType = type;
	}

	// Load a phase based on the data set by Prepare ().
	public static void Load () {
		CheckExist ();
		loadPhaseNextSceneLoading = true;

		if (phaseType == Type.SHOOT)
			Application.LoadLevel (instance.shootSceneName);
		else
			Application.LoadLevel (instance.deplacementSceneName);
	}

	// Log an error message if there is no instance of this class.
	private static void CheckExist () {
		if (instance == null)
			Debug.LogError ("Phase Loader : No instance found !");
	}

	// Load a phase if the phase loader has been told to do so ('loadPhaseNextSceneLoading' set to 'true').
	private void OnLevelWasLoaded (int level) {
		if (loadPhaseNextSceneLoading)
		{
			loadPhaseNextSceneLoading = false;
			Object world = Resources.Load ("Phases/" + phaseName);
			if (world != null)
			{
				Debug.Log (GetType ().Name + " : Successfully loaded phase \"" + phaseName + "\"");
				Instantiate (world);
			}
			else
				Debug.LogError (GetType ().Name + " : Phase \"" + phaseName + "\" not found.");
		}
	}
}
