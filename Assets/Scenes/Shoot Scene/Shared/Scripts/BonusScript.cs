using UnityEngine;
using System.Collections;

public class BonusScript : MonoBehaviour {


	private GameObject buttonSwitch;

	#if UNITY_ANDROID

	// Use this for initialization
	void Start () {
		buttonSwitch = GameObject.Find ("buttonSwitch");
	}

	// Update is called once per frame
	void Update () {
		if (buttonSwitch.GetComponent<SwitchScanShoot>().isShoot) {
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
		} 
		else
		{
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.5f);
		}
	}

	#endif
}
