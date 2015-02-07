using UnityEngine;
using System.Collections;

public class PauseGameWin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RPCWrapper.RegisterMethod (GoHomeScene);
		RPCWrapper.RegisterMethod (PauseButtonPressed);
		RPCWrapper.RegisterMethod (ResumeButtonPressed);
		RPCWrapper.RegisterMethod (RestartButtonPressed);
	}
	


	public void PauseButtonPressed()
	{
		Time.timeScale = 0;
	}
	
	public void ResumeButtonPressed()
	{
		Time.timeScale = 1;
	}

	public void RestartButtonPressed()
	{
		Time.timeScale = 1;
		PhaseLoader.Reload ();
		Player.health.Add (6);
	}
	
	public void GoHomeScene()
	{	
		Time.timeScale = 1;
		Application.LoadLevel ("Windows - HomeScene");
	}
}
