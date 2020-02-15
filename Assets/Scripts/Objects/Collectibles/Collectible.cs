using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collectible object. 
/// </summary>
public abstract class Collectible : MonoBehaviour {

	[Tooltip("The sound affect on pick up.")]
	/// <summary>
	/// The sound affect on pick up.
	/// </summary>
	public AudioClip onPickupSFX;

	void OnTriggerEnter2D(Collider2D other) {
		
		// call the child object's Collect script
		collect( other.gameObject );

		// play the pick up sound
		Easily.PlaySound(onPickupSFX);

		// turn off things
		this.GetComponent<SpriteRenderer>().enabled = false;
		this.GetComponent<Collider2D>().enabled = false;
		this.enabled = false;
	}

	/// <summary>
	/// The effect of picking up this object
	/// </summary>
	/// <param name="other">The object picking up this item.</param>
	protected abstract void collect( GameObject other );
}
