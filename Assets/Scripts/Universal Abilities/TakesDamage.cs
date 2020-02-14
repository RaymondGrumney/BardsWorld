using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TakesDamage : MonoBehaviour {


	[Tooltip("The maximum health of this object. Also the starting health on a level by default, unless currentHealth != 0..")]
	public int maxHealth = 1;

	[Tooltip("The health of this object. Will default to maxHealth at runTime if set to 0 (or higher than maxHealth)")]	
	public int currentHealth;

	[Tooltip("The standard length of time (in seconds) not accepting damage after a hit.")]
	public float standardDamageTimeout = 2;

	[Tooltip("Whether currently accepting damage.")]
	public bool takingDamage = true;

	[Tooltip("The sound played when taken damage.")]
	public AudioClip hitSound;

	[Tooltip("The object spawned when taken damage.")]
	public GameObject spawnOnDamage;

	[Tooltip("The sound played when this objecy dies.")]
	public AudioClip deathSound;

	[Tooltip("The object spawned when this objecy dies.")]
	public GameObject spawnOnDeath;


	// private variables					

	private float _damageTimeout;				// when we next start accepting damage
	private bool _flicker = false;				// whether we're currently showing the sprite

	// private component's we get at runtime
	private LifeDisplay lifeDisplay;			// this object's life display
	private SpriteRenderer spriteRenderer;		// this objects sprite renderer

	void Start() {
		// set current health to max health if current health is set to 0 or above maxHealth at runtime 
		if( currentHealth == 0 || currentHealth > maxHealth ) {
			currentHealth = maxHealth;
		}

		// get this game object's component's needed for this script
		lifeDisplay = GetComponentInChildren<LifeDisplay>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {
		// if we are not taking damage right now
		if (_damageTimeout > Time.time) {
			// flicker sprite
			if (spriteRenderer) {
				spriteRenderer.enabled = !_flicker;
				_flicker = !_flicker;
			}
		} else {
			// make sure the sprite is enabled if we're not flickering
			if (spriteRenderer) {
				if (!spriteRenderer.enabled) {
					spriteRenderer.enabled = true;
					_flicker = false;
				}
			}
		}

		if (currentHealth <= 0) {
			death();
		}
	}


	/// <summary>
	/// Adjust life total by amount.
	/// </summary>
	/// <param name="damage">Damage.</param>
	public void adjustLife(int value) {
		
		currentHealth += value;

		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}

		// if the ad
		if (value < 0) {
			_damageTimeout = Time.time + standardDamageTimeout;		// prevent damage for the standard damage timeout
			playHitSound();
		}


		if (lifeDisplay != null) {
			lifeDisplay.adjustDisplay( value );
		}


		// spawn SpawnOnImpact object
		if (spawnOnDamage != null) {
			Instantiate( spawnOnDamage, this.transform.position, Quaternion.identity );
			spawnOnDamage = null;
		}
	}

	public void kill() {
		currentHealth = 0;
	}

	/// <summary>
	/// if currently taking damage
	/// </summary>
	/// <returns><c>true</c>, if currently taking damage, <c>false</c> otherwise.</returns>
	public bool isTakingDamage() {
		return _damageTimeout < Time.time && takingDamage;
	}


	/// <summary>
	/// Plays the hit sound.
	/// </summary>
	public void playHitSound() {
		if (hitSound != null) { 
			AudioSource.PlayClipAtPoint( hitSound, transform.position );
		}
	}

	// called when the object reaches 0 life
	private void death() {

		// reload this scene
		//SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );

		
		// spawn SpawnOnImpact object
		if ( spawnOnDeath != null ) {
			Instantiate( spawnOnDeath, this.transform.position, Quaternion.identity );
			spawnOnDeath = null;				// make sure we only spawn 1
		}


		Destroy( this.gameObject );
	}


}


