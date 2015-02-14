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
	private Camera mainCamera = null;

	private void Start () {
		GetReferenceToPlayer ();
		mainCamera = Camera.main;
	}

	private void Update () {
		Vector3 cameraPos = Camera.main.transform.position;

		SpawnMonster (flyingMonster, cameraPos.x + mainCamera.orthographicSize * mainCamera.aspect + 2, cameraPos.y + Random.Range (0, mainCamera.orthographicSize), 1);
		SpawnMonster (jumpingMonster, cameraPos.x + mainCamera.orthographicSize * mainCamera.aspect + 2, player.transform.position.y, 2);
		SpawnMonster (slipingMonster, cameraPos.x + mainCamera.orthographicSize * mainCamera.aspect + 2, player.transform.position.y, 3);
		SpawnMonster (seekMonster, cameraPos.x + Random.Range (mainCamera.orthographicSize * mainCamera.aspect, mainCamera.orthographicSize * mainCamera.aspect / 2), cameraPos.y + 2 * mainCamera.orthographicSize, 4);
	}

	private void SpawnMonster (GameObject monsterPrefab, float posX, float posY, int numPrefab) {
		float deltaTime = Time.deltaTime;
		if (Random.Range (0, 300 * deltaTime) < deltaTime)
		{
			Vector3 pos = new Vector3 (posX, posY, player.transform.position.z);
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
