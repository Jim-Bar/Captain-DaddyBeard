using UnityEngine;
using System.Collections;

public class LevelChosen : MonoBehaviour {

	public int levelChosen;

	// Use this for initialization
	void Start () {
		levelChosen = 0;
	}

	public void ValidatePressed(){
		RPCWrapper.RPC("TryLaunchingWeaponScene", RPCMode.Server);
	}	
}
