using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToTheGameWin : MonoBehaviour {

	private Image iWeapon21;
	[SerializeField] Text player1;
	[SerializeField] Text player2;
	[SerializeField] private GameObject weapon11;
	[SerializeField] private GameObject weapon21;
	//[SerializeField] private Sprite weapon5 = null;
	//[SerializeField] private Sprite weapon6 = null;
	private int w11, w21, w1, w2;
	private bool ready1, ready2;

	void Start () {
		RPCWrapper.RegisterMethod (OnButtonPressedWin);
		RPCWrapper.RegisterMethod (AddWeapon);
		RPCWrapper.RegisterMethod (OnValidateWindows);
		ready1 = false;
		ready2 = false;
		iWeapon21 = weapon21.GetComponent<Image> ();
		if(Network.connections.Length == 1){
			player1.enabled = false;
			player2.enabled = false;
			iWeapon21.enabled = false;
			ready2 = true;
		}
		w11 = 0;
		w21 = 0;
	}

	public void OnButtonPressedWin(){
		PhaseLoader.Load ();
	}

	public void AddWeapon(int player, int weapon){
		Debug.Log ("the function is called");
		switch(player){
			case 2:
				if(!ready2){
					switch(weapon){
						case 1:
							weapon21.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
							w21 = 1;
							break;
						case 2:
							weapon21.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
							w21 = 2;
							break;
						case 3:
							weapon21.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
							w21 = 3;
							break;
						default:
							weapon21.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (0);
							w21 = 0;
							ready2 = false;
							break;
					}
				}
				break;
			default:
				if(!ready1){
					switch(weapon){
						case 1:
							weapon11.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
							w11 = 1;
							break;
						case 2:
							weapon11.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
							w11 = 2;
							break;
						case 3:
							weapon11.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
							w11 = 3;
							break;
						default:
							weapon11.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (0);
							w11 = 0;
							ready1 = false;
							break;
					}
					break;
				}
				break;
		}
	}

	public void OnValidateWindows(int player){
		if (player == 1){
			if(!ready1){
				if(w11 > 0){
					ready1 = true;
					if(ready2){
						RPCWrapper.RPC ("ToTheGameAndroidFunc", RPCMode.Others);
						PhaseLoader.Load ();
					}
				}
			}
			else{
				ready1 = false;
			}
		}
		else{
			if(!ready2){
				if(w21 > 0){
					ready2 = true;
					if(ready1){
						RPCWrapper.RPC ("ToTheGameAndroidFunc", RPCMode.Others);
						PhaseLoader.Load ();
					}
				}
			}
			else{
				ready2 = false;
			}
		}
	}
}
