using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Player") {
			
			// turn on parent's rigidbody
			GetComponentInParent<Rigidbody2D>().gravityScale = 1;

			// remove the script
			Destroy( this );
		}
	}
}
