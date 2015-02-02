using UnityEngine;
using System.Collections;

public class ResetWeapons : MonoBehaviour {

	public void ResetPressed(){
		RPCWrapper.RPC ("AddWeapon", RPCMode.Server, Player.id.Get (), 0);
		RPCWrapper.RPC ("AddWeapon", RPCMode.Server, Player.id.Get (), 0);
		RPCWrapper.RPC ("AddWeapon", RPCMode.Server, Player.id.Get (), 0);
	}
}
