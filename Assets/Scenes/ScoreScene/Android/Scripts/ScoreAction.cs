using UnityEngine;
using System.Collections;

public class ScoreAction : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RPCWrapper.RegisterMethod (NextLevelAnd);
		RPCWrapper.RegisterMethod (ReloadGameAnd);
		RPCWrapper.RegisterMethod (BacktoHomeAnd);
	}
	
	public void NextLevelAnd(){
		PhaseLoader.Load ();// ICIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII
	}
	
	public void ReloadGameAnd(){
		PhaseLoader.Load ();// ICIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII
	}
	
	public void BacktoHomeAnd(){
		Application.LoadLevel ("Android - HomeScene");
	}
}
