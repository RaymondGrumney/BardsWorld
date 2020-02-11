using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : Character {

	[Header("Gravity")]

	/// <summary>
	/// Whether to use normal gravity.
	/// </summary>
	[Tooltip("Whether to use normal gravity.")]
	public bool useWorldGravity;

	/// <summary>
	/// The initial gravity scale. This is disabled as long as the blob is sticking to a surface.	
	/// </summary>
	private float _initialGravityScale;	

	/// <summary>
	/// Whether the blob is currently falling (as opposed to sticking or searching for a sticky surface).
	/// </summary>
	private bool dropping = true;


	[Header("Sticking")]

	/// <summary>
	/// How many world units to cast forward to grab a wall on command.
	/// </summary>
	[Tooltip("How many world units to cast forward to grab a wall on command.")]
	public float stickForwardDistance = 1f;

	/// <summary>
	/// How many world units to cast forward to grab a wall when falling.
	/// </summary>
	[Tooltip("How many world units to cast forward to grab a wall when falling.")]
	public float cornerCastDistance = 1f;


	/// <summary>
	/// How large a difference in angle change to tolerate when sticking between surfaces.
	/// </summary>
	[Tooltip("How large a difference in angle change to will tolerate when sticking between surfaces.")]
	[Range(0,360)]
	public float stickyAngle = 70f;

	/// <summary>
	/// The normallized direction of the object the blob is currently sticking to.
	/// </summary>
	private Vector2 _myDown = Vector2.down;


	/// <summary>
	/// The physics layers the Blob can stick to.
	/// </summary>
	[Tooltip("The physics layers the Blob can stick to.")]
	public LayerMask stickyLayers;			// TODO: can this be simplified? Currently set to "Ladder, Default",

	/// <summary>
	/// How many frames to seek corners before falling.
	/// </summary>
	[Tooltip("How many frames to seek corners before falling.")]
	public float findingCornerTime = 1f;

	/// <summary>
	/// How many frames Blob has been seeking a corner after going over an edge.
	/// </summary>
	private float findingCornerCounter = 0;

	/// <summary>
	/// The angle to cast at while looking for a corner after going over an edge.
	/// </summary>
	[Tooltip("The angle to cast at while looking for a corner after going over an edge.")]
	public float findingCornerAngle = 45;

	[Header("Sprite")]

	/// <summary>
	/// The object the sprite is attached to.
	/// </summary>
	[Tooltip("The object the sprite is attached to.")]
	public GameObject spriteObj;

	/// <summary>
	/// The sprite rotation.
	/// </summary>
	private Vector3 _spriteRotation;

	/// <summary>
	/// The collider.
	/// </summary>
	private Collider2D _collider;

	/// <summary>
	/// The climbLadder script on this object.
	/// </summary>
	private ClimbLadder _climbLadder;


	//
	void Start() {
		// get protected variables from parent class
		base.startRoutine();							

		// initialize private variables
		_initialGravityScale = _rigidbody.gravityScale;
		_collider = GetComponent<Collider2D>();
		_climbLadder = GetComponent<ClimbLadder>();

		_spriteRenderer = spriteObj.GetComponent<SpriteRenderer>();

		_animator = GetComponentInChildren<Animator>();
	}


	// Update is called once per frame
	void Update () {

		//TODO: write an initializer thing that runs awake/start stuff in order or something
		if (_animator == null) {
			_animator = GetComponentInChildren<Animator>();
		}

		// look for corners
		whenFallingOffCorners();

		if ( isActive ) {
			// run parent class' update processes
			standardUpdateActions();				

			// check if user is trying to stick to a new surface
			stickOnCommand();						

			// if trying to move
			if ( receivingInput && canMove ) {
				checkMovement();
			}
		}
			

//		Debug.DrawRay( this.transform.position, -_myDown, Color.black, 0.01f );

	}


	void stickDown()
	{
		// cast "downward" to see if we've gone over a corner
		RaycastHit2D hit = Physics2D.Raycast( this.transform.position, _myDown, _collider.bounds.extents.x, stickyLayers );
//		Debug.DrawRay( this.transform.position, _myDown * _collider.bounds.extents.x * 1.1f, Color.blue, 0.1f );

		// is the blob is not contacting a collider
		if (hit.collider != null) {
			stick( hit.normal );
		}
	}


	/// <summary>
	/// Check if falling off a corner .
	/// </summary>
	void whenFallingOffCorners()
	{
		if (!useWorldGravity) {
			if ( noContacts() && !dropping ) {
				
				findCorner();

				// drop if blob has cast 
				if ( findingCornerCounter >= findingCornerTime ) {
					tryDrop();
				}
			}
			else {
				// reset counter
				findingCornerCounter = 0;
			}
		}
	}


	/// <summary>
	/// Finds the corner.
	/// </summary>
	void findCorner() {
		
		// if we're not on a ladder or dropping
		if ( !dropping && this.gameObject.layer != LayerMask.GetMask("Ladder")) {

			// cast "downward" to see if we've gone over a corner
			RaycastHit2D down = Physics2D.Raycast( this.transform.position, _myDown, _collider.bounds.extents.x, stickyLayers );

			// is the blob is not contacting a collider
			if (down.collider == null) {
				findingCornerCounter++;


				// calculate the angle to cast at
				float moveAngle =  MyUtilities.AngleInDegrees( Vector2.zero, _rigidbody.velocity );
				float downAngle =  MyUtilities.AngleInDegrees( Vector2.zero, _myDown );

					
//				float angleSign = 1;
				float angleSign = -getInput();
				float castAngle = moveAngle + ( findingCornerAngle * angleSign );

				// the direction as a vector based on 
				Vector2 castDirection = MyUtilities.NormalizedVectorFromAngle( castAngle );

				// cast
				RaycastHit2D hit2 = Physics2D.Raycast( this.transform.position + _facing, castDirection, cornerCastDistance, stickyLayers );

				// NOTE: Apparently && and || DOES short circuit the comparison???
				if (hit2.collider != null) {
					if (isSticky( hit2.collider.gameObject )) {
						moveToStickiedSpot( hit2.point + ( hit2.normal * _collider.bounds.extents.x * 0.9f ) );
						stick( hit2.normal );
					}
				}
			}
		}
	}


	/// <summary>
	/// Moves to stickied spot.
	/// </summary>
	/// <param name="target">Target.</param>
	void moveToStickiedSpot( Vector3 target )
	{
		_rigidbody.MovePosition( target );
	}


	/// <summary>
	/// Move this instance.
	/// </summary>
	void move() {
		if ( !dropping ) {

			Vector3 normalizedMoveDirection = calcNormalizedMoveDirection();


			// check if using h or v
			float userInput = getInput();


			// flip sprite 
			if (Mathf.Abs( _facing.x ) > 0.1f) {
				_spriteRenderer.flipX = ( _facing.x * _myDown.y ) > 0.1f;
			} else {
				_spriteRenderer.flipX = ( _facing.y * -_myDown.x ) > 0.1f;
			}


			// if moving
			if (Mathf.Abs( userInput ) > 0.1f) {
				_moveVector += ( (Vector2) normalizedMoveDirection * maxSpeed);
//				_rigidbody.velocity = _moveVector + ( (Vector2) normalizedMoveDirection * maxSpeed);
//				_moveVector = Vector2.zero;
				_facing = ( normalizedMoveDirection ).normalized;
			} else if ( _climbLadder.isOnLadder ) { 

				// TODO: Is there a way to excise the ladderClimb script references here?

				if (_grounded) {
					_animator.SetTrigger( "Climbing" );
					_grounded = false;
					_animator.SetBool( "Grounded", false );
				}

				_moveVector += ( new Vector2( Joypad.Read.Buttons.horizontal  * maxSpeed, Joypad.Read.Buttons.vertical * _climbLadder.climbSpeed ) );
//				_rigidbody.velocity = _moveVector + new Vector2( Joypad.Read.Buttons.horizontal  * maxSpeed, Joypad.Read.Buttons.vertical * _climbLadder.climbSpeed );
//				_moveVector = Vector2.zero;

			} else {
//				stop();
			}
		}
	}


	public void addToMoveVector( Vector2 newVector ) {
		_moveVector += newVector;

		// gravity
	}

	public override void stop()
	{
		if( !dropping) {
			_rigidbody.velocity = _moveVector;
			_moveVector = Vector2.zero;
		}
	}

	/// <summary>
	/// Gets user input from the vertical / horizontal axis based on the angle of the surface blob is on
	/// </summary>
	/// <returns>The input.</returns>
	private float getInput() {
		if (inputCheck() && canMove) {
			// between 45 & -45, 135 & -135 = h, else v
			// between 45 & 315, 135 & 225

			float myAngle = MyUtilities.AngleInDegrees( Vector2.zero, _myDown );
			float multiplier = 1;

			if (myAngle >= 45 && myAngle <= 225) {
				multiplier = -1;
			}

			// if angle is +/- 45 degrees from 0 or 180, return vertical axis
			if (myAngle <= 45 && myAngle >= 0 || myAngle <= 360 && myAngle >= 315
			    || myAngle >= 135 && myAngle <= 225) {

				// return the vertical axis
				return Joypad.Read.Buttons.vertical * multiplier;
			} else {
				// return the vertical axis

				return Joypad.Read.Buttons.horizontal * multiplier;
			}
		} else {
			return 0;
		}
	}


	/// <summary>
	/// Rotates the sprite.
	/// </summary>
	void rotateSprite()
	{
		float zAngle = MyUtilities.AngleInDegrees( Vector2.zero, _myDown );
		spriteObj.transform.eulerAngles = new Vector3( 0, 0, zAngle + 90 );
	}


	/// <summary>
	/// Calculates the move direction.
	/// </summary>
	/// <returns>The move direction.</returns>
	Vector3 calcNormalizedMoveDirection() {

		float downAngle = MyUtilities.AngleInDegrees( Vector2.zero, _myDown );
		float moveAngle = ( downAngle + ( 90 * getInput() ) );
		Vector3 moveDir = MyUtilities.NormalizedVectorFromAngle( moveAngle );

		return moveDir;
	}


	/// <summary>
	/// Checks if stick player is attempting to stick or detach to/from a surface
	/// </summary>
	void stickOnCommand() {
		float h = Joypad.Read.Buttons.horizontal;		// the horizontal axis
		float v = Joypad.Read.Buttons.vertical;		// the vertical axis

		// use jump to stick to walls / ceilings
		if( Joypad.Read.Buttons.jump ) {
			if (Mathf.Abs( h ) > 0.3f || Mathf.Abs( v ) > 0.3f) {
				stickForward();
			} else if( MyUtilities.Between( MyUtilities.AngleInDegrees( -_myDown ), 180, 360) ) {
				Vector2 away = -_myDown;

				tryDrop();

				_rigidbody.velocity = away;
			}
		}
	}


	/// <summary>
	/// If the given angle is between two two given angles
	/// </summary>
	/// <returns>True if it is, false if it is outside the angles.</returns>
	/// <param name="angle">Angle tested.</param>
	/// <param name="min">Minimum angle.</param>
	/// <param name="max">Maximum angle.</param>
	bool withinAngle( float angle, float min, float max) {
		if( max < min ) {
			
			return angle <= min && angle <= 0f || angle >= max && angle <= 360f;
		} else {
		
			return angle >= min && angle <= max;
		}
	}


	/// <summary>
	/// Checks if able to drop from current surface
	/// </summary>
	void tryDrop() {

		Debug.Log("         MyUtilities.AngleInDegrees( _myDown ): " + MyUtilities.AngleInDegrees( _myDown ));
		Debug.Log("MyUtilities.AngleInDegrees(Vector3.down) - 85f: " + ( MyUtilities.AngleInDegrees(Vector3.down) - 85f));
		Debug.Log("MyUtilities.AngleInDegrees(Vector3.down) + 85f: " + ( MyUtilities.AngleInDegrees(Vector3.down) + 85f));
			
		if ( !withinAngle( MyUtilities.AngleInDegrees( _myDown ), 
				MyUtilities.AngleInDegrees(Vector3.down) - 91f, 
				MyUtilities.AngleInDegrees(Vector3.down) + 91f)) {

			drop();
		}
	}

	/// <summary>
	/// drop from ceilings / walls 
	/// </summary>
	void drop()
	{
		_rigidbody.gravityScale = _initialGravityScale;
		_myDown = Vector2.down;
		setDropping( true );
	}


	/// <summary>
	/// Sticks to the direction of the specified normal
	/// </summary>
	/// <param name="normal">Normal.</param>
	void stick(Vector3 normal) {
		
		_myDown = -normal;
		_rigidbody.gravityScale = 0;
		_rigidbody.angularVelocity = 0;

		// TODO: Flip the sprite properly here

		setDropping( false );

		stop();
	}

	/// <summary>
	/// Sets dropping and associated actions
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	void setDropping(bool state ) {

		dropping = state;
		useWorldGravity = state; 	
		_grounded = !state;

		_animator.SetBool( "Dropping", state );
		_animator.SetBool( "Grounded", !state );

		rotateSprite();
	}


	/// <summary>
	/// checks if not contacting another collider
	/// </summary>
	/// <returns><c>true</c>, if not in contact with any other objects, <c>false</c> otherwise.</returns>
	private bool noContacts() {
		ContactPoint2D[] contacts = new ContactPoint2D[1];
		
		_collider.GetContacts( contacts );

		return contacts[ 0 ].collider == null;
	}


	/// <summary>
	/// Sets _dropping
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
//	public void setDrop(bool state) {
//		if (state) {
//			drop();
//		}
//	}


	/// <summary>
	/// Changes the myGravity based by detecting the colider in the desired direction based on facing / player input
	/// </summary>
	void stickForward() {

		// current input
		float h = Joypad.Read.Buttons.horizontal;						// horizontal axis input
		float v = Joypad.Read.Buttons.vertical;						// vertical axis input

		Vector2 castDirection = new Vector2( h, v );

		// cast direction player is pressing
		RaycastHit2D hit = Physics2D.Raycast( this.transform.position, castDirection, stickForwardDistance, stickyLayers );
//		Debug.DrawRay( this.transform.position, castDirection * stickForwardDistance, Color.white, Time.deltaTime );

		if (hit.collider != null) {
			if (isSticky( hit.collider.gameObject )) {

				stick( hit.normal );

				moveToStickiedSpot( hit.point );
			}
		}
	}


	// When this object collides with another
	void OnCollisionEnter2D(Collision2D other) {
		// if the other surface is sticky
		if ( isSticky( other.gameObject ) ) {

			Vector3 normal = other.contacts[ 0 ].normal;

			if ( dropping || withinStickyAngle( normal  )) {
				stick( normal );
			}
		}

		// if the Blob is in free-fall
		if( useWorldGravity && other.contacts.Length > 0) {
			stick( other.contacts[ 0 ].normal );
		}
	}


	/// <summary>
	/// If direction is within the allowed sticky angle
	/// </summary>
	/// <returns><c>true</c>, if normal is within sticky margin, <c>false</c> otherwise.</returns>
	/// <param name="normal">Normal.</param>
	bool withinStickyAngle( Vector3 normal ) { 
		
		float normalAngle = MyUtilities.AngleInDegrees( Vector2.zero, -normal );
		float downAngle = MyUtilities.AngleInDegrees( Vector2.zero, _myDown );
		float difference = Mathf.Abs( normalAngle - downAngle );

		return difference <= stickyAngle;
	}


	/// <summary>
	/// Checks if blob can move, and does
	/// </summary>
	private void checkMovement() {
		if ( inputCheck() && canMove ) {
			move();
		}
	}

	/// <summary>
	/// Throw this instance.
	/// </summary>
	public void thrown() {
		drop();
	}

	/// <summary>
	/// Pickup this instance.
	/// </summary>
	public void pickup() {
		drop();
	}


	/// <summary>
	/// If the passed GameObject is sticky
	/// </summary>
	/// <returns><c>true</c>, if sticky was ised, <c>false</c> otherwise.</returns>
	/// <param name="other">Other.</param>
	private bool isSticky( GameObject other ) {
		Attributes attributes = other.gameObject.GetComponent<Attributes>();

		// if we can stick to the other component
		if (attributes) {
			return attributes.sticky;
		}

		return false;
	}

	protected override void MoveByVector(){

		if (!dropping) {
			// modify velocity
			_rigidbody.velocity = _moveVector;
		} else {
			_rigidbody.velocity += _moveVector;
		}

		// reset vector to 0
		_moveVector = Vector2.zero;
	}
}
