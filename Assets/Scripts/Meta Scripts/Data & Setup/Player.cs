using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour {

	/// <summary>
	/// The game controller.
	/// </summary>
	private GameController _gameController;


	[Tooltip("The controller number.")]
	/// <summary>
	/// The number assigned to this player by the GameController. This tells the player object which controller input to read.
	/// </summary>
	public int controllerNumber;


//	[Tooltip("Whether this player is active.")]
	/// <summary>
	/// Whether this player is active in the game.
	/// </summary>
	public bool isActive {
		get {
			return _isActive;
		}

		set{
			_isActive = value;
		}
	}

	private bool _isActive;

//	[Tooltip("current character this player is controlling.")]
	/// <summary>
	/// The character assigned to this player
	/// </summary>
	public int currentCharacter{
		get {
			return _currentCharacter;
		}

		set {
			// set value
			_currentCharacter = value;

			if (_gameController) {
				GameObject target = _gameController.getCharacterObject( _currentCharacter );

				// assign camera target
				_cameraController.target = target;
			}

		}
	}

	/// <summary>
	/// The character assigned to this player
	/// </summary>
	private int _currentCharacter;

//	[Header("Character Inputs")]
//	[Tooltip("Current input on the horizontal axis.")]
	/// <summary>
	/// The current input on the horizontal axis.
	/// </summary>
	public float horizontal {
		get { return _horizontal; }
	}

	/// <summary>
	/// The current input on the horizontal axis.
	/// </summary>
	private float _horizontal; 

//	[Tooltip("Current input on the vertical axis.")]
	/// <summary>
	/// The current input on the vertical axis
	/// </summary>
	public float vertical {
		get{ return _vertical; }
	}

	/// <summary>
	/// The current input on the vertical axis
	/// </summary>
	private float _vertical;


//	[Tooltip("Action button pressed.")]
	/// <summary>
	/// Whether the action button was just pressed down.
	/// </summary>
	public bool actionPressed {					// this button does the thing the character does
		get{ return _actionPressed; }
	}

	/// <summary>
	/// Whether the action button was just pressed down.
	/// </summary>
	private bool _actionPressed;

//	[Tooltip("Action button being held.")]
	/// <summary>
	/// Whether the action button is currently being held.
	/// </summary>
	public bool actionHeld {						// this button does the thing the character does
		get { return _actionHeld; }
	}
		
	/// <summary>
	/// Whether the action button is currently being held.
	/// </summary>
	private bool _actionHeld;

//	[Tooltip("Action button released.")]
	/// <summary>
	/// Whether the action button was just released.
	/// </summary>
	public bool actionReleased;					// this button does the thing the character does

//	[Tooltip("2nd Action button.")]
	/// <summary>
	/// Whether the 2nd action button was just pressed.
	/// </summary>
	public bool action2Down;					// second action button (jump or stick)

//	[Tooltip("Select button.")]
	/// <summary>
	/// Whether the swap left button was just pressed.
	/// </summary>
	public bool swapLeft;							// this button swaps between characters

//	[Tooltip("Start button.")]
	/// <summary>
	/// Whether the start button was just pressed.
	/// </summary>
	public bool start;							// this button will bring a player into the game

//	[Tooltip("Pause button.")]
	/// <summary>
	/// Whether the pause button was just pressed.
	/// </summary>
	public bool pause;							// this button will drop the player out of the game (if not only player)

	/// <summary>
	/// If the power button has been pressed.
	/// </summary>
	/// <value><c>true</c> if power button pressed; otherwise, <c>false</c>.</value>
	public bool powerButtonPressed {
		get {

			return Input.GetButtonDown( "P" + controllerNumber + "Power" );
		}
	}
	/// <summary>
	/// If the power button has been released.
	/// </summary>
	/// <value><c>true</c> if power button released; otherwise, <c>false</c>.</value>
	public bool powerButtonReleased{
		get {
			return Input.GetButtonUp( "P" + controllerNumber + "Power" );
		}
	}

	/// <summary>
	/// If the power button is being held.
	/// </summary>
	/// <value><c>true</c> if power button held; otherwise, <c>false</c>.</value>
	public bool powerButtonHeld {
		get{
			return Input.GetButton( "P" + controllerNumber + "Power" );
		}
	}

	private bool _swapRight;

	public bool swapRight {
		get {
			return _swapRight;
		}
	}

	public bool nextPower;

	private CameraController _cameraController;

	public CameraController cameraController {
		set { _cameraController = value; }
	}



	void Start(  ) {

		GameObject gameController = GameObject.Find( "Game Controller" );

		if (gameController) {
			_gameController = gameController.GetComponent<GameController>();
		}
	}
		

	void Update() {
		readInputs();
	}

	/// <summary>
	/// Reads the controller inputs for this player
	/// </summary>
	void readInputs()
	{
		// 
		start = Input.GetButtonDown( "P" + controllerNumber + "Pause" );

		// assign inputs
		if (isActive) {
			
			// get horizontal or vertical axes
			_horizontal = Input.GetAxisRaw( "P" + controllerNumber + "Horizontal" );


			if (Mathf.Abs( horizontal ) < 0.1f) {
				_vertical = Input.GetAxisRaw( "P" + controllerNumber + "Vertical" );
			}
			else {
				_vertical = 0;
			}

			// get button presses
			_actionPressed = Input.GetButtonDown( "P" + controllerNumber + "Interact" );
			_actionHeld = Input.GetButton( "P" + controllerNumber + "Interact" );
			actionReleased = Input.GetButtonUp( "P" + controllerNumber + "Interact" );

			action2Down = Input.GetButtonDown( "P" + controllerNumber + "Jump" );

			swapLeft = Input.GetButtonDown( "P" + controllerNumber + "SwapLeft" );
			_swapRight = Input.GetButtonDown( "P" + controllerNumber + "SwapRight" );

			nextPower = Input.GetButtonDown( "P" + controllerNumber + "NextPower" );
		}
	}
}
