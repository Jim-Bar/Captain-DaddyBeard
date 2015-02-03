using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeWeaponWindows : MonoBehaviour {
	
	private Image image;
	public int levelChoice;
	[SerializeField] private Sprite nochoice = null;
	[SerializeField] private Sprite weapon1 = null;
	[SerializeField] private Sprite weapon2 = null;
	[SerializeField] private Sprite weapon3 = null;
	[SerializeField] private Sprite weapon4 = null;
	//[SerializeField] private Sprite weapon5 = null;
	//[SerializeField] private Sprite weapon6 = null;
	// Use this for initialization

	void Start () {
		image = gameObject.GetComponent<Image>();
	}

	public void UpdateWeaponWin (int weapon){
		switch (weapon) {
			case 1:
				image.sprite = weapon1;
				break;
			case 2:
				image.sprite = weapon2;
				break;
			case 3:
				image.sprite = weapon3;
				break;
			case 4:
				image.sprite = weapon4;
				break;
			default:
				image.sprite = nochoice;
				break;
		}
	}
}
