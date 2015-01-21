using UnityEngine;
using System.Collections;

public class LevelChosen : MonoBehaviour {

	public int levelChosen;

	// Use this for initialization
	void Start () {
		levelChosen = 0;
	}

	public void ValidateLevel(){
		if(levelChosen != 0){
			RPCWrapper.RPC("LoadWeaponLevel", RPCMode.Server);
			Application.LoadLevel ("Android - WeaponScene");
			PhaseLoader.Prepare (PhaseLoader.Type.SHOOT, levelChosen, 1);
		}
	}
}
