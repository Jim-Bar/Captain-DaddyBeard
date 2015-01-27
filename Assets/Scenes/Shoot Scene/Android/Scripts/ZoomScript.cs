using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZoomScript : MonoBehaviour {

	public Slider zoomSlider;

	void Start () {
		RPCWrapper.RegisterMethod (UpdateCamera);
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.orthographicSize = 10 - zoomSlider.value;
	}

	public void UpdateCamera(Vector3 pos){
		transform.position = new Vector3 (pos.x, pos.y, transform.position.z);
	}
}
