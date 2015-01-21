using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	[SerializeField] private GameObject sphere;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		this.transform.position = new Vector3(sphere.transform.position.x,sphere.transform.position.y,this.transform.position.z);
	
	}
}
