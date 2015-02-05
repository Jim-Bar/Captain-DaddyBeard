using UnityEngine;
using System.Collections;

public class LevelChosen : MonoBehaviour {

	public int levelChosen;

	// Use this for initialization
	void Start () {
		GameObject.Find ("SoundManager").GetComponent<SoundManager> ().SoundValidate();
		levelChosen = 0;
	}

	public void ValidatePressed(){
		RPCWrapper.RPC("TryLaunchingWeaponScene", RPCMode.Server);
		GameObject.Find ("SoundManager").GetComponent<SoundManager> ().SoundValidate();
	}	
}
