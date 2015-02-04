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
		SpawnMonster (flyingMonster,Random.Range (-2,3),1);
		SpawnMonster (jumpingMonster,-4,2);
		SpawnMonster (slipingMonster,-4,3);
	}

	private void SpawnMonster (GameObject monsterPrefab, float height, int numPrefab) {
		if (Random.Range (0, 500) < 1)
		{
			Vector3 pos = Camera.main.transform.position + new Vector3 (20, height, 10);
			GameObject monster = Instantiate (monsterPrefab, pos, Quaternion.identity) as GameObject;
			monster.transform.parent = gameObject.transform;	
			RPCWrapper.RPC("UpdateGO", RPCMode.Server, numPrefab);
			RPCWrapper.RPC("InstanciateGO", RPCMode.Server, pos);
		}
	}

	#endif
}
