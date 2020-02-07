using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpellBlast : MonoBehaviour {

	// unstick blob on touch
	void OnTriggerEnter2D(Collider2D other) {
		

		// Check if Blob
		Blob blob = other.GetComponent<Blob>();

		if (blob) {
			// unstick Blob
			blob.thrown();
		}
	}
}
