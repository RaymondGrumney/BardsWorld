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
	public int brokenHeartPieceSize;

	public float pieceFadeOutTime = 0.01f;

	void OnValidate() {
		if(brokenHeartPieceSize < 1) {
			brokenHeartPieceSize = 1;
		}

		if(pieceFadeOutTime < 0) {
			brokenHeartPieceSize = 0;
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
	void Start () {
		// get components
		_character = GetComponentInParent<Character>();
		_takesDamage = GetComponentInParent<TakesDamage>();

		_unbrokenHeartSprite = heartPrefab.GetComponent<SpriteRenderer>().sprite;

		_heartWidth = heartPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

		// set values
		_maxHealth = _takesDamage.maxHealth;
		_currentHealth = _takesDamage.currentLife;

		InitializeHeartDisplays();

		// if for some reason current and max health are not the same, update the display
		AdjustLifeDisplayUp( 0 );	
	}

	// initializes arrays and assigns game objects to them
	void InitializeHeartDisplays() {

		// initialize arrays to length of _maxHealth
//		fullHearts = new SpriteRenderer[_maxHealth];
//		grayHearts = new SpriteRenderer[_maxHealth];
//		emptyHearts = new SpriteRenderer[_maxHealth];

		_hearts = new GameObject[ _maxHealth ];
		_spriteRenderers = new SpriteRenderer[_maxHealth];

//		SpriteRenderer[] childrenSprites = this.gameObject.GetComponentsInChildren< SpriteRenderer >();		// all the children of this object, to be sorted into above arrays

		// amount the first sprite's position is shifted
		float shift = -_maxHealth / 2;

		// for each heart
		for( int i = 0; i < _maxHealth; i++ ) {
			float xPos = shift + ( i * _heartWidth * ( 1 + adjustment ) ) + this.transform.position.x;
			float yPos = this.transform.position.y;
			float zPos = this.transform.position.z;

			Vector3 position = new Vector3( xPos, yPos, zPos);

			// generate hearts
			_hearts[ i ] = Instantiate( heartPrefab, position, Quaternion.identity, this.transform);

			// grab sprite renderer for the heart
			_spriteRenderers[ i ] = _hearts[ i ].GetComponent<SpriteRenderer>();



			// get the current heart game objects and store them in our arrays, hiding them as we go
//			for( int j = 0; j < childrenSprites.Length; j++ ) {
//
//				if (childrenSprites[ j ].gameObject.name == "Full Heart " + ( i + 1 )) {
//					fullHearts[ i ] = childrenSprites[ j ];
//					fullHearts[ i ].color = fullHearts[ i ].color - new Color( 0, 0, 0, 1 );
//				}
//
//				if (childrenSprites[ j ].gameObject.name == "Gray Heart " + ( i + 1 )) {
//					grayHearts[ i ] = childrenSprites[ j ];
//					grayHearts[ i ].color = grayHearts[ i ].color - new Color( 0, 0, 0, 1 );
//				}
//
//				if (childrenSprites[ j ].gameObject.name == "Empty Heart " + ( i + 1 )) {
//					emptyHearts[ i ] = childrenSprites[ j ];
//					emptyHearts[ i ].color = emptyHearts[ i ].color - new Color( 0, 0, 0, 1 );
//				}
//			}
		}
	}


	// Update is called once per frame
	void Update () {
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
	private void updateAlpha() {

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
	private void adjustDisplay( int value ) {
		
		_currentHealth += value;

		// turn on all heart container at or below your current health and off everything above
		for( int i = 0; i < _maxHealth; i++ ) {
			bool state = i < _currentHealth;

//			_hearts[ i ].GetComponent<Heart>().setDamaged( state );

			if (state) {
				
			} else {
				
				BreakUp( _unbrokenHeartSprite, brokenHeartPieceSize, (int) _spriteRenderers[ i ].sprite.pixelsPerUnit, _hearts[ i ].transform.position );

				_hearts[ i ].GetComponent<Heart>().setDamaged( true );

				break;
			}
		}

		show();
	}

	public void AdjustLifeDisplayDown( int value )
	{
		adjustDisplay(-value);
	}

	public void AdjustLifeDisplayUp(int value)
	{
		adjustDisplay(value);
	}

	public void show() {
		fadeOutTime = Time.time + stayTime;
	}

	/// <summary>
	/// Breaks up the sent sprite
	/// </summary>
	/// <param name="sprite">Sprite.</param>
	/// <param name="pieces">pieces.</param>
	/// <param name="pixelsPerUnit">Pixels per unit.</param>
	/// <param name="position">Position.</param>
	void BreakUp(Sprite sprite, int size, int pixelsPerUnit, Vector3 position){

		// Get sprite texture
		Texture2D sourceTexture = sprite.texture; 
		Vector2 pivot = sprite.pivot;
		Rect rect = sprite.rect;
		Debug.Log(sprite.texture.name);

		// Get sprite width and height
		int width = Mathf.FloorToInt( sprite.rect.width );
		int height = Mathf.FloorToInt( sprite.rect.height );


		int partNumber = 0;

		for(int i = 0; i < width / size; i++) {
			for(int j = 0; j < height / size; j++) {
				
				// Cut out the needed part from the texture
				Sprite newSprite = Sprite.Create(	sourceTexture, 
													new Rect( rect.x + i * width / ( width / size ), rect.y + j * height / ( height / size ), size, size), 
													new Vector2(0f, 0f),
													pixelsPerUnit );
				
				GameObject gObject = new GameObject();
				SpriteRenderer sRenderer = gObject.AddComponent<SpriteRenderer>();
				Rigidbody2D rBody = gObject.AddComponent<Rigidbody2D>();
				FadeOut fOut = gObject.AddComponent<FadeOut>();

				fOut.fadeOutTime = pieceFadeOutTime;

				gObject.name = sprite.name + " part " + partNumber;

				sRenderer.sprite = newSprite;
				sRenderer.color = Color.white;
				sRenderer.sortingLayerName = "Local Displays";


				// Place every GameObject as it was in the original sprite
				gObject.transform.position = position + new Vector3(	sprite.bounds.min.x + ( sRenderer.sprite.rect.width / pixelsPerUnit ) * i, 
															sprite.bounds.min.y + ( sRenderer.sprite.rect.width / pixelsPerUnit ) * j, 
															0 );



				// Place the parts inside the parent object
				gObject.transform.parent = transform.parent; 	


				rBody.velocity = new Vector2( Random.Range( -2f , 2f ), Random.Range( 3f, 6f ) );
				rBody.freezeRotation = true;

//				Instantiate( gObject );
			

				partNumber++;
			}
		}
	}
}
