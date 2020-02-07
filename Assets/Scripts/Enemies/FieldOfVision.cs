using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enemy's field of vision (sight, sound, smell, etc)
/// </summary>
public class FieldOfVision : MonoBehaviour {

	/// <summary>
	/// The enemy script
	/// </summary>
	private BaseEnemy _baseEnemy;

	/// <summary>
	/// The time the character was seen.
	/// </summary>
	private float _timeSeenCharacter;

	/// <summary>
	/// The collider of the character
	/// </summary>
	private Collider2D _lastSeen;

	// NOTE: this all assumes we only see one character at a time. I forsee problems . . .


	// Use this for initialization
	void Start () {
		_baseEnemy = GetComponentInParent<BaseEnemy>();
	}
		

	void Update() {
		// if we've noticed the character

		// TODO: this really shouldn't access timeToNoticeCharacter directly
		if( _timeSeenCharacter > _baseEnemy.timeToNoticeCharacter ) {
			_baseEnemy.setTarget( _lastSeen.gameObject );
			_timeSeenCharacter = 0;
		}

		// TODO: handle when the character dies
	}


	void OnTriggerStay2D(Collider2D other) {

		// wait to notice player
		if (other.tag == "Player") {
			_lastSeen = other;

			// TODO: darkness? ie. Light * deltatime
			_timeSeenCharacter += Time.deltaTime;
		} 

		// do the bait thing
		if (_baseEnemy.attractedToBait) {
			Attributes attributes = other.GetComponent<Attributes>();
			Debug.Log( "OnTriggerStay() attributes:" + attributes );
			if (attributes != null) {
				if (attributes.bait) {
					_lastSeen = other;
					Debug.Log( "OnTriggerStay() _lastSeen:" + _lastSeen );

					_baseEnemy.setTarget( other.gameObject );
				}
			}

		}
	}


}
