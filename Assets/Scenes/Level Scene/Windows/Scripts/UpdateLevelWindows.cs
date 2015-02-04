using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateLevelWindows : MonoBehaviour {


	private Image image;
	public int levelChoice;
	[SerializeField] private Sprite nochoice = null;
	[SerializeField] private Sprite level1 = null;
	[SerializeField] private Sprite level2 = null;
	[SerializeField] private Sprite level3 = null;
	[SerializeField] private Sprite level4 = null;
	[SerializeField] private Sprite level5 = null;
	[SerializeField] private Sprite level6 = null;
	// Use this for initialization
	void Start () {
		levelChoice = 0;
		image = gameObject.GetComponent<Image>();
		RPCWrapper.RegisterMethod (UpdateColorWin);
		RPCWrapper.RegisterMethod (LoadWeaponLevel);
	}

	public void UpdateColorWin (int level){
		switch (level) {
			case 1:
				image.sprite = level1;
				levelChoice = 1;
				break;
			case 2:
				image.sprite = level2;
				levelChoice = 2;
				break;
			case 3:
				image.sprite = level3;
				levelChoice = 3;
				break;
			case 4:
				image.sprite = level4;
				levelChoice = 4;
				break;
			case 5:
				image.sprite = level5;
				levelChoice = 5;
				break;
			case 6:
				image.sprite = level6;
				levelChoice = 6;
				break;
			default:
				image.sprite = nochoice;
				levelChoice = 0;
				break;
		}
	}

	public void LoadWeaponLevel(){
		RPCWrapper.RPC ("ValidateLevel", RPCMode.Others, levelChoice);
		PhaseLoader.Prepare (levelChoice);
		Application.LoadLevel ("Windows - WeaponScene");
	}
	
}
