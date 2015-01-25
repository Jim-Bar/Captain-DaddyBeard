using UnityEngine;
using System.Collections;

/*
 * A bullet is destroyed when touching any object but the player nor the target nor the arrival.
 * If the touched object is a monster, destroy it.
 * 
 * Windows and Android only (on Android only display an explosion).
 */
public class Explode : MonoBehaviour {

	[SerializeField] private GameObject bulletExplosion = null;

	#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

	// Reference towards the player.
	private GameObject player = null;

	// Reference towards the target.
	private GameObject target = null;

	// Reference towards the arrival.
	private GameObject arrival = null;

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

		string arrivalTag = "Finish";
		arrival = GameObject.FindGameObjectWithTag (arrivalTag);
		if (arrival == null)
			Debug.LogError (GetType ().Name + " : No arrival found. The arrival must have the tag \"" + arrivalTag + "\".");
	}

	// Destroy the bullet when it touches something.
	private void OnTriggerEnter2D (Collider2D other) {
		// If the object is a monster, destroy it.
		if (other.gameObject.CompareTag ("Enemy"))
			Network.Destroy (other.gameObject);

		// If the object is not the player nor the target, destroy the bullet nor the arrival.
		if (other.gameObject != player && other.gameObject != target && other.gameObject != arrival)
			Network.Destroy (gameObject);
	}

	private void Update () {
		// Destroy the bullet if it is too far away.
		if (Vector3.Distance (transform.position, Camera.main.transform.position) > 20)
			Network.Destroy (gameObject);
	}

	#endif

	private void OnDestroy () {
		Instantiate (bulletExplosion, transform.position + Vector3.back, Quaternion.identity);
	}
}
