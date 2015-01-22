using UnityEngine;
using System.Collections;

public class ToTheGameWin : MonoBehaviour {
	
	void Start () {
		RPCWrapper.RegisterMethod (OnButtonPressedWin);
	}

	public void OnButtonPressedWin(){
		PhaseLoader.Load ();
	}
}
