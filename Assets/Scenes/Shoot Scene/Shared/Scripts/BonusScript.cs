using UnityEngine;
using System.Collections;

public class BonusScript : MonoBehaviour {

	[SerializeField] private Sprite indiceSprite = null;
	[SerializeField] private Sprite bonusSprite = null;

	private GameObject buttonSwitch;

	#if UNITY_ANDROID

	// Use this for initialization
	void Start () {
		buttonSwitch = GameObject.Find ("buttonSwitch");
	}
		
	// Update is called once per frame
	void Update () {
		if (buttonSwitch.GetComponent<SwitchScanShoot>().isShoot) {
			gameObject.GetComponent<SpriteRenderer>().sprite = indiceSprite;
		} 
		else
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = bonusSprite;
		}
	}

	#endif
}
