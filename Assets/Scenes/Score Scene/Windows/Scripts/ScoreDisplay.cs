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
		GameObject.Find ("SoundManager").GetComponent<SoundManager> ().StopThemes();
		GameObject.Find ("SoundManager").GetComponent<SoundManager> ().SoundWin();
	}

	//Change it when we have more than one level
	public void NextLevelWin(){
		RPCWrapper.RPC ("NextLevelAnd", RPCMode.Others);
		PhaseLoader.LoadNext ();
	}

	public void ReloadGameWin(){
		RPCWrapper.RPC ("ReloadGameAnd", RPCMode.Others);
		PhaseLoader.Reload ();
	}

	public void BacktoHomeWin(){
		GameObject.Find ("SoundManager").GetComponent<SoundManager> ().SoundTheme();
		RPCWrapper.RPC ("BacktoHomeAnd", RPCMode.Others);
		Application.LoadLevel ("Windows - HomeScene");
	}
	
}
