using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGrowingLadderOnContact : MonoBehaviour {

	// when the trigger leaves contact with another collider
	void OnTriggerExit2D(Collider2D other) {

		// check if moveable or carriable
		Moveable moveable = other.gameObject.GetComponent<Moveable>();
		Attributes attributes = other.gameObject.GetComponent<Attributes>();

		if (moveable) {
		} else {
			if (attributes && !attributes.carriable) {
				// find and destroy GrowingLadder script and this object
				Destroy( GetComponentInParent<GrowingLadder>() );
				Destroy( this.gameObject );
			}
		}
	}
}
