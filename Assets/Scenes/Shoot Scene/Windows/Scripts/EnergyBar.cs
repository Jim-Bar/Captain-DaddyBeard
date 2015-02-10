using UnityEngine;
using System.Collections;

public class EnergyBar : MonoBehaviour {

	private bool isScan;

	void Start () {
		isScan = false;
		RPCWrapper.RegisterMethod (SetBoolEnergyBar);
	}

	void Update () {
		if (Time.timeScale > 0) // If the game is not paused
		{
			// Disable scan if there is no more energy.
			if (Player.energy1.Get () == 0)
			{
				RPCWrapper.RPC ("DisableScan", RPCMode.Others);
				isScan = false;
			}

			if (isScan)
				Player.energy1.Burn (1);
			else
				Player.energy1.Add (1);

			gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Player.energy1.Get () / 5, 30);
		}
	}

	void SetBoolEnergyBar()
	{
		isScan = !isScan;
	}
}
