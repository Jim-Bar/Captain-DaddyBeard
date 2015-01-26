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
		Debug.Log ("Continue Pressed received Windows > Android");
		PhaseLoader.LoadNext ();
	}
	
	public void ReloadGameAnd(){
		Debug.Log ("Play Again received Windows > Android");
		PhaseLoader.Reload ();
	}
	
	public void BacktoHomeAnd(){
		Debug.Log ("Quit received Windows > Android");
		Application.LoadLevel ("Android - HomeScene");
	}
}
