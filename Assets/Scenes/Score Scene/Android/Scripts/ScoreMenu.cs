using UnityEngine;
using System.Collections;

public class ScoreMenu : MonoBehaviour {

	public void ContinuePressed(){
		Debug.Log ("Continue Pressed try to be sent Android > Windows");
		RPCWrapper.RPC ("NextLevelWin", RPCMode.Server);
		Debug.Log ("Continue Pressed sent Android > Windows");
	}

	public void PlayAgainPressed(){
		Debug.Log ("Play Again try to be sent Android > Windows");
		RPCWrapper.RPC ("ReloadGameWin", RPCMode.Server);
		Debug.Log ("Play Again sent Android > Windows");
	}

	public void QuitPressed(){
		Debug.Log ("Quit Pressed try to be sent Android > Windows");
		RPCWrapper.RPC ("BacktoHomeWin", RPCMode.Server);
		Debug.Log ("Quit Pressed sent Android > Windows");
	}
}
