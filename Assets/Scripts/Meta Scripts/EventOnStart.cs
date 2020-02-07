using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Launches an event at the begining of a level.
/// </summary>
public class EventOnStart : MonoBehaviour {

	/// <summary>
	/// The game event.
	/// </summary>
	public GameEvent gameEvent;

	/// <summary>
	/// If this has been activated.
	/// </summary>
	private bool activated = false;

	void Start() {
	}

	//HACK: doesn't belong in LateUpdate. Why doesn't this work in Start()?
	void LateUpdate() {
		if (!activated) {

			gameEvent.onActivation();
			activated = true;
		}

		gameEvent.onUpdate();
	}
}
