using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A trigger for a game event.
/// </summary>
public class GameEventTrigger : MonoBehaviour {
	
	// NOTE: event scripts live attached to these trigger objects
	// TODO: game events should live as an array on this to simplify creation

	[Tooltip("Whether this can be triggered multiple times.")]
	/// <summary>
	/// Whether this can be triggered multiple times.
	/// </summary>
	public bool canBeRetriggered = false;


	[Tooltip("The game event this triggers.")]
	/// <summary>
	/// The game event this triggers.
	/// </summary>
	public GameEvent gameEvent;


	/// <summary>
	/// The game controller.
	/// </summary>
	protected GameController _gameController;


	/// <summary>
	/// Whether this has been triggered
	/// </summary>
	protected bool triggered = false;


	[Tooltip("Whether this will affect and be controlled by all players ( or just the triggering player ).")]
	/// <summary>
	/// Whether this will affect and be controlled by all players ( or just the triggering player ).
	/// </summary>
	public bool controlledByAllPlayers = false;


	// initialize variables
	void Start() {
		_gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
	}


	// when an object passes through an attacted trigger collider
	protected virtual void OnTriggerEnter2D( Collider2D other ) {
		setEvent( other.gameObject );
	}


	// when an object contacts an attached collider
	protected virtual void OnCollisionEnter2D( Collision2D other ) {
		setEvent( other.gameObject );
	}


	/// <summary>
	/// passes the attached game event to the game controller.
	/// </summary>
	/// <param name="other">Other.</param>
	void setEvent(GameObject other) {

		// only triggers if it hasn't been previously triggereed or retriggerable, and if other is a Character object
		if (!triggered || canBeRetriggered) {
			if (other.tag == "Player") {
				
				// set event in game controller
				triggered = _gameController.setEvent( gameEvent );
			}
		}
	}
}
