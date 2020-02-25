using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collectible object. 
/// </summary>
public class Collectible : MonoBehaviour {

	[Tooltip("The sound affect on pick up.")]
	public AudioClip onPickupSFX;

	void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.CompareTag("Player"))
		{
			SendMessage("Collect", other.gameObject);

			// play the pick up sound
			Easily.PlaySound(onPickupSFX);

			// turn off things
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<Collider2D>().enabled = false;
			enabled = false;
		}
	}
}
