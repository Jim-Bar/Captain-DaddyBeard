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
				if(go.tag == "LifeBonus")
				{
					RPCWrapper.RPC("LifeBonus", RPCMode.Server, 1);
					Network.Destroy(go);
				}
				else if(go.tag == "EnergyBonus")
				{
					RPCWrapper.RPC("EnergyBonus", RPCMode.Server, 200);
					Network.Destroy(go);
				}
			}
		}
	}
	
	private void SpawnBonus () {

		GameObject go;
		switch (Random.Range (1, 4)) {
				case 1:
					go = cloud1;
					break;
				case 2:
					go = cloud2;
					break;
				case 3:
					go = cloud3;
					break;
				default:
					go = cloud4;
					break;
				}

		go = Network.Instantiate (go, new Vector3 (Camera.main.transform.position.x + 20, Random.Range(4,8), 3.5f), Quaternion.identity, 0) as GameObject;
		if (Random.Range (0, 3) < 1)
			Destroy (go.transform.GetChild (0).gameObject);
		go.transform.parent = gameObject.transform;	

	}

}
