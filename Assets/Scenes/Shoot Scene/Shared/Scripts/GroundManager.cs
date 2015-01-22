using UnityEngine;
using System.Collections;

public class GroundManager : MonoBehaviour {

	// The ground has been visible once in this cycle.
	private bool onceVisible = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameVisible () {
		onceVisible = true;
	}

	void OnBecameInvisible () {
		if (onceVisible)
		{
			transform.Translate (72, 0, 0, Space.World);
			onceVisible = false;
		}
	}
}
