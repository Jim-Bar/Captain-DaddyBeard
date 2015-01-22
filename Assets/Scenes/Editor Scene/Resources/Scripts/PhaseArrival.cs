using UnityEngine;
using System.Collections;

/*
 * Attached to a collider, check if a player enters the collider, and load next scene accordingly.
 * Both Windows and Android.
 * 
 * The player(s) must have the tag "Player".
 */
public class PhaseArrival : MonoBehaviour {

	[Tooltip("Is this phase the last of the current level ? If yes, information below will be ignored")]
	[SerializeField] private bool isFinalPhase = false;
	[Tooltip("Number of the level in which the next phase is")]
	[SerializeField] private int nextLevel = 1;
	[Tooltip("Number of the next phase")]
	[SerializeField] private int nextPhase = 2;
	[Tooltip("Type of the next phase")]
	[SerializeField] private PhaseLoader.Type nextPhaseType = PhaseLoader.Type.SHOOT;

	private GameObject[] players;

	private void Start () {
		string playerTag = "Player";
		players = GameObject.FindGameObjectsWithTag (playerTag);
		if (players.Length == 0)
			Debug.LogError (GetType ().Name + " : No player found. The player must have the tag \"" + playerTag + "\".");
	}

	private void OnTriggerEnter2D (Collider2D other) {
		foreach (GameObject player in players)
			if (player == other.gameObject)
			{
				if (!isFinalPhase)
					PhaseLoader.Load (nextPhaseType, nextLevel, nextPhase);
				else
					#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
					Application.LoadLevel ("Windows - ScoreScene");
					#else
					Application.LoadLevel ("Android - ScoreScene");
					#endif
				break; // Load next level only one time.
			}
	}
}
