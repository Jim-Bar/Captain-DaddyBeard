using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonusSpawner : MonoBehaviour {
	

	[SerializeField] private GameObject cloud1;
	[SerializeField] private GameObject cloud2;
	[SerializeField] private GameObject cloud3;
	[SerializeField] private GameObject cloud4;

	private GameObject bonusPrefab;
	private int compteur = 0;

	void Update () {
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
					Player.energy1.Add(200);
				}
			}
		}
	}
	
	private void SpawnBonus () {
			
		switch (Random.Range (1, 4)) {
				case 1:
						bonusPrefab = cloud1;
						break;
				case 2:
						bonusPrefab = cloud2;
						break;
				case 3:
						bonusPrefab = cloud3;
						break;
				default:
						bonusPrefab = cloud4;
						break;
				}

		GameObject go = Network.Instantiate (bonusPrefab, new Vector3 (Camera.main.transform.position.x + 20, Random.Range(4,8), 3.5f), Quaternion.identity, 0) as GameObject;
		if (Random.Range (0, 3) < 1)
			Destroy (go.transform.GetChild (0).gameObject);
		go.transform.parent = gameObject.transform;	

	}

}
