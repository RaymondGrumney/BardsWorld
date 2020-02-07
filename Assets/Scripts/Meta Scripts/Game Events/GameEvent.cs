using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour {

	[Tooltip("Whether this event can be interupted by other events.")]
	/// <summary>
	/// Whether this event can be interupted by other events.
	/// </summary>
	public bool interuptable = false;


	[Tooltip("Whether this event pauses player input (excepting \"Next\" option).")]
	/// <summary>
	/// Whether this event pauses player input (excepting "Next" option).
	/// </summary>
	public bool pausesControl = false;


	[Tooltip("The next game event in the chain.")]
	/// <summary>
	/// The next game event in the chain.
	/// </summary>
	public GameEvent nextEvent;


	[Tooltip("The player that controls this event.")]
	/// <summary>
	/// The player that controls this event.
	/// </summary>
	public Player[] controllingPlayers;


	/// <summary>
	/// The game controller.
	/// </summary>
	protected GameController _gameController;


	// initialize variables
	void Start() {
		startRoutine();
	}

	/// <summary>
	/// The routine ran on start for this event.
	/// </summary>
	protected virtual void startRoutine()
	{
		GameObject gameController = GameObject.Find( "Game Controller" );

		if (gameController) {
			_gameController = gameController.GetComponent<GameController>();
		}
	}

	/// <summary>
	/// Called by the game controller every update
	/// </summary>
	public abstract void onUpdate(); // this is where the event does the thing!

	/// <summary>
	/// Called by the game controller when the event is first launched.
	/// </summary>
	public abstract void onActivation();

	/// <summary>
	/// If this gameEvent has completed
	/// </summary>
	public abstract bool canBeInterruped();


	/// <summary>
	/// Sends the next event in chain to the game controller
	/// </summary>
	protected virtual void sendNextEventInChain() {
		
		if (nextEvent) {

			// passes controlling players along to the next event
			nextEvent.controllingPlayers = this.controllingPlayers;

			_gameController.setReceivingInput( controllingPlayers, !nextEvent.pausesControl );
		} else if( pausesControl ) {

			// return control of the character to the player if no next event
			_gameController.setReceivingInput( controllingPlayers, true );
		}

		// passes the next event up to the game cotroller
		_gameController.setEvent( nextEvent );
	}


	/// <summary>
	/// Adds a player to the event.
	/// </summary>
	/// <param name="player">Player.</param>
	public void addPlayer( Player player) {
		Player[] newPlayerlist = new Player[controllingPlayers.Length + 1];

		for( int i = 0; i < controllingPlayers.Length; i++ ) {
			newPlayerlist[i] = controllingPlayers[i];
		}

		newPlayerlist[ newPlayerlist.Length - 1] = player;

		controllingPlayers = newPlayerlist;
	}

}
