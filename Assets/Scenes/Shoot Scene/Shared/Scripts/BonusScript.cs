using UnityEngine;
using System.Collections;

public class BonusScript : MonoBehaviour {

	#if UNITY_STANDALONE_WIN

	void Start () {
		gameObject.renderer.enabled = false;
	}

	#elif UNITY_ANDROID

	private GameObject buttonSwitch;

	// Use this for initialization
	void Start () {
		buttonSwitch = GameObject.Find ("buttonSwitch");
	}
		
	// Update is called once per frame
	void Update () {
		if (buttonSwitch.GetComponent<SwitchScanShoot>().isShoot) {
			gameObject.renderer.enabled = false;
		} 
		else
		{
			gameObject.renderer.enabled = true;
		}
	}

	#endif
}
