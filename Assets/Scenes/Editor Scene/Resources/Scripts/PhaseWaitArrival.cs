using UnityEngine;
using System.Collections;

/*
 * This script is used when a phase is finished : the 'PhaseArrival' script will then
 * call an RPC below to tell Android to load the next scene.
 * 
 * Both Windows and Android (although it does nothing on Windows).
 * Use this script only for the prefabs of phases.
 */
public class PhaseWaitArrival : MonoBehaviour {
	
	#if UNITY_ANDROID
	
	private void Start () {
		// Server tells clients when to load next phase.
		RPCWrapper.RegisterMethod (LoadNextPhase);
		RPCWrapper.RegisterMethod (LoadScoreScene);
	}
	
	private void LoadNextPhase (int nextPhaseType, int nextLevel, int nextPhase) {
		PhaseLoader.Load ((PhaseLoader.Type) nextPhaseType, nextLevel, nextPhase);
	}
	
	private void LoadScoreScene () {
		Application.LoadLevel ("Android - ScoreScene");
	}
	
	#endif
}
