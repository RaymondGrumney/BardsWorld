using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	/// <summary>
	/// The attached rigidbody.
	/// </summary>
	private Rigidbody2D _rigidbody;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	// while touching the trigger
	void OnTriggerStay2D(Collider2D other) {
		
		// move moveable objects by this object's velocity.x
		Moveable moveable = other.GetComponent<Moveable>();

		if ( moveable ) {
			moveable.moveVector = new Vector2( _rigidbody.velocity.x, 0f);
		}
	}

	// while colliding
	void OnCollisionStay2D( Collision2D other) {
		
//		Moveable moveable = other.gameObject.GetComponent<Moveable>();
//
//		if ( moveable ) {
//			moveable.moveVector = new Vector2( _rigidbody.velocity.x, 0f);
//		}
//
//		Character character = other.gameObject.GetComponent<Character>();
//
//		if (character &&character.grounded) {
//			character.moveVector = new Vector2( _rigidbody.velocity.x, 0f);
//		}
	}

	// on leaving contact with the collider
	void OnCollisionExit2D( Collision2D other ) {

//		Moveable moveable = other.gameObject.GetComponent<Moveable>();
//
//		if (moveable) {
//			moveable.moveVector = Vector2.zero;
//		}
	}

	// upon entering the trigger
	void OnTriggerEnter2D(Collider2D other) {
//		Moveable moveable = other.GetComponent<Moveable>();

//		if (moveable) {
//			moveable.setParent( this.transform );
//		}
	}		

	// upon leaving the trigger
	void OnTriggerExit2D(Collider2D other) {
//		Moveable moveable = other.GetComponent<Moveable>();
//		if (moveable) {
//			moveable.moveVector = Vector2.zero;
//		}
	}
}
