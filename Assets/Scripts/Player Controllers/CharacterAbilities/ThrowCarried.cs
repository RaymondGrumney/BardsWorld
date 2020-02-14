using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCarried : GrabAndCarry {

	/// <summary>
	/// The angle the character throws at.
	/// </summary>
	[Tooltip("The angle the character throws at.")]
	public float throwAngle = 40f;

	/// <summary>
	/// The force the character throws at.
	/// </summary>
	[Tooltip("The force the character throws at.")]
	public float throwForce;

	/// <summary>
	/// The angular force applied when throwing objects.
	/// </summary>
	[Tooltip("The angular force applied when throwing objects.")]
	public float throwAngularForce;

	/// <summary>
	/// If the character can throw while jumping.
	/// </summary>
	[Tooltip("If the character can throw while jumping.")]
	public bool canThrowWhileJumping = false;

	/// <summary>
	/// The object we use to draw the throw trace.
	/// </summary>
	[Tooltip("The object we use to draw the throw trace.")]
	public GameObject throwTrace;

	/// <summary>
	/// How many seconds we hold the action button to enter throw event.
	/// </summary>
	[Tooltip("How many seconds we hold the action button to enter throw event.")]
	public float timeActionHeldToThrow;

	/// <summary>
	/// The speed at which the angle is adjusted by the player.
	/// </summary>
	[Tooltip("The speed at which the angle is adjusted by the player.")]
	public float angleAdjustSpeed;

	/// <summary>
	/// The number of vertices in the trajectory.
	/// </summary>
	[Tooltip("The number of vertices in the trajectory.")]
	public int drawDistance = 40;

	/// <summary>
	/// Which traces to draw.
	/// </summary>
	[Tooltip("Which traces to draw")]
	public int drawOnlyEvery = 1;

	/// <summary>
	/// fixes the length of the trajectory.
	/// </summary>
	static private float TRAJECTOR_VELOCITY_MULTIPLIER = 1.2f; // HACK: There is something wrong since this is needed.

	/// <summary>
	/// How long the player has held the action button.
	/// </summary>
	private float actionHeldTime;

	/// <summary>
	/// The current throw angle.
	/// </summary>
	private float _currentThrowAngle;

	/// <summary>
	/// The current throw force.
	/// </summary>
	private float _currentThrowForce;

	/// <summary>
	/// If the character can currently throw.
	/// </summary>
	private bool _canThrow = false;

	// this character's rigidbody
	/// <summary>
	/// This character's rigidbody.
	/// </summary>
	private Rigidbody2D _rigidbody;

	// initialize
	void Awake() {
		base.onAwake();

		_currentThrowAngle = throwAngle;
		_currentThrowForce = throwForce;
		_rigidbody = _character.GetComponent<Rigidbody2D>();
	}
		
	// overriding parent Update method
	protected override void LateUpdate() {
		
		if ( _character.inputCheck() ) {

			// attempt to pick up objects
			if ( Joypad.Read.Buttons.Held("throw") && _carriedObject == null) {

				GameObject target = getTarget();

				if (target) {
					tryPickUp( target );
				}
			}


			// if the player is pressing the action button after picking up an object
			tryThrow();


			// check if the player has released the action button from picking up an object
			if ( ! _canThrow && _carriedObject ) {
				_canThrow = Joypad.Read.Buttons.Released("throw");

			} 
		}

		countHowLongActionHeld();

		moveCarried();
	}

	/// <summary>
	/// If the player presses the action button, throw the button th
	/// </summary>
	void tryThrow()
	{
		if ( _canThrow && _carriedObject ) {
			
			// if either can throw while jumping or grounded
			if ( canThrowWhileJumping || _character.grounded ) {
				
				// when the player releases the action button
				if (Joypad.Read.Buttons.Released("action")) {

					// drop item if we didn't hold for throw
					if (actionHeldTime < timeActionHeldToThrow) {
						_currentThrowAngle = dropAngle;
						_currentThrowForce = dropForce;
					}


					// throw object
					throwObject( _currentThrowAngle, _currentThrowForce );



					// reset current throw angle
					_currentThrowAngle = throwAngle;
					_currentThrowForce = throwForce;

				// otherwise if the player is holding the action button
				} else if (Joypad.Read.Buttons.Held("action")) {

					// if the player has held the action button long enough and is grounded
					if (actionHeldTime > timeActionHeldToThrow && _character.grounded) {
						_rigidbody.velocity = Vector2.zero;

						drawAndModifyThrowTrajecory();
					}
				}
			}
		}
	}

	/// <summary>
	/// Counts how long the player has held the action button.
	/// </summary>
	void countHowLongActionHeld()
	{
		if ( Joypad.Read.Buttons.Held("throw") ) {
			// tally how long player has held action
			actionHeldTime += Time.deltaTime;
		} else {

			// reset held counter
			actionHeldTime = 0;
		}
	}

	/// <summary>
	/// Basic throw.
	/// </summary>
	void throwObject( float angle, float force )
	{
		
		// throw / drop item
		_canThrow = false;

		// allow player mvoement again
		_character.CanMove = true;

		_carriedObject.GetComponent<Rigidbody2D>().angularVelocity = throwAngularForce * -_character.facing.x;

		// throw the object at the passed angle
		dropCarried( angle, force );


		// reset _currentThrowAngle
//		_currentThrowAngle = throwAngle;
//		_currentThrowForce = throwForce;
	}

	/// <summary>
	/// Draws and modified throw trajecory.
	/// </summary>
	void drawAndModifyThrowTrajecory()
	{
		// prevent character movement
		_character.CanMove = false;

		// angle and facing
		float facedThrowAngle = _currentThrowAngle;		// default to current throw angle
		float facing = _character.facing.x;

		// reverse if facing left
		if (facing < 0) {
			facedThrowAngle = Mathf.Abs(facedThrowAngle - 180);
		}


		// draw path
		Collider2D collider = _carriedObject.GetComponent<Collider2D>();
		Vector3 startPos = _carryPosition.position;
		Vector2 startVelocity = MyUtilities.NormalizedVectorFromAngle( facedThrowAngle ) * throwForce;

		DrawTrajectory( startPos, startVelocity );

		// adjust path with left / right / up /  down input
		adjustAngle();

	}

	public override bool give(GameObject gameObject) {

		if (!carrying) {
			_canThrow = tryPickUp( gameObject );

			return _canThrow;
		} else {
			return false;
		}

	}

	void DrawTrajectory(Vector3 startPos, Vector2 startVelocity){ 
		// based on: https://answers.unity.com/questions/606720/drawing-projectile-trajectory.html

		Rigidbody2D rigidbody = _carriedObject.GetComponent<Rigidbody2D>();
		Vector3 position = startPos;
		Vector2 velocity = startVelocity;
		Vector2 gravity = new Vector2( Physics2D.gravity.x, Physics2D.gravity.y ) * rigidbody.gravityScale;
		float drag = rigidbody.drag;

		for( var i = 0; i < drawDistance; i++ ) {
			if( i % drawOnlyEvery == 0) {
				
				// draw the traces
				Instantiate( throwTrace, new Vector3( position.x, position.y, position.z), Quaternion.identity, this.transform );
			}

			// calculate next position
			velocity = ( velocity * ( 1 - Time.fixedDeltaTime * drag ) ) + gravity * Time.fixedDeltaTime;
//			velocity = velocity + gravity * Time.fixedDeltaTime * TRAJECTOR_VELOCITY_MULTIPLIER ;
			position = position + (Vector3) velocity * ( Time.fixedDeltaTime );
		}

	}

	/// <summary>
	/// Adjusts the angle based on user input
	/// </summary>
	void adjustAngle() {
		// adjust by player input
		_currentThrowAngle -= Joypad.Read.Buttons.horizontal * angleAdjustSpeed * _character.facing.x;

		_currentThrowAngle += Joypad.Read.Buttons.vertical * angleAdjustSpeed * _character.facing.x;
	}
}
