using UnityEngine;
using System.Collections;

/*
 * A bullet is destroyed when touching any object but the player nor the target.
 * If the touched object is a monster, destroy it.
 * 
 * Windows and Android only (but does nothing on Android, bullet are manager on the server).
 */
public class Explode : MonoBehaviour {

	#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

	// Reference towards the player.
	private GameObject player = null;

	// Reference towards the target.
	private GameObject target = null;

	private void Start () {
		string playerTag = "Player";
		GameObject[] players = GameObject.FindGameObjectsWithTag (playerTag);
		if (players.Length == 0)
			Debug.LogError (GetType ().Name + " : No player found. The player must have the tag \"" + playerTag + "\".");
		else
			player = players[0];

		target = GameObject.Find ("Target");
		if (target == null)
			Debug.LogError (GetType ().Name + " : No target found.");
	}

	// Destroy the bullet when it touches something.
	private void OnTriggerEnter2D (Collider2D other) {
		// If the object is a monster, destroy it.
		if (other.gameObject.CompareTag ("Enemy"))
			Network.Destroy (other.gameObject);

		// If the object is not the player nor the target, destroy the bullet.
		if (other.gameObject != player && other.gameObject != target)
			Network.Destroy (gameObject);
	}

	private void OnBecameInvisible () {
		// Destroy the bullet if it is out of the screen.
		Network.Destroy (gameObject);
	}

	#endif
}
