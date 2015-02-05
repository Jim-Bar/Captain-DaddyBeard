using UnityEngine;
using System.Collections;

public class ComboRequest : MonoBehaviour {

	GameObject currentPlatform = null;

	//RPC function 
	public void ComboAnswer (bool ok) {
		PlatformElevator ct = currentPlatform.GetComponent<PlatformElevator>();
		//ct.ValidateCombo (true);
		ct.ValidateCombo (ok);
	}

	// Use this for initialization
	void Start () {
		RPCWrapper.RegisterMethod(ComboAnswer);
	}

	public void AskCombo(GameObject platform, string combo) {

		currentPlatform = platform;

		if (Network.connections.Length > 0) {
			Debug.Log("Combo"); 
			RPCWrapper.RPC ("ComboTask", RPCMode.Others, combo);
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
