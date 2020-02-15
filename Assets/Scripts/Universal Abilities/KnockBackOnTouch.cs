using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackOnTouch : MonoBehaviour {

	[Tooltip("The length of character timeout aplied by a knockback hit.")]
	public float timeout = 0.5f;

	[Tooltip("The velocity of the knock back of impact (0s means none).")]
	public Vector3 knockBackForce;

	[Tooltip("The sound we play on impact.")]
	public AudioClip impactSound;

	// when something touches the collider
	void OnCollisionEnter2D(Collision2D other) {
		knockBack( other.gameObject );
	}

	// when something enters the trigger
	void OnTriggerEnter2D(Collider2D other) {
		knockBack( other.gameObject );
	}

	public void knockBack( GameObject other) {
		Rigidbody2D rigidbody = other.GetComponent<Rigidbody2D>();

		// knock back object
		if ( rigidbody && knockBackForce != Vector3.zero && other.tag == "Player" ) {

			// which x is "away" from the damage ( +/- 1 )
			float direction = Mathf.Sign( ( other.transform.position - transform.position ).x );

			// use direction to generate new velocity
			Vector3 newVelocity = new Vector3(knockBackForce.x * direction, knockBackForce.y, knockBackForce.z);

			other.BroadcastMessage( "Stun", timeout );

			// apply new velocity
			rigidbody.velocity = newVelocity;

		}
	}
}
