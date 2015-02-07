using UnityEngine;
using System.Collections;

/*
 * A bullet is destroyed when touching any object but the player nor the arrival.
 * If the touched object is a monster, destroy it.
 * 
 * Windows and Android only (on Android only display an explosion).
 */

#pragma warning disable 414
public class Explode : MonoBehaviour {

	// Prefab of a particle system.
	[SerializeField] private GameObject bulletExplosion = null;



	// Reference towards the player.
	private GameObject player = null;

	// Reference towards the arrival.
	private GameObject arrival = null;

	private void Start () {
		if (bulletExplosion == null)
			Debug.LogError (GetType ().Name + " : Field bullet explosion is empty");

		string playerTag = "Player";
		GameObject[] players = GameObject.FindGameObjectsWithTag (playerTag);
		if (players.Length == 0)
			Debug.LogError (GetType ().Name + " : No player found. The player must have the tag \"" + playerTag + "\".");
		else
			player = players[0];

		string arrivalTag = "Finish";
		arrival = GameObject.FindGameObjectWithTag (arrivalTag);
		if (arrival == null)
			Debug.LogError (GetType ().Name + " : No arrival found. The arrival must have the tag \"" + arrivalTag + "\".");

		Destroy (gameObject, 5);
	}

	// Destroy the bullet when it touches something.
	private void OnTriggerEnter2D (Collider2D other) {
		// If the object is a monster, destroy it.
		if (other.gameObject.CompareTag ("Enemy")) {
				Destroy (other.gameObject);
				Destroy (gameObject);
				Instantiate (bulletExplosion, transform.position + 2 * Vector3.back, Quaternion.identity);
		}

	}




}
