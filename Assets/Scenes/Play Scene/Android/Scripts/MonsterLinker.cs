using UnityEngine;
using System.Collections;

/*
 * Link the scene to the monster prefabs.
 * 
 * The only utility of this script is to prevent Unity from removing the monster prefabs from the
 * final client build (as these prefabs are needed when doing Network.Instantiate() on the server).
 * The prefabs could also be put in a Resources folder.
 */
public class MonsterLinker : MonoBehaviour {

	[SerializeField] private Rigidbody2D flyingMonster;
	[SerializeField] private Rigidbody2D walkingMonster;
	
	void Start () {
		enabled = false;
	}
}
