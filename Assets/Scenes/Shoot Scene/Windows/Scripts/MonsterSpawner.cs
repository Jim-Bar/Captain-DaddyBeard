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
	[SerializeField] private GameObject walkingMonster;

	void Update () {
		SpawnMonster (flyingMonster);
		SpawnMonster (walkingMonster);
	}

	private void SpawnMonster (GameObject monsterPrefab) {
		if (Random.Range (0, 500) < 1)
		{
			GameObject monster = Network.Instantiate (monsterPrefab, Camera.main.transform.position + new Vector3 (10, 3, 10), Quaternion.identity, 0) as GameObject;
			monster.transform.parent = gameObject.transform;			
		}
	}

	#endif
}
