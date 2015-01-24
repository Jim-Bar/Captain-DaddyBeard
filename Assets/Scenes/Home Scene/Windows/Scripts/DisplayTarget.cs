using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayTarget : MonoBehaviour {

	private SpriteRenderer imageComponent;

	private void Start () {
		// Get the image component.
		imageComponent = gameObject.GetComponent<SpriteRenderer>();

		// Register in the RPC wrapper.
		RPCWrapper.RegisterMethod (SetTargetActive);

		// In the doubt. Note that the aiming manager will call this method as soon as it begins.
		SetTargetActive (false);
	}

	public void SetTargetActive (bool active) {
		imageComponent.enabled = active;
	}
}
