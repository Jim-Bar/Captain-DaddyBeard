using UnityEngine;
using System.Collections;


public class Timer : MonoBehaviour {

	#pragma strict

	public float timer = 70;
	public UnityEngine.UI.Text displayText;
	private float oldTimer;
	public bool isFinishedLevel = false;

	// Use this for initialization
	void Start () {
		oldTimer = timer;
	}
	
	// Update is called once per frame
	void Update () {

		if (!isFinishedLevel) {
			timer -= Time.deltaTime;
		} 

		if (timer > 0) {
			string minsDisplay = ((int) (timer / 60) ).ToString();
			string secsDisplay = ( (int) timer ).ToString();
			
			if ( (timer - ( int.Parse(minsDisplay) * 60)) > 10 ) {
				secsDisplay = ((int) (timer - ( int.Parse(minsDisplay) * 60)) ).ToString();
			} 
			else {
				secsDisplay = "0" + ((int) (timer - ( int.Parse(minsDisplay) * 60)) ).ToString();
			}
			
			displayText.text = minsDisplay + " : " + secsDisplay;
		} 
		else {
			timer = oldTimer;
		}
	
	}
}
