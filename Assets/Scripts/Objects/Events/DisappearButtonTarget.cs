using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearButtonTarget : ButtonTarget {

//	[Tooltip("The time to fade in or out.")]
	/// <summary>
	/// The time to fade in or out.
	/// </summary>
//	public float fadeOutInTime;

	/// <summary>
	/// The sprite renderers associated with this object
	/// </summary>
	private SpriteRenderer[] _spriteRenderers;

	/// <summary>
	/// The initial colors of the sprite renderers.
	/// </summary>
	private Color[] _initialColors;

	/// <summary>
	/// The current alpha value.
	/// </summary>
	private float _currentAlpha = 1;

	/// <summary>
	/// The colliders associated with this object.
	/// </summary>
	private Collider2D[] _colliders;

	/// <summary>
	/// The time this was activated.
	/// </summary>
	private float _timeActivated;

	/// <summary>
	/// The time this was deactivated.
	/// </summary>
	private float _timeDeactivated;


	private Colorizer[] _colorizers;

	// Use this for initialization
	void Awake () {
		// get colliders and colors
		_colliders = GetComponents<Collider2D>();
		_spriteRenderers = GetComponents<SpriteRenderer>();
		_initialColors = new Color[_spriteRenderers.Length];
		_colorizers = GetComponents<Colorizer>();

		// assign initial colors
		for(int i = 0; i < _spriteRenderers.Length; i++) {
			_initialColors[ i ] = _spriteRenderers[ i ].color;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		// fade in
//		if (_timeDeactivated + fadeOutInTime > Time.time) {
//			if (fadeOutInTime > 0) {
//				_currentAlpha += Time.fixedDeltaTime / fadeOutInTime;
//			} else {
//				_currentAlpha = 1;
//			}
//		}	
//
//		// fade out
//		else if(_timeActivated + fadeOutInTime > Time.time) {
//			if (fadeOutInTime > 0) {
//				_currentAlpha -= Time.fixedDeltaTime / fadeOutInTime;
//			} else {
//				_currentAlpha = 0;
//			}
//		}
//
//		updateColors();

	}

	/// <summary>
	/// Updates the colors of each sprite
	/// </summary>
	void updateColors() {
		for( int i = 0; i < _spriteRenderers.Length; i++ ) {
			Color newColor = new Color( _initialColors[ i ].r, _initialColors[ i ].g, _initialColors[ i ].b, _currentAlpha);
				
			_spriteRenderers[ i ].color = newColor;
		}
	}

	public override void Activate(bool state)
	{
		// note that for this object, activated means the object is disappearing, so the reverse of sent state is applied;

		// disappearing
		if (state) {
			_timeActivated = Time.time;

		} else {
			_timeDeactivated = Time.time;

		}

		// set enabled/disabled for all colliders
		foreach( Collider2D c in _colliders ) {
			c.enabled = !state;
		}

		// disable any colorizors
		foreach( Colorizer c in _colorizers ) {
			c.enabled = !state;
		}

		// disable sprite renderers TODO: make fade in / out work
		foreach( SpriteRenderer s in _spriteRenderers ) {
			s.enabled = !state;
		}
	}
}
