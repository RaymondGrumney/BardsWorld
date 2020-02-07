using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour {

	// TODO: this should be moved to user config files
	[Tooltip("The message speed.")]
	[Range(1,11)]
	/// <summary>
	/// The message speed.
	/// </summary>
	public int messageSpeed;

	[Tooltip("The \"next\" icon for messages.")]
	/// <summary>
	/// The "next" icon for messages.
	/// </summary>
	public Sprite messageNextIcon;

	[Tooltip("The texture for the sprites to use for buttons.")]
	/// <summary>
	/// The texture for the sprites to use for buttons.
	/// </summary>
	public Texture2D buttonTexture;


	[Tooltip("How long to fade in/out a message.")]
	/// <summary>
	/// How long to fade in/out a message.
	/// </summary>
	public float messageFadeInOut = 0.5f;

	// message window variables

	[Tooltip("The UI image of the message window.")]
	/// <summary>
	/// The UI image of the message window.
	/// </summary>
	public Image messageWindowImage;

	[Tooltip("The character portrait on the message window.")]
	/// <summary>
	/// The character portrait on the message window.
	/// </summary>
	public Image messageCharacterPortrait;
	/// <summary>
	/// The text of the message window.
	/// </summary>
	private Text _messageText;

	/// <summary>
	/// What Time.time to stop showing the message
	/// </summary>
	private float _messageDisplayUntil = 0f;

	/// <summary>
	/// The alpha of the image and text of the message.
	/// </summary>
	private float _messageAlpha = 0f;

	/// <summary>
	/// the color of the message text
	/// </summary>
	private Color _messageTextColor;

	/// <summary>
	/// The color of the message image.
	/// </summary>
	private Color _messageImageColor;

	/// <summary>
	/// The color of the portrait image.
	/// </summary>
	private Color _messagePortraitColor;

	[Tooltip("The standard length of time a message displays.")]
	/// <summary>
	/// The standard length of time a message displays.
	/// </summary>
	public float messageStandardDisplayTime = 3f;

	/// <summary>
	/// The string currently displaying on the message window.
	/// </summary>
	private string _messageString = "empty";

	/// <summary>
	/// The next char to show during message displaying.
	/// </summary>
	private int _messageCurrentChar;

	/// <summary>
	/// If displaying the next icon
	/// </summary>
	private bool _messageDisplayNextIcon;

	/// <summary>
	/// Whether the window is currently using the message speed setting.
	/// </summary>
	private bool _useMessageSpeed = false;

	private int _inverseSpeed;

	/// <summary>
	/// If the message has finished displaying
	/// </summary>
	/// <returns><c>true</c>, if the message has finished displaying, <c>false</c> otherwise.</returns>
	public bool messageFinished {
		get{ return _messageCurrentChar >= _messageString.Length; }
	}

	//TODO: next icon or whatever

	// Initialize variables
	void Awake() {
		setMessageSpeed( messageSpeed );

		getMessageComponents();
	}

	// Do things on start up
	void Start () {
		hideMessage( 0 );
	}


	// Updated once per fixed frame
	void FixedUpdate() {

		// fade in / out messages
		updateMessage();
	}

	/// <summary>
	/// Sets the message speed.
	/// </summary>
	/// <param name="newSpeed">New speed.</param>
	public void setMessageSpeed(int newSpeed) {
		
		messageSpeed = newSpeed;

		if (messageSpeed == 11) { _inverseSpeed = 0; } 
		else { _inverseSpeed = 12 - messageSpeed; }
	}

	/// <summary>
	/// Gets the message components.
	/// </summary>
	void getMessageComponents()
	{
		_messageText = GetComponentInChildren<Text>();
		_messageImageColor = messageWindowImage.color;
		_messageTextColor = _messageText.color;
		_messagePortraitColor = messageCharacterPortrait.color;
	}	


	public void setUseMessageSpeed(bool state) {
		_useMessageSpeed = state;
	}
		

	/// <summary>
	/// Displays the text using the standard display time
	/// </summary>
	/// <param name="text">Text.</param>
	public void showMessage(string text) {

		messageCharacterPortrait = null;

		showMessage( parseText(text), messageStandardDisplayTime );

	}



	/// <summary>
	/// Gives the game controller a message to display. 
	/// </summary>
	/// <returns><c>true</c>, if message was displayed, <c>false</c> otherwise.</returns>
	/// <param name="text">string.</param>
	public bool giveMessage( string text, Sprite characterPortrait ) {

//		Debug.Log("GiveMessage() messageFinished: " + messageFinished);
//		Debug.Log("GiveMessage() Time.time > _messageDisplayUntil: " + ( Time.time > _messageDisplayUntil) ) ;

		// if the message has finished displaying or 
		if( messageFinished || Time.time > _messageDisplayUntil ) {

			_messageCurrentChar = -1;

			if (characterPortrait != null) {
				messageCharacterPortrait.sprite = characterPortrait;
			}

			showMessage( parseText( text ), messageStandardDisplayTime );  

			return true;
		} 
		else { 
			return false; 
		}
	}


	/// <summary>
	/// Gives the game controller a message to display. 
	/// </summary>
	/// <returns><c>true</c>, if message was displayed, <c>false</c> otherwise.</returns>
	/// <param name="text">string.</param>
	public bool giveMessage( string text ) {

		if( messageFinished || Time.time > _messageDisplayUntil ) {

			_messageCurrentChar = -1;

			messageCharacterPortrait = null;

			showMessage( parseText( text ), messageStandardDisplayTime );  

			return true;
		} 
		else { 
			return false; 
		}
	}


	/// <summary>
	/// Gives the game controller a message to display. 
	/// </summary>
	/// <returns><c>true</c>, if message was displayed, <c>false</c> otherwise.</returns>
	/// <param name="dialog">Dialog.</param>
	public bool giveMessage ( Dialog dialog ) {

		if( messageFinished || Time.time > _messageDisplayUntil ) {

			_messageCurrentChar = -1;

			//			_messageCurrentChar = 0;

			if (dialog.characterPortrait != null) {
				messageCharacterPortrait.sprite = dialog.characterPortrait;
			}

			showMessage( parseText( dialog.line ), messageStandardDisplayTime );

			return true;
		} 
		else { 
			return false; 
		}
	}



	public string parseText( string text ) {
		string workingString = "";

		// TODO : this will be a thing
		string parsedText = text;
//		int pointer = 0;

		for(int i = 0; i < text.Length; i++) {
			// reset working string and skip to next character
			if (text[ i ] == '[') {
				parsedText += workingString;
				workingString = "";
				i++;
			}

			if (text[i] == ']') {

				// do a thing with parsed text
					
			}

			workingString += text[ i ];

		}


		// https://docs.unity3d.com/Manual/StyledText.html

		// <quad material=1 size=20 x=0.1 y=0.1 width=0.5 height=0.5 />



		return parsedText;
	}


	/// <summary>
	/// Displays the text for n seconds
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="displayForNSeconds">Display for N seconds.</param>
	public void showMessage( string text, float displayForNSeconds ) {
		// clear previous message
		_messageAlpha = 0f;

		// set 
		_messageString = text;
		_messageDisplayUntil = Time.time + displayForNSeconds;

		displayNextChar();

		updateMessage();
	}


	public void hideMessage( float afterNSeconds ) {
		_messageDisplayUntil = Time.time + afterNSeconds;
	}


	public void hideMessage( ) {
		_messageDisplayUntil = Time.time + messageFadeInOut;
	}


	/// <summary>
	/// Updates the message.
	/// </summary>
	private void updateMessage() {

		// fade in
		if ( inFadeInTime() ) {

			_messageAlpha += Time.deltaTime / messageFadeInOut;

		} else if ( !messageFinished && _useMessageSpeed ) {

			displayNextChar();

		} else if (inFadeOutTime()) {

			// fade out
			_messageAlpha -= Time.deltaTime / messageFadeInOut;

		} else if (_messageDisplayNextIcon) {

			// blink input icon maybe
			blinkNextIcon();

		}

		assignMessageColors();
	}


	/// <summary>
	/// If the message should be fading in
	/// </summary>
	/// <returns><c>true</c>, if the message should be fading in, <c>false</c> otherwise.</returns>
	private bool inFadeInTime()
	{
		return _messageDisplayUntil - messageFadeInOut > Time.time && _messageAlpha < 1;
	}


	// IDEA: no fade in time, or very breif (1 or 2 frames)

	/// <summary>
	/// If the message should be fading out
	/// </summary>
	/// <returns><c>true</c>, if the message should be fading out, <c>false</c> otherwise.</returns>
	bool inFadeOutTime()
	{
		return _messageDisplayUntil - messageFadeInOut < Time.time && _messageAlpha > 0;
	}


	/// <summary>
	/// Displays the next char.
	/// </summary>
	private void displayNextChar()
	{

		int checkTime = (int) ( Time.time * 22 );

		// this should display a character every n 22nds of a second, where n is messageSpeed 	
		if (_inverseSpeed == 0) {
			nextChar();
			
		} else if (checkTime % _inverseSpeed == 0) {
			nextChar();
		}
	}

	// 
	void nextChar() {

		_messageCurrentChar++;
		_messageText.text = _messageString.Substring( 0, _messageCurrentChar );
	}


	/// <summary>
	/// Blinks the next icon.
	/// </summary>
	private void blinkNextIcon() {
		/// TODO: this
	}


	/// <summary>
	/// Assigns colors to message text and image.
	/// </summary>
	private void assignMessageColors()
	{

		// text color / alpha
		Color newTextColor = new Color( _messageTextColor.r, _messageTextColor.g, _messageTextColor.b, _messageAlpha );
		Color newWindowImageColor = new Color( _messageImageColor.r, _messageImageColor.g, _messageImageColor.b, _messageAlpha );
		Color newCharacterPortraitColor = new Color( _messageImageColor.r, _messageImageColor.g, _messageImageColor.b, _messageAlpha );

		_messageText.color = newTextColor;
		messageWindowImage.color = newWindowImageColor;
		messageCharacterPortrait.color = newCharacterPortraitColor;
	}

	/// <summary>
	/// If this message window is currently visible
	/// </summary>
	/// <returns><c>true</c>, if visible was messaged, <c>false</c> otherwise.</returns>
	public bool messageVisible() {
		return _messageDisplayUntil > Time.time;
	}
}
