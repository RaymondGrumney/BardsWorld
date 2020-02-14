using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableDamage : DealsDamage {

	private Rigidbody2D _rigidbody;

	[Tooltip("How fast the object must be travelling to deal damage.")]
	public float damageVelocity = 1f;

	public bool friendlyFire = false;

	void Start() {
		_rigidbody = GetComponentInParent<Rigidbody2D>();
	}

	//protected override void OnCollisionEnter2D(Collision2D other)
	//{
	//	if ( hitCheck( other.gameObject ) ) {
	//		base.OnCollisionEnter2D( other );
	//	}
	//}

	//protected override void OnTriggerEnter2D(Collider2D other)
	//{
	//	if ( hitCheck( other.gameObject ) ) {
	//		base.OnTriggerEnter2D( other );
	//	}
	//}

	private bool hitCheck(GameObject other) {

		if (!friendlyFire) {
			return Mathf.Abs( _rigidbody.velocity.magnitude ) > damageVelocity && other.tag != "Player";
		} else {
			return Mathf.Abs( _rigidbody.velocity.magnitude ) > damageVelocity;
		}
	}
}

