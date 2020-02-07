using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyComponentsOnImpact : MonoBehaviour {

	public Component[] toDestroy;

	void OnCollisionEnter2D(Collision2D other) {
		for( int i = 0; i < toDestroy.Length; i++ ) {
			Destroy( toDestroy[i] );
		}
	}
}
