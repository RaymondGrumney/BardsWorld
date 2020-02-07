using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealsDamage : MonoBehaviour {


	[Tooltip("The damage this deals.")]
	public int damage = 1;

	[Tooltip("The sound we play on impact.")]
	public AudioClip impactSound;

	[Tooltip("The object we spawn on impact.")]
	public GameObject spawnOnImpact;

	[Tooltip("Whether we destroy this object on impact.")]
	public bool destroyOnImpact;


	// the two following method handle both the cases that the damage dealing object uses either:
	//	1. a trigger (which you can pass through) 
	//	2. a collider (which you cannot)
	// they are otherwise functionally identical

	// when something touches the collider
	protected virtual void OnCollisionEnter2D(Collision2D other) {
		hit( other.gameObject );
	}

	// when something enters the trigger
	protected virtual void OnTriggerEnter2D(Collider2D other) {
		hit( other.gameObject );
	}


	/// <summary>
	/// Hit the other object
	/// </summary>
	/// <param name="other">The Object we're hitting.</param>
	void hit(GameObject other) {

		// deal damage to other
		dealDamage( other );


		// play hitSound
		if (impactSound != null) {
			AudioSource.PlayClipAtPoint( impactSound, transform.position );
		}


		// spawn SpawnOnImpact object
		if (spawnOnImpact != null) {
			Instantiate( spawnOnImpact, transform.GetComponent<Collider2D>().bounds.center, Quaternion.identity );
			//spawnOnImpact = null;
		}


		// destroy if destroyOnImpact
		if (destroyOnImpact) {
			Destroy( this.gameObject );
		}
	}

	/// <summary>
	/// Deals damage to other
	/// </summary>
	/// <param name="other">Other.</param>
	void dealDamage(GameObject other) {

		TakesDamage receiver = other.gameObject.GetComponent<TakesDamage>();

		if (receiver != null) {

			// if the receiver is currently taking damage
			if ( receiver.isTakingDamage() ) {

				receiver.adjustLife( - damage );
			}
		}
	}
}

