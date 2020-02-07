using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wait N seconds then execute the next event.
/// </summary>
public class WaitNSeconds : GameEvent {

	[Tooltip("How many seconds to wait before seting the next event.")]
	/// <summary>
	/// How many seconds to wait before seting the next event.
	/// </summary>
	public float secondsToWait;

	/// <summary>
	/// When this event was activated.
	/// </summary>
	private float timeActivated;

	public override bool canBeInterruped()
	{
		throw new System.NotImplementedException();
	}

	// what to do every update while active
	public override void onUpdate()
	{
		if(Time.time > timeActivated + secondsToWait ) {
			sendNextEventInChain();
		}
	}


	// what to do when assigned as active event
	public override void onActivation()
	{
		timeActivated = Time.time;
	}

}
