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
		int choiceLevel = GameObject.Find ("Image").GetComponent<UpdateLevelWindows> ().levelChoice;
		if (targetted){
			if(choiceLevel > 0){
				RPCWrapper.RPC ("ValidateLevel", RPCMode.Others, choiceLevel); // Say the clients to launch the game.
				PhaseLoader.Prepare (choiceLevel);
				Application.LoadLevel ("Windows - WeaponScene");
			}
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
