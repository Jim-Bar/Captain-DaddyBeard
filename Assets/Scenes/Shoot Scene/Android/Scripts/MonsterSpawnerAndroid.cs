using UnityEngine;
using System.Collections;

public class MonsterSpawnerAndroid : MonoBehaviour {

	[SerializeField] private GameObject flyingMonster;
	[SerializeField] private GameObject jumpingMonster;
	[SerializeField] private GameObject slipingMonster;
	[SerializeField] private GameObject seekMonster;
	private GameObject monster;

	void Start () {
		RPCWrapper.RegisterMethod (InstanciateGO);
	}

	public void InstanciateGO(int numPrefab, Vector3 pos){

		switch (numPrefab) {
				case 1:
						monster = Instantiate (flyingMonster, pos, Quaternion.identity) as GameObject;
						break;
				case 2:
						monster = Instantiate (jumpingMonster, pos, Quaternion.identity) as GameObject;
						break;
				case 3:
						monster = Instantiate (slipingMonster, pos, Quaternion.identity) as GameObject;
						break;
				default:
						monster = Instantiate (seekMonster, pos, Quaternion.identity) as GameObject;
						break;
				}

		monster.transform.parent = gameObject.transform;
	}


}
