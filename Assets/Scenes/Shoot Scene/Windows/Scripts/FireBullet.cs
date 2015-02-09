using UnityEngine;
using System.Collections;

/*
 * Receive RPCs saying hte shoot button has been pressed, and fire bullets accordingly.
 * 
 * Windows only.
 */
public class FireBullet : MonoBehaviour {

	[Tooltip("Reference towards the target")]
	[SerializeField] private GameObject target = null;

	[Tooltip("Prefab of a bullet")]
	[SerializeField] private GameObject bulletPrefab = null;

	[Tooltip("Speed of the bullet")]
	[SerializeField] private float bulletSpeed = 1;

	private void Start () {
		if (target == null)
			Debug.LogError (GetType ().Name + " : The field 'target' is empty");

		if (bulletPrefab == null)
			Debug.LogError (GetType ().Name + " : The field 'bullet' is empty");

		RPCWrapper.RegisterMethod (ShootPressed);
	}
	
	private void ShootPressed () {
		if (Player.energy1.Get () > 300) {
			Player.energy1.Burn (300);
			GameObject bullet = Instantiate (bulletPrefab, transform.position + 0.1f * Vector3.forward, Quaternion.identity) as GameObject;
			bullet.rigidbody2D.velocity = bulletSpeed * (target.transform.position - transform.position).normalized;
			bullet.rigidbody2D.angularVelocity = Random.Range (-360, 360);
			RPCWrapper.RPC ("InstanciateShoot", RPCMode.Others, target.transform.position);
		}

	}
}
