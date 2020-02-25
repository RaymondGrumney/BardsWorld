using CommonAssets;
using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealsDamage : MonoBehaviour 
{
	[Tooltip("The damage this deals.")]
	public int damage = 1;
	public Vector2 knockBackForce = DefaultValues.KnockBackForce;
	public float StunTime = DefaultValues.StunTime;

	[Tooltip("The sound we play on impact.")]
	public AudioClip impactSound;

	[Tooltip("The object we spawn on impact.")]
	public GameObject spawnOnImpact;

	[Tooltip("Where to spawn that object")]
	public GameObject spawnPoint;

	[Tooltip("Whether we destroy this object on impact.")]
	public bool destroyOnImpact;

	[Tooltip("If the damage has been disabled.")]
	public bool damageDisabled = false;

	public bool OnTrigger = true;
	public bool OnCollision = false;
	public string OnlyDamagesThisTag = null;
 
	public GameObject spawnedBy;

	// the two following method handle both the cases that the damage dealing object uses either:
	//	1. a trigger (which you can pass through) 
	//	2. a collider (which you cannot)
	// they are otherwise functionally identical

	// when something touches the collider
	protected virtual void OnCollisionEnter2D(Collision2D other) 
	{
		if(OnCollision)
		{
			StartCoroutine( Hit( other.gameObject ) );
		}
	}

	// when something enters the trigger
	protected virtual void OnTriggerEnter2D(Collider2D other) 
	{
		if (OnTrigger)
		{
			StartCoroutine( Hit( other.gameObject ) );
		}
	}


	/// <summary>
	/// Hit the other object
	/// </summary>
	/// <param name="other">The Object we're hitting.</param>
	IEnumerator Hit(GameObject other) 
	{
		// wait until end of frame. This allows shield to disable damage
		yield return new WaitForEndOfFrame();

		if(string.IsNullOrWhiteSpace(OnlyDamagesThisTag) || other.CompareTag( OnlyDamagesThisTag ))
		{	
			// deal damage
			if (!damageDisabled)
			{
				Easily.PlaySound(impactSound, transform.position);
				Easily.Knock(other).Back(knockBackForce).From(gameObject).StunningFor(StunTime);

				// Spawn object at spawnPoint or, if none defined at the center of this object
				SpawnObject();

				other.SendMessage("TakeDamage", damage);
				other.SendMessage("ThisHurtYou", spawnedBy);
			}

			// destroy if destroyOnImpact
			if (destroyOnImpact)
			{
				Destroy(this.gameObject); // TODO: Move this into it's own script
			}
		}
	}

	private void SpawnObject()
	{
		Vector3 spawnPosition = transform.localPosition;
		if (spawnPoint is null) { spawnPosition = spawnPoint.transform.localPosition; }
		Easily.Instantiate(spawnOnImpact, spawnPosition);
	}

	/// <summary>
	/// Disables damage on this object
	/// </summary>
	/// <returns></returns>
	public void DisableDamage()
	{
		damageDisabled = true;
	}

	public void EnableDamage()
	{
		damageDisabled = false;
	}
}

