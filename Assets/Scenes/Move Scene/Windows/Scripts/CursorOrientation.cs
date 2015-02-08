using UnityEngine;
using System.Collections;

public class CursorOrientation : MonoBehaviour {

	// Reference towards the player and the world object.
	private GameObject player = null;
	private GameObject arrival = null;
	public RectTransform rec = null;

	// Use this for initialization
	void Start () {

		GetReferenceToPlayer ();
		GetReferenceArrival ();
	
	}
	
	// Update is called once per frame
	void Update () {
		float angle = Vector3.Angle (player.transform.position, arrival.transform.position);
		Debug.Log ("angle1 : " + angle);
		if (arrival.transform.position.y < player.transform.position.y) {
			angle = 360 - angle;
		}
		Debug.Log ("angle2 : " + angle);
		//transform.rotation = Quaternion.Euler (0, 0, angle);
		//GetComponent<RectTransform>().rotation = Quaternion.Euler (0, 0, angle);
		rec.rotation = Quaternion.Euler (0, 0, angle);
	}

	// Get a reference to the player. The player must have the tag "Player". Only works for one player.
	private void GetReferenceToPlayer () {
		string playerTag = "Player";
		player = GameObject.FindGameObjectWithTag (playerTag);
		if (player == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"" + playerTag + "\".");
	}
	
	private void GetReferenceArrival () {
		string finishTag = "Finish";
		arrival = GameObject.FindGameObjectWithTag (finishTag);
		if (arrival == null)
			Debug.LogError (GetType ().Name + " : Cannot find object with tag \"" + finishTag + "\".");
	}
}
