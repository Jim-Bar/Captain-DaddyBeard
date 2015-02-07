using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	private int nbBonusScore = 0;
	public UnityEngine.UI.Text displayText;

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
	
	}

	//incremente le nombre de bonus score
	public void IncBonusScore() {
		nbBonusScore++;
	}
}
