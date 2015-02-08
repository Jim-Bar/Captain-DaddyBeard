using UnityEngine;
using System.Collections;

public class BonusScore : MonoBehaviour {

	// Reference towards the player and the world object.
	private GameObject player = null;
	
	// Use this for initialization
	void Start () {
		GetReferenceToPlayer ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Get a reference to the player. The player must have the tag "Player". Only works for one player.
	private void GetReferenceToPlayer () {
		string playerTag = "Player";
		player = GameObject.FindGameObjectWithTag (playerTag);
		if (player == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"" + playerTag + "\".");
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			Inventory inv = player.GetComponent<Inventory>();
			inv.IncBonusScore();
			//Player.score1.Add(scoreValue);
			Destroy(gameObject);
		}

	}
}
