using UnityEngine;
using System.Collections;

/*
 * Put the players at the position of this object.
 * 
 * The players must have the tag "Player".
 */
public class PhaseStart : MonoBehaviour {

	private void Start () {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		if (players.Length == 0)
			Debug.LogError (GetType ().Name + " : No player found");

		foreach (GameObject player in players)
			player.transform.position = transform.position;

		GameObject.Destroy (gameObject);
	}
}
