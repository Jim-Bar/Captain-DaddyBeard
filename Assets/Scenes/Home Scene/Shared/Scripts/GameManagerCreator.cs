using UnityEngine;
using System.Collections;

/*
 * Create the game manager the first time the scene is loaded.
 * 
 * Both Android and Windows.
 */
public class GameManagerCreator : MonoBehaviour {

	// Prefab of the game manager.
	[SerializeField] private GameObject gameManager = null;

	private static bool gameManagerCreated = false;

	private void Awake () {
		// If the game manager has not been created already, create it.
		if (!gameManagerCreated)
		{
			Instantiate (gameManager);
			gameManagerCreated = true;
		}

		// Destroy itself (we do not need it anymore).
		Destroy (gameObject);
	}
}
