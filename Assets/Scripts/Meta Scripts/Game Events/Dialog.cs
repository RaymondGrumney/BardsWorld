using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : GameEvent {

	// this script is a single dialog event

	[TextArea]
	[Tooltip("The line of dialog to display.")]
	/// <summary>
	/// The line of dialog to display.
	/// </summary>
	public string line;

	[Tooltip("The character portrait to display.")]
	/// <summary>
	/// The character portrait to display.
	/// </summary>
	public Sprite characterPortrait;

	/// <summary>
	/// The message controller.
	/// </summary>
	private MessageController _messageController;


	[Tooltip("Whether to use message speed when displaying these dialogs.")]
	/// <summary>
	/// Whether to use message speed when displaying these dialogs
	/// </summary>
	public bool useMessageSpeed = true;


	/// <summary>
	/// Whether the message has been sent to the 
	/// </summary>
	private bool messageAccepted = false;


	// stuff to do every gameController update while this game event is the active one
	public override void onUpdate()
	{
		if ( !messageAccepted ) {
			sendMessage();
		} else if ( _messageController.messageFinished ){


			if (!_messageController.messageVisible()) {
				sendNextEventInChain();
			} else {

				// checks input for each controlling player
				foreach( Player p in controllingPlayers ) {
					
					if (p.actionPressed || p.action2Down || p.swapLeft || p.swapRight) {
						sendNextEventInChain();
					}
				}
			}
		}
	}

	protected override void sendNextEventInChain()
	{
		base.sendNextEventInChain();

		// if the next event isn't a dialog, hide the message window
		Dialog next = nextEvent as Dialog;

		if( next == null ) {
			_messageController.hideMessage();
		}
	}

	// stuff to do when this event is made active in the game controller
	public override void onActivation()
	{
		_messageController = _gameController.messageController;

		sendMessage();

		Debug.Log("onActivation() messageAccepted: " + messageAccepted);
	}

	public override bool canBeInterruped()
	{
		return _messageController.messageFinished;
	}

	/// <summary>
	/// Sends the message to the game controller
	/// </summary>
	private void sendMessage() {

		// set if the GameController will use messageSpeed for this message
		_messageController.setUseMessageSpeed( useMessageSpeed );

		// attempt to display the message
		messageAccepted = _messageController.giveMessage( line, characterPortrait );

	}
}
