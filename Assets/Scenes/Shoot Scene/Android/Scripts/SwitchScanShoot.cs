using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchScanShoot : MonoBehaviour {

	public bool isShoot;
	private Image image;
	[SerializeField] private Sprite scan = null;
	[SerializeField] private Sprite shoot = null;
	[SerializeField] private GameObject bFire = null;
	[SerializeField] private GameObject filter = null;

	void Start () {
		image = gameObject.GetComponent<Image>();
		isShoot = true;

		// Register so that when there is no more energy, the server can call this function to disable the scan.
		RPCWrapper.RegisterMethod (DisableScan);
	}

	public void SwitchButtonPressed() {
		isShoot = !isShoot;
		RPCWrapper.RPC("SetBoolEnergyBar", RPCMode.Server);
		if (isShoot)
		{
			image.sprite = scan;
			bFire.SetActive(true);
			filter.SetActive(false);
		} 
		else
		{
			image.sprite = shoot;
			bFire.SetActive(false);
			filter.SetActive(true);
		}
	}

	private void DisableScan () {
		isShoot = true;
		image.sprite = scan;
		bFire.SetActive(true);
		filter.SetActive(false);
	}
}
