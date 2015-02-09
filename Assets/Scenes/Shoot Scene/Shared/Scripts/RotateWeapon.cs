using UnityEngine;
using System.Collections;

public class RotateWeapon : MonoBehaviour {

	[Tooltip("Reference towards the target")]
	[SerializeField] private GameObject target = null;

	private GameObject player = null;
	private float lastAngle = 0;
	public float angle;
	private void Start () {
		if (target == null)
			Debug.LogError (GetType ().Name + " : Field target is empty");

		string playerTag = "Player";
		GameObject[] players = GameObject.FindGameObjectsWithTag (playerTag);
		if (players.Length == 0)
			Debug.LogError (GetType ().Name + " : No player found. The player must have the tag \"" + playerTag + "\".");
		else
			player = players[0];
	}

	private void Update () {
		// We use Vector2 because we do not care about z position (and it would introduce errors to consider it).
		Vector2 lineOfSight = target.transform.position - player.transform.position;
		angle = Vector2.Angle (Vector2.right, lineOfSight);

		// Vector2.Angle() return the acute angle ([0, 180]), so we computed the real angle.
		if (lineOfSight.y < 0)
			angle = 360 - angle;

		transform.RotateAround (transform.parent.position, Vector3.forward, angle - lastAngle);
		lastAngle = angle;

	}
}
