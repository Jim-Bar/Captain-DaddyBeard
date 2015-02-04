using UnityEngine;
using System.Collections;

public class MonsterSpawnerAndroid : MonoBehaviour {

	[SerializeField] private GameObject flyingMonster;
	[SerializeField] private GameObject jumpingMonster;
	[SerializeField] private GameObject slipingMonster;
	private GameObject monster;
	private int numPrefab;
	// Use this for initialization
	void Start () {
		RPCWrapper.RegisterMethod (InstanciateGO);
		numPrefab = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateGO(int num)
	{
		numPrefab = num;
	}

	public void InstanciateGO(Vector3 pos){

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
				}

		monster.transform.parent = gameObject.transform;
	}


}
