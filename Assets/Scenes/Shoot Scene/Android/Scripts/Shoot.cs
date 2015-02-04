using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	
	public void ShootButtonPressed () {
		RPCWrapper.RPC ("ShootPressed", RPCMode.Server);
		GameObject.Find ("SoundManager").GetComponent<SoundManager> ().SoundShoot();
	}
}
