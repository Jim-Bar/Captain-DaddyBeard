using UnityEngine;
using System.Collections;

public class JumpMove : MonoBehaviour {

	private int coef;
	// Use this for initialization
	void Start () {
		coef = Random.Range (3, 8);
	}
	
	// Update is called once per frame
	void Update () {
		float posX = transform.position.x;
		transform.position = new Vector3 (-2f * Time.deltaTime + posX, Mathf.Abs (Mathf.Cos (posX)) * coef, 0);
	}
}
