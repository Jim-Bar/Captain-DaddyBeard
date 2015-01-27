using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

	[SerializeField] private Sprite oneCore;
	[SerializeField] private Sprite twoCore;
	[SerializeField] private Sprite threeCore;
	private Image image;
	// Use this for initialization
	void Start () {
		image = gameObject.GetComponent<Image> ();
		RPCWrapper.RegisterMethod (UpdateLifeBar);
	}
	


	public void UpdateLifeBar (){
		switch (Player.health.Get()) {
		case 1:
			image.sprite = oneCore;
			break;
		case 2:
			image.sprite = twoCore;
			break;
		default:
			image.sprite = threeCore;
			break;
		}
	}
}
