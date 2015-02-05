using UnityEngine;
using System.Collections;

public class ToTheWeaponScene : MonoBehaviour {
	
	// Reference towards the target.
	[SerializeField] private GameObject target;
	
	// If the target is on the object.
	private bool targetted = false;
	
	void Start () {
		//this line is very important, that is the way we use to call any sound from the sound manager
		RPCWrapper.RegisterMethod (TryLaunchingWeaponScene);
	}
	
	// Launch the game if the play "button" is targetted when the user press the button on his tablet.
	public void TryLaunchingWeaponScene () {
		if (targetted)
		{
			RPCWrapper.RPC ("LoadWeaponAndroid", RPCMode.Others); // Say the clients to launch the game.
			Application.LoadLevel ("Windows - WeaponScene");
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
