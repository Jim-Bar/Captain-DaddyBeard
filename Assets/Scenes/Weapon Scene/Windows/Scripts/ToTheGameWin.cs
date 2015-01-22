using UnityEngine;
using System.Collections;

public class ToTheGameWin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RPCWrapper.RegisterMethod (OnButtonPressedWin);
	}

	public void OnButtonPressedWin(){
		Application.LoadLevel ("Windows - ShootScene");
	}
}
