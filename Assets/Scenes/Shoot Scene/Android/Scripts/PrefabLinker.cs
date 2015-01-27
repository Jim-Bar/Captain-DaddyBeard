using UnityEngine;
using System.Collections;

/*
 * Link the scene to the prefabs.
 * 
 * The only utility of this script is to prevent Unity from removing the monster prefabs from the
 * final client build (as these prefabs are needed when doing Network.Instantiate() on the server).
 * The prefabs could also be put in a Resources folder.
 * 
 * Android only.
 */
public class PrefabLinker : MonoBehaviour {

	[SerializeField] private GameObject flyingMonster;
	[SerializeField] private GameObject jumpingMonster;
	[SerializeField] private GameObject slipingMonster;
	[SerializeField] private Rigidbody2D bullet;
	
	void Start () {
		if (flyingMonster == null)
			Debug.LogError (GetType ().Name + " : The field 'flyingMonster' is empty");
		if (jumpingMonster == null)
			Debug.LogError (GetType ().Name + " : The field 'walkingMonster' is empty");
		if (slipingMonster == null)
			Debug.LogError (GetType ().Name + " : The field 'slipingMonster' is empty");
		if (bullet == null)
			Debug.LogError (GetType ().Name + " : The field 'bullet' is empty");

		enabled = false;
	}
}
