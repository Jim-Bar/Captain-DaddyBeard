using UnityEngine;
using System.Collections;

/*
 * Attached to a collider, check if a player enters the collider, and load next scene accordingly.
 * In shoot mode, add the score earned during the sequence to the total score.
 * 
 * Both Windows and Android (although it does nothing on Android).
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
		// Add the score of the sequence to the total score.
		if (PhaseLoader.CurrentPhaseType == PhaseLoader.Type.SHOOT)
			Player.score1.Confirm ();

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
