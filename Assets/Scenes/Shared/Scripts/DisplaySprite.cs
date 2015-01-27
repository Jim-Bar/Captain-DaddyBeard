using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * Display or not a sprite. By default does not display, but this can be changed using 'SetActive ()'.
 */
public class DisplaySprite : MonoBehaviour {

	private SpriteRenderer imageComponent;

	private void Start () {
		// Get the image component.
		imageComponent = gameObject.GetComponent<SpriteRenderer>();

		// Register in the RPC wrapper.
		RPCWrapper.RegisterMethod (SetVisible);

		// In the doubt. Note that for the home scene, the aiming manager will call this method as soon as it begins.
		SetVisible (false);
	}

	public void SetVisible (bool active) {
		imageComponent.enabled = active;
	}
}
