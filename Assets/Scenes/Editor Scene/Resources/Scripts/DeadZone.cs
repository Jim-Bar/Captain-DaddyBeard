using UnityEngine;
using System.Collections;

/*
 * Attached to a collider, check if a player enters the collider, and kill the player.
 * In shoot mode, erase the score earned during the sequence and subtract a malus.
 * 
 * Both Windows and Android (although it does nothing on Android).
 * Use this script only for the prefabs of phases.
 * 
 * The player(s) must have the tag "Player".
 */
public class DeadZone : MonoBehaviour {

	#if UNITY_STANDALONE_WIN

	private GameObject[] players;
	
	private void Start () {
		string playerTag = "Player";
		players = GameObject.FindGameObjectsWithTag (playerTag);
		if (players.Length == 0)
			Debug.LogError (GetType ().Name + " : No player found. The player must have the tag \"" + playerTag + "\".");
	}
	
	// Only reload the phase when we are the server. Otherwise we wait for an RPC to notify us.
	private void OnTriggerEnter2D (Collider2D other) {
		foreach (GameObject player in players) // For all players...
			if (player == other.gameObject) // ...if one is in the arrival area, load next phase.
			{
				// Erase the score of the sequence and subtract a malus.
				if (PhaseLoader.CurrentPhaseType == PhaseLoader.Type.SHOOT)
					Player.score1.Wipe ();

				RPCWrapper.RPC ("ReloadCurrentPhase", RPCMode.Others); // Tell the clients to reload the phase.
				PhaseLoader.ReloadPhase ();
				break; // Load next level only one time.
			}
	}

	#endif
}
