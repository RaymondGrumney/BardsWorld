using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A characters special power that spawns an object, which they then pickup
/// </summary>
public class SpawnPower : MagicPower
{

	/// <summary>
	/// The object to spawn when the player activates this power.
	/// </summary>
	[Tooltip("The object to spawn when the player activates this power.")]
	public GameObject spawnable;

	protected Character _character;
	protected GrabAndCarry _grabAndCarry;

	public bool onlySpawnOne = false;

	public GameObject lastObjectSpawned;

	/// <summary>
	/// The routine to run on Awake().
	/// </summary>
	public override void init(Character character) {

		_character = character;
		_grabAndCarry = _character.GetComponent<GrabAndCarry>();

		if (!_grabAndCarry) {
			Destroy( this );
		}
	}

	public override void castSpell() {

		GameObject newSpawn = Instantiate( spawnable, _character.transform.parent );

		// give to _grabAndCarry or destroy
		if (!_grabAndCarry.give( newSpawn )) {
			Destroy( newSpawn );
			// destroy other if only one should be spawned	
		} else if (onlySpawnOne) {
			if (lastObjectSpawned) {
				Destroy( lastObjectSpawned );
			}

			lastObjectSpawned = newSpawn;
		}
	}
}

