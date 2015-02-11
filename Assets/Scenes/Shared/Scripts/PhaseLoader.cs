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

	// Hold the type of the first phase of each level.
	[Tooltip("Types of the first phases of each level")]
	[SerializeField] private Type[] firstPhasesTypes;

	// Picture used for the transitions between phases.
	[Tooltip("Transition picture")]
	[SerializeField] private Texture2D transitionPicture;

	// Font used for the transitions between phases.
	[Tooltip("Transition font")]
	[SerializeField] private Font transitionFont;

	private static PhaseLoader instance = null; // Reference towards the instance of this class.
	private static string phaseName = null; // Name of the next phase to load.
	private static int nextLevel = 1; // The next level to load.
	private static int nextPhase = 1; // The next phase to load.
	private static Type nextPhaseType = Type.SHOOT; // Type of the next phase to load.
	private static bool loadPhaseNextSceneLoading = false; // Will the phase loader load a phase on the next scene loading ?
	private static int currentLevel = 1; // The level currently or lastly played.
	private static int currentPhase = 1; // The phase currently or lastly played.
	private static Type currentPhaseType = Type.SHOOT; // The type of the phase currently or lastly played.

	// Getters.
	public static int NextLevel { // Used for the music.
		get { return nextLevel; }
	}
	public static int CurrentLevel { // Used for the music.
		get { return currentLevel; }
	}
	public static Type CurrentPhaseType { // Getter used by PhaseArrival.
		get { return currentPhaseType; }
	}

	// Lock the phase loader when already loading a new phase.
	private static bool isLoading = false;

	// Is the transition currently displayed ? 
	private static bool transitionDisplayed = false;
	private static float timeTransitionBegan = 0;
	private GUIStyle transitionTextStyle;

	// Type of the phase to be loaded.
	public enum Type {
		SHOOT,
		DEPLACEMENT
	};

	private void Awake () {
		// Check taht there is only one instance.
		if (instance == null)
			instance = this;
		else
			Debug.LogError (GetType ().Name + " : Two instances of " + GetType ().Name + " running at the same time.");

		// Check that fields are filled.
		if (shootSceneName == null)
			Debug.LogError (GetType ().Name + " : The field \"Shoot Scene\" is empty !");
		if (deplacementSceneName == null)
			Debug.LogError (GetType ().Name + " : The field \"Deplacement Scene\" is empty !");
		if (transitionPicture == null)
			Debug.LogError (GetType ().Name + " : The field \"Transition Picture\" is empty !");
		if (transitionFont == null)
			Debug.LogError (GetType ().Name + " : The field \"Transition Font\" is empty !");

		// Initialize text of the transitions.
		transitionTextStyle = new GUIStyle ();
		transitionTextStyle.font = transitionFont;
		transitionTextStyle.fontSize = Screen.width / 13;
		transitionTextStyle.normal.textColor = Color.black;
		transitionTextStyle.alignment = TextAnchor.MiddleCenter;

		// Keep the phase loader alive through scenes.
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
		if (!isLoading)
		{
			CheckExist ();
			nextLevel = level;
			nextPhase = phase;
			nextPhaseType = type;
			phaseName = "World " + level.ToString("D2") + " " + phase.ToString("D2");
		}
	}

	// Load a phase based on the data set by Prepare ().
	public static void Load () {
		if (!isLoading)
		{
			isLoading = true; // Lock the loader.

			CheckExist ();
			loadPhaseNextSceneLoading = true;

			// The transition is made in OnGUI ().
			transitionDisplayed = true;
			timeTransitionBegan = Time.unscaledTime;

			// Wait for the transition to load next scene.
			instance.Invoke ("ActualLoad", 1);
		}
	}

	// Load the current level from the beginning.
	public static void Reload () {
		Load (currentLevel);
	}

	// Load the current phase of the current level.
	public static void ReloadPhase () {
		Load (currentLevel, currentPhase, currentPhaseType);
	}

	// Load the next level.
	public static void LoadNext () {
		if (currentLevel == instance.firstPhasesTypes.Length)
			Load (1);
		else
			Load (currentLevel + 1);
	}

	// Do the load. Note that this function is not static because it is called by Invoke ().
	private void ActualLoad () {
		if (nextPhaseType == Type.SHOOT)
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
			isLoading = false; // Unlock the loader.

			// Update state.
			loadPhaseNextSceneLoading = false;
			Time.timeScale = 0;
			currentLevel = nextLevel;
			currentPhase = nextPhase;
			currentPhaseType = nextPhaseType;

			// Load the phase, reset the score.
			#if UNITY_STANDALONE_WIN
			LoadPhase ();

			// Reset the score at the beginning of a new level.
			if (currentPhase == 1)
			{
				Player.score1.Reset ();
				Player.score2.Reset ();
			}
			#elif UNITY_ANDROID
			// Do not load the phase on Android when the phase is deplacement.
			if (nextPhaseType != Type.DEPLACEMENT)
				LoadPhase ();

			// Create an object which will wait for the server notification to load next scene.
			GameObject levelLoadWaiter = new GameObject ("Level Loading Waiter");
			levelLoadWaiter.AddComponent<WaitLevelLoad> ();
			#endif
		}
	}

	// Load a prefab of a phase.
	private void LoadPhase () {
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

	private void OnGUI () {
		if (transitionDisplayed)
		{
			float deltaTotal = Screen.width / 20;
			const float transitionDuration = 2;
			const float transitionArrivalDuration = 0.5f;
			const float transitionLeavingDuration = transitionArrivalDuration;
			float timeSinceStartup = Time.unscaledTime;
			float x = 0;
			if (timeSinceStartup - timeTransitionBegan <= transitionArrivalDuration)
				x = Mathf.Exp (30 * transitionArrivalDuration * (transitionArrivalDuration - timeSinceStartup + timeTransitionBegan));
			else if (timeSinceStartup - timeTransitionBegan <= transitionArrivalDuration + transitionDuration)
				x = - deltaTotal * (timeSinceStartup - timeTransitionBegan - transitionDuration - transitionArrivalDuration) / transitionDuration - deltaTotal;
			else if (timeSinceStartup - timeTransitionBegan <= transitionArrivalDuration + transitionDuration + transitionLeavingDuration)
				x = - Mathf.Exp (30 * transitionLeavingDuration * (timeSinceStartup - timeTransitionBegan - transitionArrivalDuration - transitionDuration)) - deltaTotal;
			else
			{
				transitionDisplayed = false;
				Time.timeScale = 1;
			}

			if (transitionDisplayed) // Retest because it might have changed.
			{
				GUI.DrawTexture (new Rect(x, 0, Screen.width + deltaTotal, Screen.height), transitionPicture, ScaleMode.ScaleAndCrop);
				GUI.Label (new Rect(x, 0, Screen.width, Screen.height), nextPhaseType == Type.SHOOT ? "Visez l'écran !" : "Tablette à plat !", transitionTextStyle);
			}
		}
	}
}
