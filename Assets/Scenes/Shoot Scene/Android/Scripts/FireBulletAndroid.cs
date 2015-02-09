using UnityEngine;
using System.Collections;


public class FireBulletAndroid : MonoBehaviour {
		
	[Tooltip("Prefab of a bullet")]
	[SerializeField] private GameObject bulletPrefab = null;
	
	[Tooltip("Speed of the bullet")]
	[SerializeField] private float bulletSpeed = 1;

	private SoundManager soundManager = null;
	
	private void Start () {
		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
		if (soundManager == null)
			Debug.LogError (GetType ().Name + " : Sound manager not found.");

		RPCWrapper.RegisterMethod (InstanciateShoot);
	}
	
	private void InstanciateShoot (Vector3 targetPosition) {
		soundManager.SoundShoot();
		GameObject bullet = Instantiate (bulletPrefab, transform.position + 0.1f * Vector3.forward, Quaternion.identity) as GameObject;
		bullet.rigidbody2D.velocity = bulletSpeed * (targetPosition - transform.position).normalized;
		bullet.rigidbody2D.angularVelocity = Random.Range (-360, 360);
	}
}
