using UnityEngine;
using System.Collections;


public class FireBulletAndroid : MonoBehaviour {
		
	[Tooltip("Prefab of a bullet")]
	[SerializeField] private GameObject bulletPrefab = null;
	
	[Tooltip("Speed of the bullet")]
	[SerializeField] private float bulletSpeed = 1;
	
	private void Start () {
		RPCWrapper.RegisterMethod (InstanciateShoot);
	}
	
	private void InstanciateShoot (Vector3 targetPosition) {
		GameObject bullet = Instantiate (bulletPrefab, transform.position + 0.1f * Vector3.forward, Quaternion.identity) as GameObject;
		bullet.rigidbody2D.velocity = bulletSpeed * (targetPosition - transform.position).normalized;
		bullet.rigidbody2D.angularVelocity = Random.Range (-360, 360);
	}
}
