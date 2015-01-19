using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	
	public void ShootButtonPressed () {
		RPCWrapper.RPC ("ShootButtonPressed", RPCMode.Server);
	}
}
