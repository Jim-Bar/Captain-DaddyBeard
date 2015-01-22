using UnityEngine;
using System.Collections;

public class ToTheGameAndroid : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButtonPressedAndroid(){
		RPCWrapper.RPC("OnButtonPressedWin", RPCMode.Server);
		Application.LoadLevel ("Android - ShootScene");
	}
}
