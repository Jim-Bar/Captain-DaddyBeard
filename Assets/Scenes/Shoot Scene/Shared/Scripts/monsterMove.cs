using UnityEngine;
using System.Collections;

public class MonsterMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		transform.Translate (-0.075f, 0, 0);
	}
}
