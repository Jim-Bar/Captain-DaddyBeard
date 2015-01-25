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

		RPCWrapper.RegisterMethod (ShootButtonPressed);
	}
	
	private void ShootButtonPressed () {
		GameObject bullet = Network.Instantiate (bulletPrefab, transform.position, Quaternion.identity, 0) as GameObject;
		bullet.rigidbody2D.velocity = bulletSpeed * (target.transform.position - transform.position).normalized;
		bullet.rigidbody2D.angularVelocity = Random.Range (-120, 120);
	}
}
