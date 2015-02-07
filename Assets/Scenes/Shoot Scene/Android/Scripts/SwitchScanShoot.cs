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

	// Use this for initialization
	void Start () {
		image = gameObject.GetComponent<Image>();
		isShoot = true;
	}
	
	// Update is called once per frame
	void Update () {

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


}
