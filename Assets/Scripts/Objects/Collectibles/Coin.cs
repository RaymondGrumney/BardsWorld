using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectible {

	/// <summary>
	/// The session controller for this level.
	/// </summary>
//	private Level _level;

	void Awake() {
//		_level = GameObject.Find( "Level Info" ).GetComponent<Level>();
	}

	protected override void collect(GameObject other)
	{
		// TODO: this
//		_level.addCollectible(this);
	}
}
