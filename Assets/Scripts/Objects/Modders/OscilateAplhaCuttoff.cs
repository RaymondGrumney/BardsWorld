using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Oscilates the Alpha Cutoff of a Sprite Mask
/// </summary>
public class OscilateAplhaCuttoff : MonoBehaviour {

	/// <summary>
	/// The amplitude of the oscilation.
	/// </summary>
	[Range(0,1)]
	[Tooltip("The amplitude of the oscilation (how far it wanders from default.")]
	public float amplitude;

	/// <summary>
	/// The frequency of the oscillation.
	/// </summary>
	[Tooltip("The frequency of the oscilation (how often it cycles.")]
	public float frequency;

	/// <summary>
	/// Noise in oscilation.
	/// </summary>
	[Tooltip("Noise in the oscilation (random changes within the cutoff).")]
	public float noise;

	private float _defaultCutoff;

	/// <summary>
	/// The distance between the default value and 0 or 1, whichever is smaller
	/// </summary>
	private float _diff;

	private float phase = 0;

	// Validate properies
	void OnValidate() {
		if(amplitude < 0) {
			amplitude = 0;
		}

		if(frequency < 0) {
			frequency = 0;
		}

		if(noise < 0) {
			noise = 0;
		}
	}

	/// <summary>
	/// The sprite mask.
	/// </summary>
	private SpriteMask _spriteMask;

	// initialize
	void Awake() {
		
		_spriteMask = GetComponent<SpriteMask>();
		_defaultCutoff = _spriteMask.alphaCutoff;

		if (_defaultCutoff > 0.5f) {
			_diff = 1 - _defaultCutoff;
		} else {
			_diff = _defaultCutoff;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// randomly add noise
//		float randomNumber = Random.Range( 0f, 1f);
//
//		if (randomNumber < noise / Time.fixedDeltaTime ) {
//			phase = (phase + Random.Range( 0f, 1f )) % 1 ;
//		}

		// set alpha cutoff
		_spriteMask.alphaCutoff = _defaultCutoff + _diff * MyUtilities.Oscillation( amplitude, frequency, phase, Time.time );

	}
}
