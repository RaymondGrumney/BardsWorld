/* This script grounds or ungrounds a character, managing when it can / cannot jump. 
 * It also manages the Double Jump ability.
 * 
 * Author: Raymond Grumney
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Regulates whether a character is "grounded", as opposed to 
/// </summary>
public class Grounding : MonoBehaviour {

	/// <summary>
	/// The character this script grounds.
	/// </summary>
	private Character _character;
	// the double jump script (if applicable)
	private DoubleJump _doubleJump;

	// Use this for initialization
	void Start () {
		_character = GetComponentInParent<Character>();
		_doubleJump = GetComponentInParent<DoubleJump>();
	}

	/// <summary>
	/// Raises the collision stay2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnCollisionStay2D(Collision2D other) {
		// get attributes from other object
		Attributes attributes = other.gameObject.GetComponent<Attributes>();

		// if the object has an attribute list
		if (attributes != null) {
			// and it's not ungroundable
			if (!attributes.unGroundable) {
				groundCharacter();
			}
		} else {
			// or if it doesn't have an attribute list
			groundCharacter();
		}
	}

	/// <summary>
	/// Raises the collision exit2 d event.
	/// </summary>
	void OnCollisionExit2D() {
		_character.grounded = false;
	}

	/// <summary>
	/// Grounds the character.
	/// </summary>
	void groundCharacter()
	{
		_character.grounded = true;

		// reset double jump flag
		if (_doubleJump) {
			_doubleJump.resetDoubleJumped();  // IDEA: Is it possible to rewrite character.grounded in doubleJump?
		}

	}
}
