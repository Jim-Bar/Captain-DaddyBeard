using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {

	[SerializeField] private GameObject game;
	[SerializeField] private GameObject pause;



	public void PauseButtonPressed () {
		Time.timeScale = 0;
		game.gameObject.SetActive (false);
		pause.gameObject.SetActive (true);
		RPCWrapper.RPC ("PauseButtonPressed", RPCMode.Server);
	}

	public void ResumeButtonPressed () {
		Time.timeScale = 1;
		game.gameObject.SetActive (true);
		pause.gameObject.SetActive (false);
		RPCWrapper.RPC ("ResumeButtonPressed", RPCMode.Server);
	}

	public void RestartButtonPressed () {
		Time.timeScale = 1;
		game.gameObject.SetActive (true);
		pause.gameObject.SetActive (false);
		RPCWrapper.RPC ("Restart", RPCMode.Server);
		RPCWrapper.RPC ("ResumeButtonPressed", RPCMode.Server);
	}
}
