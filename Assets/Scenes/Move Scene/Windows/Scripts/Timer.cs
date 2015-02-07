using UnityEngine;
using System.Collections;


public class Timer : MonoBehaviour {

	public float timer = 10;
	//point per second
	public int point = 10;
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
			//string minsDisplay = ((int) (timer / 60) ).ToString();
			string secsDisplay = ( (int) timer ).ToString();
			
			//if ( (timer - ( int.Parse(minsDisplay) * 60)) > 10 ) {
				//secsDisplay = ((int) (timer - ( int.Parse(minsDisplay) * 60)) ).ToString();
			//} 
			//else {
				//secsDisplay = "0" + ((int) (timer - ( int.Parse(minsDisplay) * 60)) ).ToString();
			//}
			
			//displayText.text = minsDisplay + " : " + secsDisplay;
			displayText.text = "+ " + (int.Parse(secsDisplay) * point).ToString();
		} 
			
	}
}
