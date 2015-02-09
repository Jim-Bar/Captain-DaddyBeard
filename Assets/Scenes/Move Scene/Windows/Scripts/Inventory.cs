using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	private int nbBonusScore = 0;
	private int nbBonusLife = 0;
	public UnityEngine.UI.Text displayText;
	public UnityEngine.UI.Text lifeText;
	public int scoreValue = 150;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (nbBonusScore >= 10) {
			displayText.text = "X " + nbBonusScore.ToString();

		} 
		else {
			displayText.text = "X 0" + nbBonusScore.ToString();
		}
		if (nbBonusLife >= 10) {
			lifeText.text = "X " + nbBonusLife.ToString();
			
		} 
		else {
			lifeText.text = "X 0" + nbBonusLife.ToString();
		}
	
	}

	public int getScoreBonus() {
		return nbBonusScore * scoreValue;
	}

	public int getLifeBonus() {
		return nbBonusLife;
	}

	//incremente le nombre de bonus score
	public void IncBonusScore() {
		nbBonusScore++;
	}

	//incremente le nombre de bonus score
	public void IncBonusLife() {
		nbBonusLife++;
	}
}
