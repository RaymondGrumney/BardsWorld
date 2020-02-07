using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	/// <summary>
	/// The available menu items in this menu.
	/// </summary>
	[Tooltip("The available menu items in this menu.")]
	public GameObject[] menuItems;

	/// <summary>
	/// The UI Element indicating which option the user is selecting.
	/// </summary>
	[Tooltip("The UI Element indicating which option the user is selecting.")]
	public GameObject pointer;

	/// <summary>
	/// The color to indicate the selected option.
	/// </summary>
	[Tooltip("The color of the selected option.")]
	public Color selectedColor;

	public float colorCycleDifference = 0.5f;
	public float colorCycleSpeed = 2f;

	/// <summary>
	/// The default color of the current selected option.
	/// </summary>
	private Color _defaultColor;

	/// <summary>
	/// The sound when the selection changes.
	/// </summary>
	[Tooltip("The sound when the selection changes.")]
	public AudioClip moveSound;

	/// <summary>
	/// The sound when the final selection is made.
	/// </summary>
	[Tooltip("The sound when the selection changes.")]
	public AudioClip selectSound;

	/// <summary>
	/// The audio source.
	/// </summary>
	private AudioSource _audioSource;

	/// <summary>
	/// The current menuItem selection.
	/// </summary>
	private int _currentSelection = 0;

	/// <summary>
	/// The text element of the current menu item.
	/// </summary>
	private Text _currentItemText;

	/// <summary>
	/// The player in control of the menu
	/// </summary>
	protected Player _controllingPlayer;

	/// <summary>
	/// If currently allowing directional input.
	/// </summary>
	protected bool _allowDirectionalInput;

	/// <summary>
	/// The previous menu.
	/// </summary>
	private MenuController previousMenu;

	/// <summary>
	/// Whether this <see cref="MenuController"/> is active. 
	/// </summary>
	private bool _isActive;

	/// <summary>
	/// If the user is currently pressing a direction.
	/// </summary>
	/// <value><c>true</c> if user pressing direction; otherwise, <c>false</c>.</value>
	private bool userPressingDirection {
		get {
			return Mathf.Abs( _controllingPlayer.vertical ) > 0.1f || Mathf.Abs( _controllingPlayer.horizontal ) > 0.1f;
		}
	}

	/// <summary>
	/// The game controller in this scene.
	/// </summary>
//	protected GameController gameController;

	// Intialize variables
	void Awake () {

		// TODO: determine controlling player for pause menu

		_audioSource = GetComponent<AudioSource>();
	}


	public void makeActive() {
		
		_isActive = true;

		foreach( Image i in transform.parent.GetComponentsInChildren<Image>() ) {
			i.color = new Color(i.color.r, i.color.g, i.color.b, 1f);
		}

		foreach( Text t in transform.parent.GetComponentsInChildren<Text>() ) {
			t.color = new Color(t.color.r, t.color.g, t.color.b, 1f);
		}
	}

	public void makeInactive() {

	}

	// set to starting state
	void Start() {
		_controllingPlayer = GameObject.Find("Game Controller").GetComponent<GameController>().players[0];

//		_controllingPlayer.isActive = true;

		// set colors and the like for initial selection
		updateSelection();
	}

	// Update is called once per frame
	void Update () {

		Color colorDifference = _defaultColor - selectedColor;
		float colorCycle = Mathf.Abs( MyUtilities.Oscillation( colorCycleDifference, 1 / colorCycleSpeed, 0, Time.time ) );

		// cycle colors
		_currentItemText.color = selectedColor + ( colorDifference  * colorCycle );

		// move up / down
		if( _isActive && _allowDirectionalInput && userPressingDirection ) {
			// add or subtract based on sign of horizontal 
			menuItems[ _currentSelection ].GetComponent<Text>().color = _defaultColor;

			_currentSelection = (int) ( ( _currentSelection - (int) Mathf.Sign(_controllingPlayer.vertical) ) % menuItems.Length );

			// cycle through if negative
			if (_currentSelection == -1) {
				_currentSelection += menuItems.Length;
			}
				
			updateSelection();


			// adjust pointer position
//			if (pointer) {
//				pointer.transform.position = positionFromCurrentOption();
//			}
		}


		// pause directional input if holding direction
		_allowDirectionalInput = !( Mathf.Abs( _controllingPlayer.horizontal ) > 0.1f || Mathf.Abs( _controllingPlayer.vertical ) > 0.1f);


		// when press action, do the thing of the current menu item
		if (_controllingPlayer.actionPressed) {
			
			playSound(selectSound);

			menuItems[ _currentSelection ].GetComponentInChildren<MenuAction>().Activate();
		}


		// exit option
		if (Input.GetButtonDown( "Quit" )) {
			Application.Quit();
		}
	}

	/// <summary>
	/// Updates the selection.
	/// </summary>
	void updateSelection() {
		_currentItemText = menuItems[ _currentSelection ].GetComponent<Text>();

		_defaultColor = _currentItemText.color;
		_currentItemText.color = selectedColor;

		playSound(moveSound);
	}

	/// <summary>
	/// Plays a sound.
	/// </summary>
	/// <param name="sound">Sound.</param>
	void playSound(AudioClip sound)
	{
		if (_audioSource && sound) {
			_audioSource.PlayOneShot( selectSound );
		}
	}
}
