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
		PhaseLoader.LoadNext ();
	}
	
	public void ReloadGameAnd(){
		PhaseLoader.Reload ();
	}
	
	public void BacktoHomeAnd(){
		Application.LoadLevel ("Android - HomeScene");
	}
}
