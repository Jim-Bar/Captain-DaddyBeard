using UnityEngine;
using System.Collections;

public class ValidateWeapons : MonoBehaviour {

	public void ValidatePressed(){
		RPCWrapper.RPC ("OnValidateWindows", RPCMode.Server);
	}
}
