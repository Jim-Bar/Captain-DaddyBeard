﻿using UnityEngine;
using System.Collections;

public class MonsterMove : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	void Update () {
		transform.Translate (-3f * Time.deltaTime, 0, 0);
	}



}
