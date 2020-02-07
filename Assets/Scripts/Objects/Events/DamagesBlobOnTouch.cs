using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damages Blob on touch. Includes a KnockBackOnTouch.
/// </summary>
public class DamagesBlobOnTouch : MonoBehaviour {

	/// <summary>
	/// If this knocks Blob back on damaging him.
	/// </summary>
	[Tooltip("If this knocks Blob back on damaging him.")]
	public bool knocksBack;

	/// <summary>
	/// The force applied when hit.
	/// </summary>
	[Tooltip("The force applied when hit.")]
	public Vector3 knockBackForce;

	/// <summary>
	/// How long blob times out when hit.
	/// </summary>
	[Tooltip("How long blob times out when hit.")]
	public float timeOut;

	// damage blob on contact
	void OnCollisionEnter2D (Collision2D other) {
		damageBlob( other.gameObject );
	}

	// damage blob on entering trigger
	void OnTriggerEnter2D (Collider2D other) {
		damageBlob( other.gameObject );
	}

	/// <summary>
	/// Damages the Blob.
	/// </summary>
	/// <param name="other">Other.</param>
	void damageBlob(GameObject other) {
		Blob blob = other.gameObject.GetComponent<Blob>();

		// damages blob and knocks back (if enabled)
		if (blob) {
			blob.gameObject.GetComponent<TakesDamage>().adjustLife( -1 );

			blob.inputTimeout( timeOut );

			if (knocksBack) {
				knockBack( other );
			}
		}
	}
		
	public void knockBack( GameObject other) {
		Rigidbody2D rigidbody = other.GetComponent<Rigidbody2D>();

		// which x is "away" from the damage ( +/- 1 )
		float direction = Mathf.Sign( ( other.transform.position - transform.position ).x );

		// use direction to generate new velocity
		Vector3 newVelocity = new Vector3(knockBackForce.x * direction, knockBackForce.y, knockBackForce.z);

		// apply new velocity
		rigidbody.velocity = newVelocity;

		Debug.Log("newVelocity: " + newVelocity);
	}
}
