using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDisplay : MonoBehaviour {

	[Tooltip("How long it takes to fade out. Also fade in time.")]
	/// <summary>
	/// How long it takes to fade out. Also fade in time.
	/// </summary>
	public float fadeoutTime = 1;

	[Tooltip("How long the heart display stays after displaying.")]
	/// <summary>
	/// How long the heart display stays after displaying.
	/// </summary>
	public float stayTime = 5;

	[Tooltip("The adjustment of the sprites as a percentage of the width of the sprite.")]
	/// <summary>
	/// The adjustment of the sprites as a percentage of the width of the sprite.
	/// </summary>
	public float adjustment = 0.1f;

	[Tooltip("The prefab for a heart.")]
	/// <summary>
	/// The prefab for a heart.
	/// </summary>
	public GameObject heartPrefab;

	[Tooltip("The size of the broken heart piece.")]
	/// <summary>
	/// The size of the broken heart piece.
	/// </summary>
	public int brokenHeartPiecePieces;

	public float pieceFadeOutTime = 0.01f;

	void OnValidate() 
	{
		if(brokenHeartPiecePieces < 1) {
			brokenHeartPiecePieces = 1;
		}

		if(pieceFadeOutTime < 0) {
			brokenHeartPiecePieces = 0;
		}
	}

	/// <summary>
	/// The Character to which this life display is attached.
	/// </summary>
	private Character _character;

	/// <summary>
	/// The takes damage script of the character.
	/// </summary>
	private TakesDamage _takesDamage;

	// the maximum health of the character
	private int _maxHealth;

	// the current health of the character
	private int _currentHealth;


	// the hearts for this display
	private GameObject[] _hearts;

	private Sprite _unbrokenHeartSprite;


	private SpriteRenderer[] _spriteRenderers;

	// the width of a heart sprite
	private float _heartWidth;

	[Tooltip("How long it takes to fade the display out.")]
	public float fadeOutTime = 5;

	// the current alpha of the spriteRenderers
	private float _alpha = 1;


	// Use this for initialization
	void Start()
	{
		GetComponents();
		ExtractValues();
		InitializeHeartDisplays();

		// if for some reason current and max health are not the same, update the display
		AdjustLifeDisplayUp(0);
	}

	private void ExtractValues()
	{
		_maxHealth = _takesDamage.maxHealth;
		_currentHealth = _takesDamage.currentLife;
	}

	private void GetComponents()
	{
		_character = GetComponentInParent<Character>();
		_takesDamage = GetComponentInParent<TakesDamage>();
		_unbrokenHeartSprite = heartPrefab.GetComponent<SpriteRenderer>().sprite;
		_heartWidth = _unbrokenHeartSprite.bounds.size.x;
	}

	// initializes arrays and assigns game objects to them
	void InitializeHeartDisplays() 
	{
		_hearts = new GameObject[ _maxHealth ];
		_spriteRenderers = new SpriteRenderer[_maxHealth];

		// amount the first sprite's position is shifted
		float shift = -_maxHealth / 2;

		// for each heart
		for( int i = 0; i < _maxHealth; i++ ) 
		{
			float xPos = shift + ( i * _heartWidth * ( 1 + adjustment ) ) + this.transform.position.x;
			float yPos = this.transform.position.y;
			float zPos = this.transform.position.z;

			Vector3 position = new Vector3( xPos, yPos, zPos);

			// generate hearts
			_hearts[ i ] = Easily.Instantiate( heartPrefab, position );
			_hearts[i].transform.SetParent( this.transform );
			// grab sprite renderer for the heart
			_spriteRenderers[ i ] = _hearts[ i ].GetComponent<SpriteRenderer>();
		}
	}


	// Update is called once per frame
	void Update()
	{
		if (fadeOutTime > Time.time) {
			
			// if we are still displaying, haven't entered _fadeOutTime, and alpha is less than 1
			if (fadeOutTime - fadeoutTime > Time.time && _alpha < 1) {
				_alpha += Time.deltaTime / fadeoutTime;									// step up alpha
				updateAlpha();
			}

			// if we are still displaying, have entered _fadeOutTime, and alpha is greater than 0
			if (fadeOutTime - fadeoutTime < Time.time && _alpha > 0) {
				_alpha -= Time.deltaTime / fadeoutTime;	
				updateAlpha();
			}
		} else if (_alpha > 0) {
			_alpha = 0;
			updateAlpha();
		}
	}

	// change the alpha of child sprites to _alpha
	private void updateAlpha() 
	{
		// set the alpha for each sprite in each sprite array to _alpha
		for( int i = 0; i < _maxHealth; i++ ) {
			Color color = _spriteRenderers[ i ].color;
			_spriteRenderers[ i ].color = new Color( color.r, color.g, color.b, _alpha );
		}
	}

	/// <summary>
	/// Adjusts the display.
	/// </summary>
	/// <param name="value">The amount will we adjust the display by.</param>
	private void adjustDisplay( int value ) 
	{	
		_currentHealth += value;

		// turn on all heart container at or below your current health and off everything above
		for( int i = 0; i < _maxHealth; i++ ) {
			bool state = i < _currentHealth;

			if (state) {
				
			} else {

				SpriteEffects.Explode(_unbrokenHeartSprite)
							 .At(_hearts[i].transform.position)
							 .Into(brokenHeartPiecePieces).Pieces();

				_hearts[ i ].GetComponent<Heart>().setDamaged( true );

				break;
			}
		}

		ShowLifeDisplay();
	}

	public void AdjustLifeDisplayDown( int value )
	{
		adjustDisplay(-value);
	}

	public void AdjustLifeDisplayUp( int value )
	{
		adjustDisplay(value);
	}

	public void ShowLifeDisplay()
	{
		fadeOutTime = Time.time + stayTime;
	}

}
