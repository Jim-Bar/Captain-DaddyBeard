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
	[SerializeField] private GameObject lifeBar;

	[SerializeField] private Sprite oneCore;
	[SerializeField] private Sprite twoCore;
	[SerializeField] private Sprite threeCore;

	private Image image;


	void Update () {
		transform.Translate (atSpeed * Time.deltaTime, 0, 0);
		image = lifeBar.GetComponent<Image> ();
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
				Player.health.Add(3);
				break;
			case 1:
				image.sprite = oneCore;
				break;
			case 2:
				image.sprite = twoCore;
				break;
			default:
				image.sprite = threeCore;
				break;
			}
		}
	}

	#endif
}
