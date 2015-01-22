using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour {

	#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

	[SerializeField] private TargetDetection flyingMonster;
	[SerializeField] private TargetDetection walkingMonster;

	private List<TargetDetection> monsters;

	void Start () {
		monsters = new List<TargetDetection> ();

		RPCWrapper.RegisterMethod (ShootButtonPressed);
	}

	void Update () {
		SpawnMonster (flyingMonster);
		SpawnMonster (walkingMonster);
	}

	private void SpawnMonster (TargetDetection monsterPrefab) {
		if (Random.Range (0, 500) < 1)
		{
			TargetDetection monster = Network.Instantiate (monsterPrefab, Camera.main.transform.position + new Vector3 (10, 3, 10), Quaternion.identity, 0) as TargetDetection;
			monster.transform.parent = gameObject.transform;
			monsters.Add (monster);
			
		}
	}

	private void ShootButtonPressed () {
		for (int i = monsters.Count - 1; i >= 0; i--)
			if (monsters[i].ShootButtonPressed ())
				monsters.RemoveAt (i);
	}

	#endif
}
