using UnityEngine;
using System.Collections;

public class PressLaunchGame : MonoBehaviour {

	public void ValidateButtonPressed () {
		RPCWrapper.RPC ("TryLaunchingGame", RPCMode.Server);

		// Register LoadGame() in the RPC wrapper.
		RPCWrapper.RegisterMethod (LoadGame);
	}

	public void LoadGame () {
		Debug.Log (GetType ().Name + " : Launching game !");
		Application.LoadLevel ("Android - PlayScene");
	}
}
