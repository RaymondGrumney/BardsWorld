using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class from which classes that recolor a sprite can be derived (and turned off by other classes if necessary).
/// </summary>
public abstract class Colorizer : MonoBehaviour
{
	protected Color _defaultColor;
	protected SpriteRenderer _spriteRenderer;

	void Awake() {
		_spriteRenderer = GetComponent<SpriteRenderer>();

		if (_spriteRenderer != null) {
			_defaultColor = _spriteRenderer.color;
		} else {
			Destroy( this ); 			// we don't need this at runtime if we didn't assign to a gameobject with a sprite renderer
		}
	}
}

