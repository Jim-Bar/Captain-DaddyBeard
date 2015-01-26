using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplay : MonoBehaviour {

	[SerializeField] Text totalScore;
	[SerializeField] Text score1;
	[SerializeField] Text score2;
	[SerializeField] Image imageScore1;
	[SerializeField] Image imageScore2;
	// Use this for initialization
	void Start () {
		RPCWrapper.RegisterMethod (BacktoHomeWin);
		RPCWrapper.RegisterMethod (ReloadGameWin);
		RPCWrapper.RegisterMethod (NextLevelWin);
		if(Network.connections.Length == 1){
			int iScore1 = Player.score1.Get();
			totalScore.text = iScore1.ToString();
			score1.enabled = false;
			score2.enabled = false;
			imageScore1.enabled = false;
			imageScore2.enabled = false;
		}
		else{
			int iScore1 = Player.score1.Get();
			int iScore2 = Player.score2.Get();
			int iTotalScore = iScore1 + iScore2;
			score1.text = iScore1.ToString();
			score1.text = iScore2.ToString();
			totalScore.text = iTotalScore.ToString();
		}
	}

	//Change it when we have more than one level
	public void NextLevelWin(){
		Debug.Log ("Continue Pressed received Android > Windows");
		Debug.Log ("Continue Pressed try to be sent Windows > Android");
		RPCWrapper.RPC ("NextLevelAnd", RPCMode.Others);
		Debug.Log ("Continue Pressed sent Windows > Android");
		PhaseLoader.LoadNext ();
	}

	public void ReloadGameWin(){
		Debug.Log ("Play Again Pressed received Android > Windows");
		Debug.Log ("Play Again Pressed try to be sent Windows > Android");
		RPCWrapper.RPC ("ReloadGameAnd", RPCMode.Others);
		Debug.Log ("Play Again Pressed sent Windows > Android");
		PhaseLoader.Reload ();
	}

	public void BacktoHomeWin(){
		Debug.Log ("Quit Pressed received Android > Windows");
		Debug.Log ("Quit Pressed try to be sent Windows > Android");
		RPCWrapper.RPC ("BacktoHomeAnd", RPCMode.Others);
		Debug.Log ("Quit Pressed sent Windows > Android");
		Destroy(GameObject.Find("GameManager"));
		Application.LoadLevel ("Windows - HomeScene");
	}
	
}
