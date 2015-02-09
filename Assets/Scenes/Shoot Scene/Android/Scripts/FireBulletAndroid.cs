using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class FireBulletAndroid : MonoBehaviour {
		
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

	private SoundManager soundManager = null;
	
	private void Start () {
		image = gameObject.GetComponent<SpriteRenderer>();
		switch(Player.weapon1.Get()){
		case 2:
			image.sprite = weapon2;
			bulletPrefab = prefab2;
			break;
		case 3:
			image.sprite = weapon3;
			bulletPrefab = prefab3;
			break;
		default:
			image.sprite = weapon1;
			bulletPrefab = prefab1;
			break;
		}
		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
		if (soundManager == null)
			Debug.LogError (GetType ().Name + " : Sound manager not found.");

		RPCWrapper.RegisterMethod (InstanciateShoot);
	}
	
	private void InstanciateShoot (Vector3 targetPosition) {
		soundManager.SoundShoot();
		GameObject bullet = Instantiate (bulletPrefab, transform.position + 0.1f * Vector3.forward, Quaternion.identity) as GameObject;
		bullet.rigidbody2D.velocity = bulletSpeed * (targetPosition - transform.position).normalized;
		if(Player.weapon1.Get() == 1){
			bullet.rigidbody2D.angularVelocity = Random.Range (-360, 360);
		}
	}
}
