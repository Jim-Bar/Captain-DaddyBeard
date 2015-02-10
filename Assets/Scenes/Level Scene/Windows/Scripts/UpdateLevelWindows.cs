using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateLevelWindows : MonoBehaviour {


	private Image image;
	public int levelChoice;
	[SerializeField] private Text number = null;
	//[SerializeField] private Sprite nochoice = null;
	//[SerializeField] private Sprite level1 = null;
	//[SerializeField] private Sprite level2 = null;
	//[SerializeField] private Sprite level3 = null;
	//[SerializeField] private Sprite level4 = null;
	//[SerializeField] private Sprite level5 = null;
	//[SerializeField] private Sprite level6 = null;
	// Use this for initialization
	void Start () {
		levelChoice = 0;
		number = GameObject.Find ("Number").GetComponent<Text> ();
		image = gameObject.GetComponent<Image>();
		image.color = Color.red;
		RPCWrapper.RegisterMethod (UpdateColorWin);
	}

	public void UpdateColorWin (int level){
		switch (level) {
			case 1:
				image.color = Color.green;
				number.text = level.ToString();
				levelChoice = 1;
				break;
			case 2:
				image.color = Color.green;
				number.text = level.ToString();
				levelChoice = 2;
				break;
			case 3:
				image.color = Color.green;
				number.text = level.ToString();
				levelChoice = 3;
				break;
			case 4:
				image.color = Color.green;
				number.text = level.ToString();
				levelChoice = 4;
				break;
			case 5:
				image.color = Color.green;
				number.text = level.ToString();
				levelChoice = 5;
				break;
			case 6:
				image.color = Color.green;
				number.text = level.ToString();
				levelChoice = 6;
				break;
			default:
				image.color = Color.red;
				number.text = null;
				levelChoice = 0;
				break;
		}
	}
}
