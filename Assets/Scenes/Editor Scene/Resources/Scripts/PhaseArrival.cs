using UnityEngine;
using System.Collections;

/*
 * Attached to a collider, check if a player enters the collider, and load next scene accordingly.
 * In shoot mode, add the score earned during the sequence to the total score.
 * In deplacement mode, add the items earned during the sequence to the total score or player health.
 * 
 * Both Windows and Android (although it does nothing on Android excepted hiding itself).
 * Use this script only for the prefabs of phases.
 * 
 * The player(s) must have the tag "Player".
 */

// 'Pragma' removes warnings as the fields bellow are not used on Android (see the comment bellow).
#pragma warning disable 414
public class PhaseArrival : MonoBehaviour {

	[Tooltip("Is this phase the last of the current level ? If yes, information below will be ignored")]
	[SerializeField] private bool isFinalPhase = false;
	[Tooltip("Number of the level in which the next phase is")]
	[SerializeField] private int nextLevel = 1;
	[Tooltip("Number of the next phase")]
	[SerializeField] private int nextPhase = 2;
	[Tooltip("Type of the next phase")]
	[SerializeField] private PhaseLoader.Type nextPhaseType = PhaseLoader.Type.SHOOT;

	private void Awake () {
		// Hide the flag.
		GetComponent<SpriteRenderer> ().enabled = false;
	}

	/* 
	 * Do NOT move this line further up (if you do, the settings in the editor for the fields
	 * above will be lost when switching from Windows to Android in the Unity build settings).
	 */
	#if UNITY_STANDALONE_WIN

	private GameObject[] players;
	
	private void Start () {
		string playerTag = "Player";
		players = GameObject.FindGameObjectsWithTag (playerTag);
		if (players.Length == 0)
			Debug.LogError (GetType ().Name + " : No player found. The player must have the tag \"" + playerTag + "\".");
	}

	// The only purpose of Update () is to skip phases with enter.
	private void Update () {
		if(Input.GetKeyUp(KeyCode.Return))
			LoadNextPhase ();
	}

	// Only load next phase when we are the server. Otherwise we wait for an RPC to notify us.
	private void OnTriggerEnter2D (Collider2D other) {
		foreach (GameObject player in players) // For all players...
			if (player == other.gameObject) // ...if one is in the arrival area, load next phase.
			{
				LoadNextPhase ();
				break; // Load next level only one time.
			}
	}

	private void LoadNextPhase () {
		// Add the score and health to the total score and health.
		if (PhaseLoader.CurrentPhaseType == PhaseLoader.Type.DEPLACEMENT)
		{
			Inventory inventory = players[0].GetComponent<Inventory> ();
			if (inventory == null)
				Debug.LogError (GetType ().Name + " : The player must have a inventory in deplacement mode !");
			else
			{
				Player.health.Add (inventory.getLifeBonus ());
				Player.score1.Add (inventory.getScoreBonus ());
			}

			Timer timer = players[0].GetComponent<Timer> ();
			if (timer == null)
				Debug.LogError (GetType ().Name + " : The player must have a timer in deplacement mode !");
			else
				Player.score1.Add (timer.getScore ());
		}

		// Add the score of the sequence to the total score.
		Player.score1.Commit ();

		// Load the right scene.
		if (!isFinalPhase)
		{
			RPCWrapper.RPC ("LoadNextPhase", RPCMode.Others, (int) nextPhaseType, nextLevel, nextPhase); // Tell the clients to load the next phase.
			PhaseLoader.Load (nextLevel, nextPhase, nextPhaseType);
		}
		else
		{
			RPCWrapper.RPC ("LoadScoreScene", RPCMode.Others); // Tell the clients to load scores scene.
			Application.LoadLevel ("Windows - ScoreScene");
		}
	}
	
	#endif
}
