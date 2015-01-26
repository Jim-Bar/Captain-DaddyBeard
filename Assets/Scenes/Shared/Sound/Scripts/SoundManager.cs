using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {


	[SerializeField] private GameObject jump;
	[SerializeField] private GameObject validate;
	[SerializeField] private GameObject theme;
	[SerializeField] private GameObject shoot;
	[SerializeField] private GameObject explode;
	
	// Use this for initialization
	void Start () {
		jump.audio.Stop ();
		validate.audio.Stop ();
		theme.audio.Stop ();
		shoot.audio.Stop ();
		explode.audio.Stop ();
		theme.audio.Play ();
	}

	public void SoundJump(){
		jump.audio.Play ();
	}

	public void SoundValidate(){
		validate.audio.Play ();
	}

	public void SoundShoot(){
		shoot.audio.Play ();
	}

	public void SoundExplode(){
		explode.audio.Play ();
	}
}
