using UnityEngine;
using System.Collections;

public class LaunchGame : MonoBehaviour {

	// Reference towards the target.
	[SerializeField] private GameObject target;

	// If the target is on the object.
	private bool targetted = false;

	void Start () {
		// Register TryLaunchingGame() in the RPC wrapper.
		RPCWrapper.RegisterMethod (TryLaunchingGame);
	}

	// Launch the game if the play "button" is targetted when the user press the button on his tablet.
	public void TryLaunchingGame () {
		if (targetted)
		{
			RPCWrapper.RPC ("LoadGame", RPCMode.Others); // Say the clients to launch the game.
			Application.LoadLevel ("Windows - LevelScene");
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject == target)
			targetted = true;
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject == target)
			targetted = false;
	}
}
