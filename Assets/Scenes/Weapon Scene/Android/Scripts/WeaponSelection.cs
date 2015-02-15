using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class WeaponSelection : MonoBehaviour {

	[SerializeField] private int weaponNumber;

	public void SelectWeapon () {
		Player.weapon1.Set (weaponNumber);
		RPCWrapper.RPC ("AddWeapon", RPCMode.Server, Player.id.Get (), weaponNumber);
	}
}
