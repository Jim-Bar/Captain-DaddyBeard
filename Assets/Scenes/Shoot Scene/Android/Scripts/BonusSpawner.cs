using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonusSpawner : MonoBehaviour {
	

	[SerializeField] private GameObject bonus;
		
	void Update () {
		SpawnBonus (bonus);

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
	
	private void SpawnBonus (GameObject bonusPrefab) {
		if (Random.Range (0, 1000) < 1)
		{
			GameObject go = Network.Instantiate (bonusPrefab, Camera.main.transform.position + new Vector3 (20, 0, 9), Quaternion.identity, 0) as GameObject;
			go.transform.parent = gameObject.transform;	
		}
	}

}
