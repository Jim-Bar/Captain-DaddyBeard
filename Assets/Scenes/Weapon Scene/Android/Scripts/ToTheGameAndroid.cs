using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToTheGameAndroid : MonoBehaviour {

	[SerializeField] public Text carac;

	void Start () {
		RPCWrapper.RegisterMethod (ToTheGameAndroidFunc);
		carac.enabled = false;
	}

	public void OnButtonPressedAndroid(){
		RPCWrapper.RPC("OnButtonPressedWin", RPCMode.Server);
		PhaseLoader.Load ();
	}

	public void ToTheGameAndroidFunc(){
		PhaseLoader.Load ();
	}

	public void ShowText(int weapon){
		switch(weapon){
			case 2 :
				carac.text = ("Weapon2 : Damage 2, Energy -2");
				break;
			case 3 :
				carac.text = ("Weapon3 : Damage 3, Energy -3");
				break;
			case 4 :
				carac.text = ("Weapon4 : Damage 4, Energy -4");
				break;
			default :
				carac.text = ("Gun : Damage 1, Energy -1");
				break;
		}
		carac.enabled = true;
	}

	public void HideText(){
		carac.enabled = false;
	}
}
