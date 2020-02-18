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

	void Start () 
	{
		_baseEnemy = GetComponentInParent<BaseEnemy>();
	}
		

	void Update() 
	{
		// if we've noticed the character
		if( _timeSeenCharacter > _baseEnemy.timeToNoticeCharacter ) 
		{
			SendMessageUpwards( "SetTarget", _lastSeen.gameObject );
			_timeSeenCharacter = 0;
		}
	}


	void OnTriggerStay2D(Collider2D other) 
	{
		// wait to notice player
		if (other.CompareTag("Player")) 
		{
			_lastSeen = other;

			// TODO: darkness? ie. Light * deltatime
			_timeSeenCharacter += Time.deltaTime;
		} 

		// do the bait thing
		if (_baseEnemy.attractedToBait) 
		{
			Attributes attributes = other.GetComponent<Attributes>();
			if (attributes != null) 
			{
				if (attributes.bait) 
				{
					_lastSeen = other;

					SendMessageUpwards("SetTarget", other.gameObject);
				}
			}
		}
	}
}
