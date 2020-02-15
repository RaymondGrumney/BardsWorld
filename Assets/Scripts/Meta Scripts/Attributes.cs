using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game attributes for this object
/// </summary>
public class Attributes : MonoBehaviour {


	[Tooltip("The Unique in-game ID applied to this object.")]
	/// <summary>
	/// The Unique in-game ID applied to this object. See documentation for naming conventions.
	/// </summary>
	public string ID;

	[Tooltip("Whether this object can be carried.")]
	/// <summary>
	/// Whether this object can be carried.
	/// </summary>
	public bool carriable = false;

	[Tooltip("Whether this object floats in water, and, therefore, IS A WITCH!!!")]
	/// <summary>
	/// Whether this object floats in water, and, therefore, IS A WITCH!!!.
	/// </summary>
	public bool floats = false;

	[Tooltip("The level at which this floats, as a percentage of the height of the sprite.")]
	/// <summary>
	/// The level at which this floats, as a percentage of the height of the colliders.
	/// </summary>
	public float floatOffset = 0.5f;

	[Tooltip("Whether this object does not ground characters.")]
	/// <summary>
	/// Whether this object does not ground characters.
	/// </summary>
	public bool doesNotGroundCharacter = false;

	[Tooltip("Whether this object is bait for enemies.")]
	/// <summary>
	/// Whether this object is bait for enemies.
	/// </summary>
	public bool bait;

	[Tooltip("If the object should be subject to additional drag on a button.")]
	/// <summary>
	/// If the object should be subject to additional drag on a button.
	/// </summary>
	public bool slowOnButton;

	[Tooltip("The Color of this object (for buttons).")]
	/// <summary>
	/// The Color of this object (for color-oriented puzzles).
	/// </summary>
	public string color;

	[Tooltip("If this object grows vines.")]
	/// <summary>
	/// If this object grows vines.
	/// </summary>
	public bool grows;
}
