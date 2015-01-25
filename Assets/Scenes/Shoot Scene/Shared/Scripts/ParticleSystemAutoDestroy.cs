using UnityEngine;
using System.Collections;

/*
 * Destroy a particle emitter when it has ended.
 * 
 * Windows and Android.
 */
public class ParticleSystemAutoDestroy : MonoBehaviour {
	
	private ParticleSystem ps;
	
	private void Start() {
		ps = GetComponent<ParticleSystem>();
	}
	
	private void Update() {
		if (ps)
			if (!ps.IsAlive())
				Destroy(gameObject);
	}
}
