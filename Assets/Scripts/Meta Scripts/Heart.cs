using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

	[Tooltip("The full heart sprite.")]
	public Sprite fullHeart;

	[Tooltip("The empty heart sprite.")]
	public Sprite emptyHeart;

	[Tooltip("How big the heart gets on a pulse.")]
	public float pulseScale;

	private Vector3 _initialScale;

	[Tooltip("How long the pulse takes.")]
	public float pulseTime;

	[Tooltip("How many pulses per pulse.")]
	public int numPulses;

	[Tooltip("How long each indivual pulse takes.")]
	public float individualPulseTime;

	[Tooltip("Whether or not to pulse the hearts.")]
	public bool pulse;

	// whether the heart is pulsing in or out
	private bool pulseOut = true;

	// which pulse out of numPulses the heart is currently on
	private int currentPulse = 0;

	// when the heart last pulsed
	private float lastPulse;

	// whether this heart is representing being damaged or not
	private bool damaged = false;

	// this object's sprite renderer
	private SpriteRenderer _spriteRenderer;

	// initialization
	void Awake() {
		_initialScale = transform.localScale;
		_spriteRenderer = GetComponent<SpriteRenderer>();

		updateHeartSprite();
	}

	// called once per frame
	void Update() {
		pulseHeart();
	}

	/// <summary>
	/// Pulses the heart.
	/// </summary>
	void pulseHeart() {

		// if we are pulsing this heart
		if( !damaged && pulse ) {
			
			// if it's time to pulse again
			if ( Time.time > lastPulse ) {
				// increment lastPulse and reset currentPulse
				lastPulse += pulseTime;
				currentPulse = 0;
			}

			// if we still need to scale this 
			if ( pulseOut ) {
				// if we haven't pulse numPulses times
				if (currentPulse < numPulses) {
					scaleTransform( 1f );
				}

				// set pulseOut
				pulseOut = this.transform.localScale.x < _initialScale.x + pulseScale;
			} else {
				
				// if the scale isn't the initial scale
				scaleTransform( -1f );
			}

			// if the scale is the initial scale
			if (this.transform.localScale.x > _initialScale.x + pulseScale) {

				pulseOut = false;
				currentPulse++;
			}
		}
	}


	/// <summary>
	/// Scales the transform up or down. Passing a positive number scales up, a negative scales down
	/// </summary>
	/// <param name="sign"> The sign of the multiplier.</param>
	void scaleTransform(float sign) {
		sign = Mathf.Sign( sign );

		transform.localScale += Vector3.one * sign * Time.deltaTime / individualPulseTime;
	}


	/// <summary>
	/// Sets the damaged.
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void setDamaged(bool state) {
		this.damaged = state;

		updateHeartSprite();
	}

	/// <summary>
	/// Updates the heart sprite.
	/// </summary>
	void updateHeartSprite() {
		
		// full heart if not damaged
		if( !damaged ) {
			_spriteRenderer.sprite = fullHeart;
		} else {

			// empty heart if is damaged
			_spriteRenderer.sprite = emptyHeart;
		}
	}
}
