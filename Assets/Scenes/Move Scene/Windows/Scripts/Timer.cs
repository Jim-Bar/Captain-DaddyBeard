using UnityEngine;
using System.Collections;


public class Timer : MonoBehaviour {

	public float timer = 10;
	//point per second
	public int point = 10;
	private int score = 0;
	public UnityEngine.UI.Text displayText;
	public UnityEngine.UI.Text ScoredisplayText;
	public bool isFinishedLevel = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("score : "+Player.score1.Get ());
		ScoredisplayText.text = Player.score1.Get().ToString();
		if (!isFinishedLevel) {
			timer -= Time.deltaTime;
		} 

		if (timer > 0) {

			string secsDisplay = ( (int) timer ).ToString();
	
			score = int.Parse(secsDisplay) * point;
			displayText.text = "+ " + score.ToString();
		} 
			
	}

	public int getScore() {
		return score;
	}
}
