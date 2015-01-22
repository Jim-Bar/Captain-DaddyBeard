using UnityEngine;
using System.Collections;

public class ToTheGameAndroid : MonoBehaviour {

	public void OnButtonPressedAndroid(){
		RPCWrapper.RPC("OnButtonPressedWin", RPCMode.Server);
		PhaseLoader.Load ();
	}
}
