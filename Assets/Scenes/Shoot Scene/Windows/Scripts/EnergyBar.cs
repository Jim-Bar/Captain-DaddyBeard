using UnityEngine;
using System.Collections;

public class EnergyBar : MonoBehaviour {

	private bool isScan;
	// Use this for initialization
	void Start () {
		isScan = false;
		RPCWrapper.RegisterMethod (SetBoolEnergyBar);
	}
	
	// Update is called once per frame
	void Update () {
		if (isScan)
		{
			Player.energy1.Burn (1);
		} 
		else
		{
			Player.energy1.Add (1);
		}

		gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Player.energy1.Get () / 5, 30);


	}

	void SetBoolEnergyBar()
	{
		isScan = !isScan;
	}
}
