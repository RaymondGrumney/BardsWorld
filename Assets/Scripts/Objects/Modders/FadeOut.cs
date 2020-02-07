using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {

	/// <summary>
	/// The sprite renderer to fade out
	/// </summary>
	protected SpriteRenderer _spriteRenderer;

	/// <summary>
	/// How long it takes to fade the sprite out.
	/// </summary>
	[Tooltip("How long it takes to fade the sprite out.")]
	public float fadeOutTime = 1f;

	// Use this for initialization
	void Start () {
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_spriteRenderer.color = new Color( _spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a - Time.fixedDeltaTime / fadeOutTime );

		if( _spriteRenderer.color.a < 0 ) {
			Destroy(gameObject);
		}
	}
}
