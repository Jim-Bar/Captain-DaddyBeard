using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeColorLevel : MonoBehaviour {

	[SerializeField] private Sprite validated = null;
	[SerializeField] private Sprite nonValidated = null;
	[SerializeField] private int levelNumber;
	private Image image;

	// Use this for initialization
	void Start () {
		image = gameObject.GetComponent<Image> ();
		image.sprite = nonValidated;
	}

	// Update is called once per frame
	void Update () {
		if(GameObject.Find ("Button").GetComponent<LevelChosen>().levelChosen == levelNumber){
			image.sprite = validated;
		}
		else{
			image.sprite = nonValidated;
		}
		
	}

	public void OnLevelPressed(){
		if (image.sprite == nonValidated) {
			image.sprite = validated;
			GameObject.Find ("Button").GetComponent<LevelChosen>().levelChosen = levelNumber;
			RPCWrapper.RPC("UpdateColorWin", RPCMode.Server, levelNumber);
		}
		else{
			image.sprite = nonValidated;
			GameObject.Find ("Button").GetComponent<LevelChosen>().levelChosen = 0;
			RPCWrapper.RPC("UpdateColorWin", RPCMode.Server, 0);
		}
	}

}
