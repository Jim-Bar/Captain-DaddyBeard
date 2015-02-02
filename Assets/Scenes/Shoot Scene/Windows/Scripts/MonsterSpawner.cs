using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Pop monsters on the network.
 * 
 * Windows only.
 */
public class MonsterSpawner : MonoBehaviour {

	#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

	[SerializeField] private GameObject flyingMonster;
	[SerializeField] private GameObject jumpingMonster;
	[SerializeField] private GameObject slipingMonster;

	void Update () {
		SpawnMonster (flyingMonster, 3);
		SpawnMonster (jumpingMonster,0);
		SpawnMonster (slipingMonster,0);
	}

	private void SpawnMonster (GameObject monsterPrefab, float height) {
		if (Random.Range (0, 500) < 1)
		{
			GameObject monster = Network.Instantiate (monsterPrefab, Camera.main.transform.position + new Vector3 (20, height, 10), Quaternion.identity, 0) as GameObject;
			monster.transform.parent = gameObject.transform;			
		}
	}

	#endif
}
