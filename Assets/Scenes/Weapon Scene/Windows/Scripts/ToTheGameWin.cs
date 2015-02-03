using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToTheGameWin : MonoBehaviour {

	[SerializeField] Text player1;
	[SerializeField] Text player2;
	[SerializeField] Image weapon11;
	[SerializeField] Image weapon12;
	[SerializeField] Image weapon13;
	[SerializeField] Image weapon21;
	[SerializeField] Image weapon22;
	[SerializeField] Image weapon23;
	[SerializeField] private Sprite nochoice = null;
	[SerializeField] private Sprite weapon1 = null;
	[SerializeField] private Sprite weapon2 = null;
	[SerializeField] private Sprite weapon3 = null;
	[SerializeField] private Sprite weapon4 = null;
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
		weapon11 = weapon11.GetComponent<Image> ();
		weapon12 = weapon12.GetComponent<Image> ();
		weapon13 = weapon13.GetComponent<Image> ();
		weapon21 = weapon21.GetComponent<Image> ();
		weapon22 = weapon22.GetComponent<Image> ();
		weapon23 = weapon23.GetComponent<Image> ();
		if(Network.connections.Length == 1){
			player1.enabled = false;
			player2.enabled = false;
			weapon21.enabled = false;
			weapon22.enabled = false;
			weapon23.enabled = false;
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
		switch(player){
			case 2:
				if(!ready2){
					if(w2 >= 3){
						w2 = 1;
					}
					switch(w2){
						case 2:
							switch(weapon){
								case 1:
									weapon22.sprite = weapon1;
									w22 = 1;
									break;
								case 2:
									weapon22.sprite = weapon2;
									w22 = 2;
									break;
								case 3:
									weapon22.sprite = weapon3;
									w22 = 3;
									break;
								case 4:
									weapon22.sprite = weapon4;
									w22 = 4;
									break;
								default:
									weapon22.sprite = nochoice;
									w22 = 0;
									ready2 = false;
									break;
							}
							break;
						case 3:
							switch(weapon){
								case 1:
									weapon23.sprite = weapon1;
									w23 = 1;
									break;
								case 2:
									weapon23.sprite = weapon2;
									w23 = 2;
									break;
								case 3:
									weapon23.sprite = weapon3;
									w23 = 3;
									break;
								case 4:
									weapon23.sprite = weapon4;
									w23 = 4;
									break;
								default:
									weapon23.sprite = nochoice;
									w23 = 0;
									ready2 = false;
									break;
							}
							break;
						default:
							switch(weapon){
								case 1:
									weapon21.sprite = weapon1;
									w21 = 1;
									break;
								case 2:
									weapon21.sprite = weapon2;
									w21 = 2;
									break;
								case 3:
									weapon21.sprite = weapon3;
									w21 = 3;
									break;
								case 4:
									weapon21.sprite = weapon4;
									w21 = 4;
									break;
								default:
									weapon21.sprite = nochoice;
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
					if(w1 >= 3){
						w1 = 1;
					}
					switch(w1){
						case 2:
							switch(weapon){
								case 1:
									weapon12.sprite = weapon1;
									w12 = 1;
									break;
								case 2:
									weapon12.sprite = weapon2;
									w12 = 2;
									break;
								case 3:
									weapon12.sprite = weapon3;
									w12 = 3;
									break;
								case 4:
									weapon12.sprite = weapon4;
									w12 = 4;
									break;
								default:
									weapon12.sprite = nochoice;
									w12 = 0;
									ready1 = false;
									break;
							}
							break;
						case 3:
							switch(weapon){
								case 1:
									weapon13.sprite = weapon1;
									w13 = 1;
									break;
								case 2:
									weapon13.sprite = weapon2;
									w13 = 2;
									break;
								case 3:
									weapon13.sprite = weapon3;
									w13 = 3;
									break;
								case 4:
									weapon13.sprite = weapon4;
									w13 = 4;
									break;
								default:
									weapon13.sprite = nochoice;
									w13 = 0;
									ready1 = false;
									break;
							}
							break;
						default:
							switch(weapon){
								case 1:
									weapon11.sprite = weapon1;
									w11 = 1;
									break;
								case 2:
									weapon11.sprite = weapon2;
									w11 = 2;
									break;
								case 3:
									weapon11.sprite = weapon3;
									w11 = 3;
									break;
								case 4:
									weapon11.sprite = weapon4;
									w11 = 4;
									break;
								default:
									weapon11.sprite = nochoice;
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
