using UnityEngine;
using System.Collections;

/*
 * Load a phase (shoot or deplacement), both Windows and Android.
 * /!\ ONLY ONE INSTANCE OF THIS CLASS SHOULD BE USED AT THE SAME TIME /!\
 * 
 * HOW TO USE THIS CLASS :
 * 1 - First of all, add this script to a game manager and fill the fields (Shoot Scene Name and Deplacement Scene Name)
 * 	   which are the names of the scenes used for shooting and deplacement (do not put 'Android - ' or 'Windows - ' prefix).
 * 2 - A phase is identified by a type (shoot or deplacement), a level number and a phase number.
 *     For instance if I want to load the third phase (which is a deplacement phase) of the second level, I call :
 *     PhaseLoader.Load (PhaseLoader.Type.DEPLACEMENT, 2, 3);
 * 
 * How to create a phase and load it later :
 * 1 - In a scene (can be an empty one, like EditorScene...) create an object 'World XX YY' where XX is the level number and YY the phase number.
 * 2 - Add all the objects you want as children of 'World XX YY'.
 * 3 - Make a prefab of 'World XX YY' and put in the folder Scenes/Editor Scene/Resources/Phases.
 * 4 - You can now access it using the previously mentioned Load () method.
 */
public class PhaseLoader : MonoBehaviour {

	// Names of the scenes of shoot and deplacement (without the Android or Windows prefix).
	[Tooltip("Name of the scene of shoot (without the prefix 'Android - ' or 'Windows - ')")]
	[SerializeField] private string shootSceneName;
	[Tooltip("Name of the scene of deplacement (without the prefix 'Android - ' or 'Windows - ')")]
	[SerializeField] private string deplacementSceneName;

	private static PhaseLoader instance = null; // Reference towards the instance of this class.
	private static string phaseName = null; // Name of the next phase to load.
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
	 * 'phaseType' is the type of the phase loaded : PhaseLoader.Type.SHOOT or PhaseLoader.Type.DEPLACEMENT.
	 * 'level' is the number of the level.
	 * 'phase' is the number of the phase inside the level.
	 */
	public static void Load(Type phaseType, int level, int phase) {
		loadPhaseNextSceneLoading = true;
		phaseName = "World " + level.ToString("D2") + " " + phase.ToString("D2");

		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		string system = "Windows - ";
		#else
		string system = "Android - ";
		#endif
		if (phaseType == Type.SHOOT)
			Application.LoadLevel (system + instance.shootSceneName);
		else
			Application.LoadLevel (system + instance.deplacementSceneName);
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
