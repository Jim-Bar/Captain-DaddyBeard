using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Pop monsters on the network.
 * 
 * Windows only.
 */
public class MonsterSpawner : MonoBehaviour {

	#if UNITY_STANDALONE_WIN

	[SerializeField] private GameObject flyingMonster;
	[SerializeField] private GameObject jumpingMonster;
	[SerializeField] private GameObject slipingMonster;
	[SerializeField] private GameObject seekMonster;

	void Update () {
		SpawnMonster (flyingMonster,Random.Range (-2,3),15,1);
		SpawnMonster (jumpingMonster,-4,15,2);
		SpawnMonster (slipingMonster,-4,15,3);
		SpawnMonster (seekMonster,10,Random.Range (5,12),4);
	}

	private void SpawnMonster (GameObject monsterPrefab, float height, float posX, int numPrefab) {
		if (Random.Range (0, 500) < 1)
		{
			Vector3 pos = Camera.main.transform.position + new Vector3 (posX, height, 10);
			GameObject monster = Instantiate (monsterPrefab, pos, Quaternion.identity) as GameObject;
			monster.transform.parent = gameObject.transform;
			RPCWrapper.RPC("InstanciateGO", RPCMode.Server, numPrefab, pos);
		}
	}

	#endif
}
