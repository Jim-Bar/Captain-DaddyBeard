using UnityEngine;
using System.Collections;

public class JumpMove : MonoBehaviour {

	private int coef;
	private GameObject player = null;

	void Start () {
		GetReferenceToPlayer ();
		coef = Random.Range (1, 5);
	}
	
	// Update is called once per frame
	void Update () {
		float posX = transform.position.x;
		transform.position = new Vector3 (-2f * Time.deltaTime + posX, player.transform.position.y + Mathf.Abs (Mathf.Cos (posX)) * coef, 0);
	}

	// Get a reference to the player. The player must have the tag "Player". Only works for one player.
	private void GetReferenceToPlayer () {
		string playerTag = "Player";
		player = GameObject.FindGameObjectWithTag (playerTag);
		if (player == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"" + playerTag + "\".");
	}
}
