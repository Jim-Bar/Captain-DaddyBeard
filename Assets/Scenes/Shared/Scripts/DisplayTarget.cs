using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayTarget : MonoBehaviour {

	private SpriteRenderer imageComponent;

	private void Start () {
		// Get the image component.
		imageComponent = gameObject.GetComponent<SpriteRenderer>();

		SetTargetActive (false);

		// Register in the RPC wrapper.
		RPCWrapper.RegisterMethod (SetTargetActive);

		DontDestroyOnLoad (gameObject);
	}

	public void SetTargetActive (bool active) {
		imageComponent.enabled = active;
	}
}
