using UnityEngine;
using System.Collections;

public class NextLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RPCWrapper.RegisterMethod (ValidateLevel);
	}

	public void ValidateLevel(int levelChosen){
		if(levelChosen != 0){
			Application.LoadLevel ("Android - WeaponScene");
			PhaseLoader.Prepare (levelChosen);
		}
	}
}
