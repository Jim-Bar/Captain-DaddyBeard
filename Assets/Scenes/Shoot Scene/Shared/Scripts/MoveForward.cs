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

	void Update () {
		transform.Translate (atSpeed * Time.deltaTime, 0, 0);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("Enemy")) {
			Destroy (other.gameObject);
			Player.health.Subtract (1);

			switch (Player.health.Get()) {
			case 0:
				RPCWrapper.RPC ("ReloadCurrentPhase", RPCMode.Others);
				PhaseLoader.ReloadPhase();
				Player.health.Add(6);
				l2.SetActive(true);
				l3.SetActive(true);
				l4.SetActive(true);
				l5.SetActive(true);
				l6.SetActive(true);
				break;
			case 1:
				l2.SetActive(false);
				break;
			case 2:
				l3.SetActive(false);
				break;
			case 3:
				l4.SetActive(false);
				break;
			case 4:
				l5.SetActive(false);
				break;
			default:
				l6.SetActive(false);
				break;			
			}
		}
	}

	#endif
}
