using UnityEngine;
using System.Collections;

/*
 * Load a phase (shoot or deplacement), both Windows and Android.
 * /!\ ONLY ONE INSTANCE OF THIS CLASS SHOULD BE USED AT THE SAME TIME /!\
 * 
 * HOW TO USE THIS CLASS :
 * 1 - First of all, add this script to a game manager and fill the fields 'Shoot Scene Name' and 'Deplacement Scene Name'
 * 	   which are the names of the scenes used for shooting and deplacement.
 * 2 - Fill also the field 'First Phases Types' which are just the type of the first phases of each level. There must have
 *     as many elements as there are playable levels in the game.
 * 3 - The phase loader automatically loads next phases when you have loaded the first phase of the level. To do so, call :
 * 	   PhaseLoader.Load (levelIndex);
 * 	   where 'levelIndex' is the number of the level (the first level should have the index 1, the second level 2, etc...).
 * 3'- Alternatively, you can also load any other phase of a level (not only the first). A phase is identified by a type
 *     (shoot or deplacement), a level number and a phase number. For instance if I want to load the third phase (let's say
 *     it is a deplacement phase) of the second level, I call :
 *     PhaseLoader.Load (2, 3, PhaseLoader.Type.DEPLACEMENT);
 * Note : You can also split the loading in two parts : PhaseLoader.Prepare (...) then PhaseLoader.Load ().
 * 
 * How to create a phase and load it later :
 * 1 - In the editor scene, create an object 'World XX YY' where XX is the level number and YY the phase number.
 * 2 - Add all the objects you want as children of 'World XX YY'.
 * 3 - Do not forget to put an start and an arrival
 * 4 - Give to all the ground and platforms the layer 'World', and give the tag 'World' to the container 'World XX YY'.
 * 5 - Make a prefab of 'World XX YY' and put in the folder Scenes/Editor Scene/Resources/Phases.
 * 6 - You can now access it using the previously mentioned Load () method.
 * 
 */
public class PhaseLoader : MonoBehaviour {

	// Names of the scenes of shoot and deplacement.
	[Tooltip("Name of the scene of shoot")]
	[SerializeField] private string shootSceneName;
	[Tooltip("Name of the scene of deplacement")]
	[SerializeField] private string deplacementSceneName;

	// Held the type of the first phase of each level.
	[Tooltip("Types of the first phases of each level")]
	[SerializeField] private Type[] firstPhasesTypes;
	
	private static PhaseLoader instance = null; // Reference towards the instance of this class.
	private static string phaseName = null; // Name of the next phase to load.
	private static Type phaseType = Type.SHOOT; // Type of the next phase to load.
	private static bool loadPhaseNextSceneLoading = false; // Will the phase loader load a phase on the next scene loading ?
	private static int currentLevel = 0; // The level currently or lastly played.

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
	public static void Load (int level) { Load (level, 1, instance.firstPhasesTypes[level - 1]); }
	public static void Load (int level, int phase, Type type) {
		Prepare (level, phase, type);
		Load ();
	}

	// Register phase data before loading it with Load ().
	public static void Prepare (int level) { Prepare (level, 1, instance.firstPhasesTypes[level - 1]); }
	public static void Prepare (int level, int phase, Type type) {
		CheckExist ();
		currentLevel = level;
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

	// Load the current level from the beginning.
	public static void Reload () {
		Load (currentLevel);
	}

	// Load the next level.
	public static void LoadNext () {
		Load (currentLevel + 1);
	}

	// Log an error message if there is no instance of this class.
	private static void CheckExist () {
		if (instance == null)
			Debug.LogError ("Phase Loader : No instance found !");
	}

	// Load a phase if the phase loader has been told to do so ('loadPhaseNextSceneLoading' set to 'true').
	private void OnLevelWasLoaded (int level) {
		// Do not load the phase on Android when the phase is deplacement.
		#if UNITY_STANDALONE_WIN
		if (loadPhaseNextSceneLoading)
			LoadPhase ();
		#elif UNITY_ANDROID
		if (loadPhaseNextSceneLoading && phaseType != Type.DEPLACEMENT)
			LoadPhase ();

		// Create an object which will wait for the server notification to load next scene.
		GameObject phaseArrivalWaiter = new GameObject ("Phase Arrival Waiter");
		phaseArrivalWaiter.AddComponent<PhaseWaitArrival> ();
		#endif
	}

	// Load a prefab of a phase.
	private void LoadPhase () {
		loadPhaseNextSceneLoading = false;
		Object world = Resources.Load ("Phases/" + phaseName);
		if (world != null)
		{
			if (Instantiate (world, Vector3.zero, Quaternion.identity) != null)
				Debug.Log (GetType ().Name + " : Successfully loaded phase \"" + phaseName + "\"");
			else
				Debug.LogError (GetType ().Name + " : World instantiation failed");
		}
		else
			Debug.LogError (GetType ().Name + " : Phase \"" + phaseName + "\" not found.");
	}
}
