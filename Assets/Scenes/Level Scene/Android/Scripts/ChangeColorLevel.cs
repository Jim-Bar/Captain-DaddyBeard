using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeColorLevel : MonoBehaviour {

	//[SerializeField] private Sprite validated = null;
	//[SerializeField] private Sprite nonValidated = null;
	[SerializeField] private int levelNumber;
	private Image image;

	// Use this for initialization
	void Start () {
		image = gameObject.GetComponent<Image> ();
		image.color = Color.red;
		//image.sprite = nonValidated;
	}

	// Update is called once per frame
	void Update () {
		if(GameObject.Find ("Button").GetComponent<LevelChosen>().levelChosen == levelNumber){
			image.color = Color.green;
		}
		else{
			image.color = Color.red;
		}
		
	}

	public void OnLevelPressed(){
		if (image.color == Color.red) {
			image.color = Color.green;;
			GameObject.Find ("Button").GetComponent<LevelChosen>().levelChosen = levelNumber;
			RPCWrapper.RPC("UpdateColorWin", RPCMode.Server, levelNumber);
		}
		else{
			image.color = Color.red;
			GameObject.Find ("Button").GetComponent<LevelChosen>().levelChosen = 0;
			RPCWrapper.RPC("UpdateColorWin", RPCMode.Server, 0);
		}
	}

}
