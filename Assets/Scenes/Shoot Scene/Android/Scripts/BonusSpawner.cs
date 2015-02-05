using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonusSpawner : MonoBehaviour {
	

	[SerializeField] private GameObject cloud1;
	[SerializeField] private GameObject cloud2;
	[SerializeField] private GameObject cloud3;
	[SerializeField] private GameObject cloud4;

	private int compteur = 0;

	void Update () {
		compteur ++;
		if (compteur >= 800)
		{
			SpawnBonus ();
			compteur = 0;
		}
		

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint((Input.GetTouch (0).position)), Vector2.zero);
			if(hit.collider != null)
			{
				GameObject go = hit.collider.gameObject;
				if(go.tag == "Bonus")
				{
					Network.Destroy(go);
				}
			}
		}
	}
	
	private void SpawnBonus () {

		GameObject go;
		switch (Random.Range (1, 4)) {
				case 1:
					go = Network.Instantiate (cloud1, new Vector3 (Camera.main.transform.position.x + 20, Random.Range(4,8), 3.5f), Quaternion.identity, 0) as GameObject;
					break;
				case 2:
					go = Network.Instantiate (cloud2, new Vector3 (Camera.main.transform.position.x + 20, Random.Range(4,8), 3.5f), Quaternion.identity, 0) as GameObject;
					break;
				case 3:
					go = Network.Instantiate (cloud3, new Vector3 (Camera.main.transform.position.x + 20, Random.Range(4,8), 3.5f), Quaternion.identity, 0) as GameObject;
					break;
				default:
					go = Network.Instantiate (cloud4, new Vector3 (Camera.main.transform.position.x + 20, Random.Range(4,8), 3.5f), Quaternion.identity, 0) as GameObject;
					break;
				}

		if (Random.Range (0, 3) < 1)
			Destroy (go.transform.GetChild (0).gameObject);
		go.transform.parent = gameObject.transform;	

	}

}
