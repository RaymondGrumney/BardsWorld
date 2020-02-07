using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level. This tracks things associated with this particular life. This gets reset upon death.
/// </summary>
public class Level : MonoBehaviour {

	/// <summary>
	/// What game time this level started
	/// </summary>
	private float _timeStart;

	/// <summary>
	/// What game time this level started.
	/// </summary>
	public float timeStart {
		get { return _timeStart; }
	}

	/// <summary>
	/// A list of collectibles retrieved in this layer.
	/// </summary>
	private ArrayList _collectibles;

	// Initialization
	void Awake() {
		_timeStart = Time.time;
		_collectibles = new ArrayList();

		// set up characters from Session
		//
	}

	/// <summary>
	/// Adds a collectible to the list.
	/// </summary>
	/// <param name="collectible">Collectible.</param>
	public void addCollectible(Collectible collectible) {
		bool assigned = false;
		//TODO: check this type checking

		// check if existing
		// if so, add to existing Count<T>
		// else, new Count<T>
		for(int i = 0; i < _collectibles.Count; i++) {
			Count count = _collectibles[ i ] as Count;

			if (!assigned && count != null && count.isType( collectible )) {
				count += 1;

				_collectibles[ i ] = count; // NOTE: do we need to reassign? does <c>as</c> make a new object? don't think so . . .

				assigned = true;
			}
		}

		if (!assigned) {
			_collectibles.Add( collectible );
		}
	}

	// public collectibles
	// constructs arrays of types of collectibles?
	// counts them, at least
}
