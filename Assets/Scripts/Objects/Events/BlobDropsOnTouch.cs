using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobDropsOnTouch : MonoBehaviour {

	/// <summary>
	/// The collider.
	/// </summary>
//	Collider2D _collider;

	void Awake() {
//		_collider = GetComponent<Collider2D>();
	}

	void OnTriggerEnter2D (Collider2D other) {
		Blob blob = other.gameObject.GetComponent<Blob>();

		if (blob) {

			blob.thrown();

			blob.GetComponent<Rigidbody2D>().velocity = Vector2.down;

		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		Blob blob = other.gameObject.GetComponent<Blob>();

		if (blob) {

			blob.thrown();

			blob.GetComponent<Rigidbody2D>().velocity = Vector2.down;

		}
	}
}
