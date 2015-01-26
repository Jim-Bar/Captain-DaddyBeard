using UnityEngine;
using System.Collections;

public class ScoreMenu : MonoBehaviour {

	public void ContinuePressed(){
		RPCWrapper.RPC ("NextLevelWin", RPCMode.Server);
	}

	public void PlayAgainPressed(){
		RPCWrapper.RPC ("ReloadGameWin", RPCMode.Server);	
	}

	public void QuitPressed(){
		RPCWrapper.RPC ("BacktoHomeWin", RPCMode.Server);
	}
}
