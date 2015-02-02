using UnityEngine;
using System.Collections;

public class ToTheGameAndroid : MonoBehaviour {

	void Start () {
		RPCWrapper.RegisterMethod (ToTheGameAndroidFunc);
	}

	public void OnButtonPressedAndroid(){
		RPCWrapper.RPC("OnButtonPressedWin", RPCMode.Server);
		PhaseLoader.Load ();
	}

	public void ToTheGameAndroidFunc(){
		PhaseLoader.Load ();
	}
}
