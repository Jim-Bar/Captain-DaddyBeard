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

	void start () {
		RPCWrapper.RegisterMethod (Restart);
		RPCWrapper.RegisterMethod (GoHomeScene);
		RPCWrapper.RegisterMethod (PauseButtonPressed);
		RPCWrapper.RegisterMethod (ResumeButtonPressed);
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
			break;
		case 2:
			l2.SetActive(true);
			l3.SetActive(false);
			break;
		case 3:
			l3.SetActive(true);
			l4.SetActive(false);
			break;
		case 4:
			l4.SetActive(true);
			l5.SetActive(false);
			break;
		case 5:
			l5.SetActive(true);
			l6.SetActive(false);
			break;
		default:
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
		RPCWrapper.RPC ("ReloadCurrentPhase", RPCMode.Others);
		PhaseLoader.ReloadPhase ();
		Player.health.Add (6);
		l2.SetActive (true);
		l3.SetActive (true);
		l4.SetActive (true);
		l5.SetActive (true);
		l6.SetActive (true);
	}

	public void PauseButtonPressed()
	{
		Time.timeScale = 0;
	}

	public void ResumeButtonPressed()
	{
		Time.timeScale = 1;
	}

	public void GoHomeScene()
	{	
		Time.timeScale = 1;
		Application.LoadLevel ("Windows - HomeScene");
	}

	public void EnergyBonus(int quantity){
		Player.energy1.Add (quantity);
	}

	public void LifeBonus(int quantity){
		Player.health.Add (quantity);
	}

	#endif
}
