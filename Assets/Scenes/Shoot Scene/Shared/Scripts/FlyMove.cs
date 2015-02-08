using UnityEngine;
using System.Collections;

public class FlyMove : MonoBehaviour {

	private Transform player;
	private int moveSpeed = 3;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Captain DaddyBeard").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (player);
		transform.Translate (transform.forward * moveSpeed * Time.deltaTime);
	}
}
