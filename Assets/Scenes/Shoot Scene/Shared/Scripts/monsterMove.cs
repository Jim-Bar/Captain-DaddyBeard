using UnityEngine;
using System.Collections;

public class MonsterMove : MonoBehaviour {

	#if UNITY_STANDALONE_WIN
	void Update () {
		transform.Translate (-0.075f, 0, 0);
	}
	#endif
}
