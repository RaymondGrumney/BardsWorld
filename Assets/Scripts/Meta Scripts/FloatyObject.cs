using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyObject : MonoBehaviour {

	[Header("Objects")]
	[Tooltip("The top wing object.")]
	public GameObject topObj;

	[Tooltip("The bottom wing object.")]
	public GameObject bottomObj;


	[Header("Numbers")]
	[Tooltip("The distance the wings bounce.")]
	public float bounciness = 1f;

	[Tooltip("The tolerance at which the object reverses direction.")]
	public float topBounce = 0.001f;

	[Tooltip("The speed at which the object floats")]
	public float speed = 1f;

	[Tooltip("The highest speed the object can move")]
	public float maxSpeed = 3f;


	// the direction the wings are moving currently
	private int moveDirection = 1;

	// the character script attached to this object
	private Character _character;

	// the initial y position of this object
	private float _initialYPosition;

	// the x offset of this object
	private float _xOffset;

	// the sprite renderer of the top wing
	private SpriteRenderer _topWing;
	private Vector2 _topWingInitialPosition;

	// the sprite renderer of the bottom wing
	private SpriteRenderer _bottomWing;
	private Vector2 _bottomWingInitialPosition;


	// Use this for initialization
	void Start () {
		initializePrivateVariables();
	}

	/// <summary>
	/// Initializes the private variables.
	/// </summary>
	void initializePrivateVariables()
	{
		_character = GetComponentInParent<Character>();

		// local xOffset & y position
		_initialYPosition = transform.position.y;
		_xOffset = transform.position.x;

		// top wing components
		_topWing = topObj.GetComponent<SpriteRenderer>();
		_topWingInitialPosition = topObj.transform.position;

		// bottom wing components
		_bottomWing = bottomObj.GetComponent<SpriteRenderer>();
		_bottomWingInitialPosition = bottomObj.transform.position;
	}

	// Update is called once per frame
	void Update () {

		float move = calculateMove();

		// set positions
		setPositions( move );
		flipSprites();
	}
		
	/// <summary>
	/// Calculates the movement for this frame
	/// </summary>
	/// <returns>The move.</returns>
	protected float calculateMove()
	{
		
		// calculate the difference between current and initial position's x coordinate
		float absPositionDifference = Mathf.Abs( _initialYPosition - transform.position.y );


		// toggle multiplier if at top/bottom of float cycle
		if ( absPositionDifference > bounciness ) {
			moveDirection *= -1;
		}

		// calculate movement
		float move = speed * moveDirection;

		// as long as it won't divide by 0
		if ( absPositionDifference != 0 ) {

			// calculate a modifier based on distance from initial position, slowing as it approaches the min / max height
			float moveModifier = Mathf.Clamp( bounciness / absPositionDifference, 1, maxSpeed);

			move *= moveModifier;
		}

		return move;
	}

	/// <summary>
	/// Sets the positions.
	/// </summary>
	/// <param name="move">Move.</param>
	protected void setPositions( float move )
	{
		float multiplier = 1;

		if (_character) { 
			multiplier = _character.facing.x;
		}
		
		transform.position = new Vector2( _xOffset * multiplier, transform.position.y + move );
		topObj.transform.position = new Vector2( _topWingInitialPosition.x * multiplier, _topWingInitialPosition.y );
		bottomObj.transform.position = new Vector2( _bottomWingInitialPosition.x * multiplier, _bottomWingInitialPosition.y );
	}

	/// <summary>
	/// Flips the sprites based on facing
	/// </summary>
	protected void flipSprites()
	{
		if (_character) {
			// flip sprites if character facing
			_topWing.flipX = !( _character.facing.x < 0.1f );
			_bottomWing.flipX = !( _character.facing.x < 0.1f );
		}
	}
}
