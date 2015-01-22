using UnityEngine;
using System.Collections;

public class JumpPlatform : MonoBehaviour 
{

	[SerializeField] private GameObject sphere;

	public void ControllerEnter2D(CircleCollider2D controller)
	{

		sphere.rigidbody2D.AddForce(new Vector2(0,100F), ForceMode2D.Force);
	}
}
