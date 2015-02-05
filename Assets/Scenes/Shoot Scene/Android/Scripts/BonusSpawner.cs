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
			GameObject go = Network.Instantiate (bonusPrefab, new Vector3 (Camera.main.transform.position.x + 20, Random.Range(4,8), /*Camera.main.transform.position.z + 9*/2.5f), Quaternion.identity, 0) as GameObject;
			go.transform.parent = gameObject.transform;	
		}
	}

}
