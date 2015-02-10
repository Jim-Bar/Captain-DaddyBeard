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

	private GameObject player = null;

	private void Start () {
		GetReferenceToPlayer ();
	}

	private void Update () {
		SpawnMonster (flyingMonster,Random.Range (2,5),17,1);
		SpawnMonster (jumpingMonster,0,17,2);
		SpawnMonster (slipingMonster,0,17,3);
		SpawnMonster (seekMonster,12,Random.Range (5,12),4);
	}

	private void SpawnMonster (GameObject monsterPrefab, float height, float posX, int numPrefab) {
		float deltaTime = Time.deltaTime;
		if (Random.Range (0, 300 * deltaTime) < deltaTime)
		{
			Vector3 pos = player.transform.position + new Vector3 (posX, height, 0);
			GameObject monster = Instantiate (monsterPrefab, pos, Quaternion.identity) as GameObject;
			monster.transform.parent = gameObject.transform;
			RPCWrapper.RPC("InstanciateGO", RPCMode.Server, numPrefab, pos);
		}
	}

	// Get a reference to the player. The player must have the tag "Player". Only works for one player.
	private void GetReferenceToPlayer () {
		string playerTag = "Player";
		player = GameObject.FindGameObjectWithTag (playerTag);
		if (player == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"" + playerTag + "\".");
	}

	#endif
}
