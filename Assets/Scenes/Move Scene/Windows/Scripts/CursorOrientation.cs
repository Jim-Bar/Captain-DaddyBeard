using UnityEngine;
using System.Collections;

public class CursorOrientation : MonoBehaviour {
	
	// Reference towards the player and the world object.
	private GameObject player = null;
	private GameObject arrival = null;
	private float angle = 0;
	public UnityEngine.UI.Text displayText;
	
	// Use this for initialization
	void Start () {
		GetReferenceToPlayer ();
		GetReferenceArrival ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 direction = arrival.transform.position - player.transform.position;
		angle = Vector2.Angle (Vector2.right, direction);
		
		if (direction.y < 0)
			angle = 360 - angle;
		
		transform.rotation = Quaternion.Euler (0, 0, angle);

		float dist = Vector3.Distance(arrival.transform.position, player.transform.position);
		displayText.text = ((int) dist).ToString() + " m";

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
