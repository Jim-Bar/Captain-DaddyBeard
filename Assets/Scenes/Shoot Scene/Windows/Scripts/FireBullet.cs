using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * Receive RPCs saying hte shoot button has been pressed, and fire bullets accordingly.
 * 
 * Windows only.
 */
public class FireBullet : MonoBehaviour {

	[Tooltip("Reference towards the target")]
	[SerializeField] private GameObject target = null;

	//[Tooltip("Prefab of a bullet")]
	//[SerializeField] private GameObject bulletPrefab = null;
	private GameObject bulletPrefab = null;

	[Tooltip("Speed of the bullet")]
	[SerializeField] private float bulletSpeed = 1;

	private SpriteRenderer image;
	[SerializeField] private Sprite weapon1 = null;
	[SerializeField] private Sprite weapon2 = null;
	[SerializeField] private Sprite weapon3 = null;
	[SerializeField] private GameObject prefab1 = null;
	[SerializeField] private GameObject prefab2 = null;
	[SerializeField] private GameObject prefab3 = null;

	private void Start () {
		image = gameObject.GetComponent<SpriteRenderer>();
		switch(Player.weapon1.Get()){
			case 2:
				transform.localScale = new Vector3(1.25f, 1.25f, 1);
				image.sprite = weapon2;
				bulletPrefab = prefab2;
				break;
			case 3:
				transform.localScale = new Vector3(0.35f, 0.35f, 1);
				image.sprite = weapon3;
				bulletPrefab = prefab3;
				break;
			default:
				image.sprite = weapon1;
				bulletPrefab = prefab1;
				break;
		}
		if (target == null)
			Debug.LogError (GetType ().Name + " : The field 'target' is empty");

		if (bulletPrefab == null)
			Debug.LogError (GetType ().Name + " : The field 'bullet' is empty");

		RPCWrapper.RegisterMethod (ShootPressed);
	}
	
	private void ShootPressed () {
		if (Player.energy1.Get () > 75) {
			Player.energy1.Burn (75);
			GameObject bullet = Instantiate (bulletPrefab, transform.position + 0.1f * Vector3.forward, Quaternion.identity) as GameObject;
			bullet.rigidbody2D.velocity = bulletSpeed * (target.transform.position - transform.position).normalized;
			bullet.transform.Rotate(0, 0, GameObject.Find ("WeaponSlot").GetComponent<RotateWeapon> ().angle);
			if(Player.weapon1.Get() == 1){
				bullet.rigidbody2D.angularVelocity = Random.Range (-360, 360);
			}
			RPCWrapper.RPC ("InstanciateShoot", RPCMode.Others, target.transform.position);
		}

	}
}
