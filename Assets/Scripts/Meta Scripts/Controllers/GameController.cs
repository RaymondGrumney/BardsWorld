using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Gamification, ie. time high score table, rewards for certain times in levels
// TODO: hurt zones for lava, blades, lasers, etc
// TODO: Portals
// TODO: Rotating Rooms

/// <summary>
/// Primarily concerned with coordinating between different game objects
/// </summary>
public class GameController : MonoBehaviour {

	// Data about players

	[Tooltip("The background music for this level.")]
	/// <summary>
	/// The background music for this level.
	/// </summary>
	public AudioClip BGM;	// NOTE: this might move to a level controller when such a thing is implemented.


	[Tooltip("The available characters")]
	/// <summary>
	/// The available player characters.
	/// </summary>
	public GameObject[] playerCharacters;

	[Tooltip("The Event that triggers at the start of the level ( if any )")]
	/// <summary>
	/// The event to execute at level load.
	/// </summary>
	public GameEvent startingEvent;

	/// <summary>
	/// The current event.
	/// </summary>
	private GameEvent _currentEvent;

	/// <summary>
	/// The message controller of the messge window.
	/// </summary>
	public MessageController messageController;

	/// <summary>
	/// The characters controllers of the avaiable characters
	/// </summary>
	private Character[] _characters;

	/// <summary>
	/// The current number of players.
	/// </summary>
	private int currentNumPlayers = 1;

	[Tooltip("Player Cameras.")]
	/// <summary>
	/// The cameras for this level.
	/// </summary>
	public GameObject[] cameras;


	// Initialize Variables
	void Awake () {
		

		initializeCharacterArray();

	}

	// After initialization, assign relationships
	void Start() {
		
	}

	// Update is called once per frame
	void Update () {


		// check if quit
		if (Input.GetButtonDown("Quit"))
		{
			SceneManager.LoadScene("Outro");
		}

		// run the current event
		runEvent();
	}


	// Updated once per fixed frame
	void FixedUpdate() {
		
	}

	/// <summary>
	/// Initializes the character array and populates it with the characters.
	/// </summary>
	private void initializeCharacterArray()
	{
		// initialize
		_characters = new Character[ playerCharacters.Length ];

		// populate
		for( int i = 0; i < playerCharacters.Length; i++ ) {
			// assign character controller
			_characters[ i ] = playerCharacters[ i ].GetComponent<Character>();
		}
	}
		

	/// <summary>
	/// executes the current event's update function
	/// </summary>
	private void runEvent() {
		if (_currentEvent) {
			_currentEvent.onUpdate();
		}
	}


	/// <summary>
	/// Sets if the player's character is or is not receiving input
	/// </summary>
	/// <param name="player">Player.</param>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void setReceivingInput (Player player, bool state) {

		_characters[ player.currentCharacter ].receivingInput = state;

	}

	/// <summary>
	/// Sets if a group of players' characters are receiving input
	/// </summary>
	/// <param name="players">Players.</param>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void setReceivingInput ( Player[] players, bool state ) {
		foreach( Player p in players ) {
			setReceivingInput( p, state );
		}
	}


	/// <summary>
	/// Sets the event and returns true if successful.
	/// </summary>
	/// <returns><c>true</c>, if event was set, <c>false</c> otherwise.</returns>
	/// <param name="gameEventStage">Game event stage.</param>
	public bool setEvent( GameEvent gameEventStage ) {

		// sets currentEvent if there's not currently an event playing or the current event is interruptable
		if(gameEventStage == null && _currentEvent.canBeInterruped() ) {
			_currentEvent = null;
			return true;
		} else 	if (!_currentEvent || _currentEvent.interuptable || gameEventStage.GetInstanceID() == _currentEvent.nextEvent.GetInstanceID() ) {
			
			_currentEvent = gameEventStage;
			_currentEvent.onActivation();

			if (_currentEvent.pausesControl) {

				// disable character controll while this event is active
				foreach( Player p in _currentEvent.controllingPlayers ) {
					_characters[ p.currentCharacter ].receivingInput = false;
				}
			}

			return true;
		} else {
			return false;
		}
	}


	/// <summary>
	/// Check if players are dropping in or out
	/// </summary>
	void playersDropInOut()
	{
//		for( int p = 0; p < players.Length; p++ ) {
//			// if a player presses start
//			if (players[ p ].start) {
//				
//				// if that player isn't active
//				if (!players[ p ].isActive) {
//					
//					// add that player in
//					addNewPlayer( p );
//
//				} else if (currentNumPlayers != 1) {
//					
//					// else remove that player
//					removePlayer( p );
//				}
//			}
//		}
	}

	/// <summary>
	/// Adds a new player.
	/// </summary>
	/// <param name="playerNum">Player number.</param>
	void addNewPlayer(int playerNum) {

//		currentNumPlayers++;
//
//		setPlayerActive( playerNum, true );
//
//		// assign character
//		// if the first character is already active
//		if ( _characters[ 0 ].isActive ) {
//			// temporarily set current character to the first
//			players[ playerNum ].currentCharacter = 0;
//
//			// then seek the next available character
//			seekNextCharacter( players[ playerNum ] );
//
//		} else {
//			// otherwise assign player to first character
//			activateCharacter( 0, players[ playerNum ] );
//		}
	}


	public void makeOnlyActiveCamera( GameObject cameraObj ) {
		
	}


	/// <summary>
	/// Updates the cameras.
	/// </summary>
	private void updateCameras(  ) {
		
		// count number of active cameras
		int numActiveCameras = 0;

		// count active cameras
		foreach( GameObject c in cameras ) {
			if( c.gameObject.activeInHierarchy ) {
				numActiveCameras ++;
			}
		}

		for( int i = 0; i < cameras.Length; i++ ) {

			// only adjust active cameras
			if( cameras[ i ].activeInHierarchy ) {

				// generate viewing rectangle dimensions 
				float x = Screen.width;
				float y = i / numActiveCameras;
				float width = 1;
				float height = 1 / numActiveCameras;

				Rect newVeiwingRect = new Rect( x, y, width, height );

				// get camera for this player 
				Camera camera = cameras[ i ].GetComponent<Camera>();

				camera.rect= newVeiwingRect;		
			}
		}
	}


	// TODO: when subtract player, move existing player to top of list, ie. a single active player should always be player 1


	/// <summary>
	/// Removes the player.
	/// </summary>
	/// <param name="playerNum">Player number.</param>
	void removePlayer(int playerNum) {
//
//		currentNumPlayers--;
//
//		setPlayerActive( playerNum, false );
//
//		// deactivate this character's
//		deactivateCharacter( players[ playerNum ].currentCharacter ); 
	}


	/// <summary>
	/// Seeks the next character for the given player.
	/// </summary>
	/// <param name="player">Player.</param>
	void seekNextCharacter( Player player ) {

		// if all characters are active we can't cycle through them
		if (currentNumPlayers != _characters.Length) {

			// next character, cycling through to begining of array if necessary
			int nextCharacter = ( player.currentCharacter + 1 ) % playerCharacters.Length;
			
			// starting with the next charater, cycling to beginning of array if necessary
			for(int i = nextCharacter; i != player.currentCharacter; i = ( i + 1 ) % playerCharacters.Length ) { 

				// if this character isn't active
				if (!_characters[ i ].isActive) {

					deactivateCharacter( player.currentCharacter );
					activateCharacter( i, player );

					// exit loop
					break;
				}
			}
		}
	}


	/// <summary>
	/// Deactivates the character.
	/// </summary>
	/// <param name="characterNum">Character number.</param>
	void deactivateCharacter( int characterNum )
	{
		Character c = _characters[ characterNum ];

		// deactivate current character
		c.isActive = false;
		c.receivingInput = false;
		c.CanMove = false;
		c.stop();
	}


	/// <summary>
	/// Activates the character.
	/// </summary>
	/// <param name="characterNum">Character number.</param>
	/// <param name="player">The player to assign to the character.</param>
	void activateCharacter( int characterNum, Player player )
	{
		Character c = _characters[ characterNum ];

//		Debug.Log("activateCharacter player: " + player);

		// activate new character
		c.isActive = true;
		c.receivingInput = true;
		c.CanMove = true;

//		Debug.Log("activateCharacter c.controllingPlayer: " + c.controllingPlayer);

		// assign player to character and vice versa
		
//		Debug.Log("activateCharacter c.controllingPlayer: " + c.controllingPlayer);

		// show the life display for the character
		c.GetComponentInChildren<LifeDisplay>().show(); // this should be moved to a function in Character, which passes the Show() command to it's LifeDisplay
	}


	public GameObject getCharacterObject( int characterNum) {
		return playerCharacters[ characterNum ];
	}
}
