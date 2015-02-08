using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Move the attached object to the right at speed 'atSpeed'.
 */
public class MoveForward : MonoBehaviour {

	[SerializeField] private float atSpeed = 1;

	#if UNITY_ANDROID

	void Update () {
		transform.Translate (atSpeed * Time.deltaTime, 0, 0);
	}

	#elif UNITY_STANDALONE_WIN

	[SerializeField] private GameObject l2;
	[SerializeField] private GameObject l3;
	[SerializeField] private GameObject l4;
	[SerializeField] private GameObject l5;
	[SerializeField] private GameObject l6;

	void Start () {	
		RPCWrapper.RegisterMethod (EnergyBonus);
		RPCWrapper.RegisterMethod (LifeBonus);
	}

	void Update () {
		transform.Translate (atSpeed * Time.deltaTime, 0, 0);


		switch (Player.health.Get()) {
		case 0:
			Restart();
			break;
		case 1:
			l2.SetActive(false);
			l3.SetActive(false);
			l4.SetActive(false);
			l5.SetActive(false);
			l6.SetActive(false);
			break;
		case 2:
			l2.SetActive(true);
			l3.SetActive(false);
			l4.SetActive(false);
			l5.SetActive(false);
			l6.SetActive(false);
			break;
		case 3:
			l2.SetActive(true);
			l3.SetActive(true);
			l4.SetActive(false);
			l5.SetActive(false);
			l6.SetActive(false);
			break;
		case 4:
			l2.SetActive(true);
			l3.SetActive(true);
			l4.SetActive(true);
			l5.SetActive(false);
			l6.SetActive(false);
			break;
		case 5:
			l2.SetActive(true);
			l3.SetActive(true);
			l4.SetActive(true);
			l5.SetActive(true);
			l6.SetActive(false);
			break;
		default:
			l2.SetActive(true);
			l3.SetActive(true);
			l4.SetActive(true);
			l5.SetActive(true);
			l6.SetActive(true);
			break;			
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("Enemy")) {
			Destroy (other.gameObject);
			Player.health.Subtract (1);
		}
	}

	public void Restart()
	{
		PhaseLoader.ReloadPhase ();
		RPCWrapper.RPC("ReloadCurrentPhase", RPCMode.Others);
		Player.health.Add (6);
		Player.score1.Wipe ();
	}


	public void EnergyBonus(int quantity){
		Player.energy1.Add (quantity);		 
	}

	public void LifeBonus(int quantity){
		Player.health.Add (quantity);
	}

	public void ScoreBonus(int quantity){
		Player.score1.Add (quantity);
	}

	#endif
}
