﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplay : MonoBehaviour {

	[SerializeField] Text totalScore;
	[SerializeField] Text score1;
	[SerializeField] Text score2;
	// Use this for initialization
	void Start () {
		if(Network.connections.Length == 1){
			int iScore1 = Player.score1.Get();
			totalScore.text = iScore1.ToString();

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
}