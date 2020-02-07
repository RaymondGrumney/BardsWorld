using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : BaseEnemy {

	[Header("Bat AI Settings")]
	[Tooltip("How high we fly.")]
	public float flyHeight;
	[Tooltip("Our upward velocity.")]
	public float flightStrength;
	[Tooltip("The Bat's gravity.")]
	public float flightGravity;
	[Tooltip("The bat's maximum falling velocity.")]
	[Range(0f, -9.8f)]
	public float fallingVelocity;
	[Tooltip("Whether the bat follows the character.")]
	public bool homesOnCharacter;

	// the sign (+/-) of the position of the character the bat spotted (used to see if we've passed that character by)
	private float _seenSign;

	// whether the bat has forgotten the character it saw
	private bool _forgot;
	// whether the bat is hiding
	private bool _hiding;

	[Tooltip("Length of time the Bat remains active before fleeing (in seconds, 0 = infinity).")]
	public float awakeTime;
	// when the bat saw the character
	private float _timeAwakened;

	void Start() {
		initializationRoutine();
	}


	protected override bool forgetBehavior() {
		
		return _forgot;
	}

	/// <summary>
	/// Raises the collision enter2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnCollisionEnter2D(Collision2D other) {

		// when the bat hits the player
		if (other.gameObject.tag == "Player") {
			if (!_hiding) {

				// forget the character
				forgetPlayer();

				// face away
				setFacing( this.transform.position.x - other.transform.position.x );

				// and away
				_rigidbody.velocity = facing * speed;
			} else {

				_hiding = false;
				setTarget( other.gameObject );
			}
		} else {

			setFacing( this.transform.position.x - other.transform.position.x );
			_pursuing = false;
			_rigidbody.gravityScale = -flightGravity;
		}
	}

	private void forgetPlayer()
	{
		// the bat forgets the character when it hits them
		_forgot = true;
		_hiding = true;
		_pursuing = false;
		// time to fly up
		_rigidbody.gravityScale = -flightGravity;
		// and the other direction
	}

	/// <summary>
	/// The AI's default behavior.
	/// </summary>
	protected override void defaultBehavior() {
	// this is where the bat retreats while hiding
		if (_hiding) {

			RaycastHit2D hit = Physics2D.Raycast( this.transform.position, Vector2.up, _collider.bounds.extents.y * 1.1f, _physicsLayer );

			// if the bat is not on the ceiling
			if (hit == null) {
				// go up
				return;
			}

			moveForward();

		}
	}

	/// <summary>
	/// The AI's idle behavior
	/// </summary>
	protected override void idleBehavior() {
		// do nothing
	}		

	/// <summary>
	/// the AI's pursuit.
	/// </summary>
	protected override void pursuitBehavior() {
		if ( _pursuing ) {

			// make sure gravity is turned on 
			_rigidbody.gravityScale = flightGravity;


			// turn to face the character if
			if (homesOnCharacter) {
				faceTarget();
			} else if( Mathf.Sign( ( _lastCharacterSeen.transform.position - transform.position ).x ) != _seenSign ) {
				// forget the character
				forgetPlayer();
			}

			// x is constant
			moveForward();


			if (_rigidbody.velocity.y < fallingVelocity) {
				_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, fallingVelocity);
			}

			// drop
			// add force based on the distance from the ground
			RaycastHit2D hit = Physics2D.Raycast( this.transform.position, Vector2.down, 100f, _physicsLayer );
			float fromFlyHeight = hit.distance - flyHeight;
			float diffHeightFromPlayer = this.transform.position.y - _lastCharacterSeen.transform.position.y;

			if (diffHeightFromPlayer < 0) {
				_rigidbody.AddForce( Vector2.up * - diffHeightFromPlayer * flightStrength );
			}

			_rigidbody.AddForce( Vector2.up * flightStrength / fromFlyHeight );


			if (awakeTime > 0) {
				if (_timeAwakened + awakeTime < Time.time) {
					forgetPlayer();
				}
			}
		}
	}

	/// <summary>
	/// Sets the target.
	/// </summary>
	/// <param name="target">Target.</param>
	public override void setTarget(GameObject target) {
		if(! _pursuing) {
			_pursuing = true;
			_lastCharacterSeen = target;
			_targetPoint = target.transform.position;
			_lastSawCharacter = Time.time + timeToForget;

			faceTarget();

			_seenSign = Mathf.Sign( ( _lastCharacterSeen.transform.position - transform.position ).x );

			_timeAwakened = Time.time;
		}
	}
}
