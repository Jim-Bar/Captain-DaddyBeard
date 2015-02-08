using UnityEngine;
using System.Collections;

public class ToTheGame : MonoBehaviour {
	
	// Reference towards the target.
	[SerializeField] private GameObject target;
	
	// If the target is on the object.
	private bool targetted = false;
	
	void Start () {
		//this line is very important, that is the way we use to call any sound from the sound manager
	}
	
	// Launch the game if the play "button" is targetted when the user press the button on his tablet.
	public void TryLaunchingGame () {
		if (targetted){
			GameObject.Find ("SoundManager").GetComponent<SoundManager> ().StopThemes();
			GameObject.Find ("SoundManager").GetComponent<SoundManager> ().PlayLevelTheme(PhaseLoader.CurrentLevel);
			RPCWrapper.RPC ("ToTheGameAndroidFunc", RPCMode.Others);
			PhaseLoader.Load ();
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
