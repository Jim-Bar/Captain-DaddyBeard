using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToTheGameWin : MonoBehaviour {

	private Image iWeapon21;
	private Image iWeapon22;
	private Image iWeapon23;
	[SerializeField] Text player1;
	[SerializeField] Text player2;
	[SerializeField] private GameObject weapon11;
	[SerializeField] private GameObject weapon12;
	[SerializeField] private GameObject weapon13;
	[SerializeField] private GameObject weapon21;
	[SerializeField] private GameObject weapon22;
	[SerializeField] private GameObject weapon23;
	//[SerializeField] private Sprite weapon5 = null;
	//[SerializeField] private Sprite weapon6 = null;
	private int w11, w12, w13, w21, w22, w23, w1, w2;
	private bool ready1, ready2;

	void Start () {
		RPCWrapper.RegisterMethod (OnButtonPressedWin);
		RPCWrapper.RegisterMethod (AddWeapon);
		RPCWrapper.RegisterMethod (OnValidateWindows);
		ready1 = false;
		ready2 = false;
		iWeapon21 = weapon21.GetComponent<Image> ();
		iWeapon22 = weapon22.GetComponent<Image> ();
		iWeapon23 = weapon23.GetComponent<Image> ();
		if(Network.connections.Length == 1){
			player1.enabled = false;
			player2.enabled = false;
			iWeapon21.enabled = false;
			iWeapon22.enabled = false;
			iWeapon23.enabled = false;
			ready2 = true;
		}
		w11 = 0;
		w12 = 0;
		w13 = 0;
		w21 = 0;
		w22 = 0;
		w23 = 0;
		w1 = 1;
		w2 = 1;
	}

	public void OnButtonPressedWin(){
		PhaseLoader.Load ();
	}

	public void AddWeapon(int player, int weapon){
		Debug.Log ("the function is called");
		switch(player){
			case 2:
				if(!ready2){
					if(w2 > 3){
						w2 = 1;
					}
					switch(w2){
						case 2:
							switch(weapon){
								case 1:
									weapon22.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w22 = 1;
									break;
								case 2:
									weapon22.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w22 = 2;
									break;
								case 3:
									weapon22.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w22 = 3;
									break;
								case 4:
									weapon22.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w22 = 4;
									break;
								default:
									weapon22.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (0);
									w22 = 0;
									ready2 = false;
									break;
							}
							break;
						case 3:
							switch(weapon){
								case 1:
									weapon23.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w23 = 1;
									break;
								case 2:
									weapon23.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w23 = 2;
									break;
								case 3:
									weapon23.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w23 = 3;
									break;
								case 4:
									weapon23.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w23 = 4;
									break;
								default:
									weapon23.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (0);
									w23 = 0;
									ready2 = false;
									break;
							}
							break;
						default:
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
								case 4:
									weapon21.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w21 = 4;
									break;
								default:
									weapon21.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (0);
									w21 = 0;
									ready2 = false;
									break;
							}
							break;
					}
					w2++;
				}
				break;
			default:
				if(!ready1){
					if(w1 > 3){
						w1 = 1;
					}
					switch(w1){
						case 2:
							switch(weapon){
								case 1:
									weapon12.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w12 = 1;
									break;
								case 2:
									weapon12.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w12 = 2;
									break;
								case 3:
									weapon12.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w12 = 3;
									break;
								case 4:
									weapon12.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w12 = 4;
									break;
								default:
									weapon12.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (0);
									w12 = 0;
									ready1 = false;
									break;
							}
							break;
						case 3:
							switch(weapon){
								case 1:
									weapon13.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w13 = 1;
									break;
								case 2:
									weapon13.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w13 = 2;
									break;
								case 3:
									weapon13.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w13 = 3;
									break;
								case 4:
									weapon13.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w13 = 4;
									break;
								default:
									weapon13.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (0);
									w13 = 0;
									ready1 = false;
									break;
							}
							break;
						default:
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
								case 4:
									weapon11.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (weapon);
									w11 = 4;
									break;
								default:
									weapon11.GetComponent<ChangeWeaponWindows>().UpdateWeaponWin (0);
									w11 = 0;
									ready1 = false;
									break;
							}
							break;
					}
					w1++;
				}
				break;
		}
	}

	public void OnValidateWindows(int player){
		if (player == 1){
			if(!ready1){
				if(w11 > 0 && w12 > 0 && w13 > 0){
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
				if(w21 > 0 && w22 > 0 && w23 > 0){
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
