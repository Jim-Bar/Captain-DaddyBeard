using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToTheGameWin : MonoBehaviour {

	[SerializeField] Text player1;
	[SerializeField] Text player2;
	[SerializeField] Image weapon21;
	[SerializeField] Image weapon22;
	[SerializeField] Image weapon23;

	void Start () {
		RPCWrapper.RegisterMethod (OnButtonPressedWin);
		if(Network.connections.Length == 1){
			player1.enabled = false;
			player2.enabled = false;
			weapon21.enabled = false;
			weapon22.enabled = false;
			weapon23.enabled = false;
		}
	}

	public void OnButtonPressedWin(){
		PhaseLoader.Load ();
	}
}
