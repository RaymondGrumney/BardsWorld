using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAndCarry : MonoBehaviour {

	[Tooltip("The angle of the drop in degrees.")]
	/// <summary>
	/// The angle of the drop in degrees.
	/// </summary>
	public float dropAngle;

	[Tooltip("The force with which the character throws objects.")]
	/// <summary>
	/// The force with which the character throws objects.
	/// </summary>
	public float dropForce;

	[Tooltip("The distance the character grabs objects from.")]
	/// <summary>
	/// The distance the character grabs objects from.
	/// </summary>
	public float grabDistance;

	/// <summary>
	/// The character this script is attached to.
	/// </summary>
	protected Character _character;

	/// <summary>
	/// The position objects are carried at.
	/// </summary>
	protected Transform _carryPosition;

	protected bool flipCarryPositionX = false;

	/// <summary>
	/// The currently carried object.
	/// </summary>
	protected GameObject _carriedObject = null;	

	protected ClimbLadder _climbLadder;

	/// <summary>
	/// The carried object sprite renderer.
	/// </summary>
	protected SpriteRenderer _carriedObjectSpriteRenderer;

	/// <summary>
	/// The character object's collider.
	/// </summary>
	protected Collider2D _collider;

	/// <summary>
	/// Whether this <see cref="GrabAndCarry"/> is carrying an object.
	/// </summary>
	/// <value><c>true</c> if carrying; otherwise, <c>false</c>.</value>
	public bool carrying {
		get{ return _carriedObject != null; }
	}



	// Use this for initialization
	void Awake() {
		// gets private/protected variables

		onAwake();
	}

	/// <summary>
	/// The routine run at start
	/// </summary>
	protected void onAwake()
	{
		// find character script and carry position
		_character = GetComponent<Character>();
		_carryPosition = transform.Find("GrabAndCarryPosition");
		_collider = _character.GetComponent<Collider2D>();
		_climbLadder = _character.GetComponent<ClimbLadder>();
	}

	
	// Update is called once per frame
	protected virtual void LateUpdate() {

		// attempt to pick up objects
		if ( _character.inputCheck() && Joypad.Read.Buttons.Held("pickup") ) {
			
			// if not carrying an object
			if (_carriedObject == null) {
				
				GameObject target = getTarget();

				if (target) {
					tryPickUp( target );
				}

			// if you are carrying an object
			} else {
				dropCarried( dropAngle, dropForce ); 
			}
		}

		// move any carried objects with you
		moveCarried();

	}

	/// <summary>
	/// Gets the target.
	/// </summary>
	/// <returns>The target.</returns>
	protected GameObject getTarget() {

		GameObject target = null;
		RaycastHit2D[] hits = new RaycastHit2D[1];

		_collider.Cast( _character.facing, new ContactFilter2D(), hits, grabDistance);		// cast the player's Collider forward and store it in hits

		if ( hits[0] )  {
			target = hits[0].transform.gameObject; 	// get GameObject 1 game unit ahead of you
		}

		return target;
	}

	/// <summary>
	/// Picks up the target if carriable
	/// </summary>
	/// <param name="target">Target.</param>
	protected bool tryPickUp(GameObject target) {
		Attributes targetAttributes = target.GetComponent<Attributes>();

		if ( targetAttributes ) {
			if (targetAttributes.carriable && !carrying && !_climbLadder.isOnLadder) {
				pickUp( target );
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Picks up the target
	/// </summary>
	/// <param name="target">Target.</param>
	protected void pickUp (GameObject target) {

		Blob blob = target.GetComponent<Blob>();

		if (blob) {
			blob.pickup();
		}

		// assign carriedObject
		_carriedObject = target;

		// turn of its colliders
		setColliders(target, false);

		// play animation for picking up object
		_character.animator.SetTrigger( "Pick Up" );

		// get the sprite renderer for the object
		_carriedObjectSpriteRenderer = target.GetComponent<SpriteRenderer>();

		// set postion of object
		moveCarried();
	}

	public virtual bool give(GameObject gameObject) {
		return tryPickUp( gameObject );
	}

	/// <summary>
	/// Moves the carried object along with the character.
	/// </summary>
	protected void moveCarried() {

		if (_carriedObject) {

			_carriedObjectSpriteRenderer.flipX = Joypad.Read.Buttons.horizontal < -0.1f;

			checkXFlip();

			// set position
			_carriedObject.transform.position = _carryPosition.position;

			// flip the x position
			if (flipCarryPositionX) {
				float x = _character.transform.position.x - _carryPosition.localPosition.x;

				_carriedObject.transform.position = new Vector3( 	x, 
																	_carryPosition.position.y, 
																	_carryPosition.position.z 
																);
			}
		}
	}


	/// <summary>
	/// Checks if the x position of the needs to be flipped.
	/// </summary>
	protected void checkXFlip() {
		if(flipCarryPositionX != _character.facing.x < -0.1f) {
 			flipCarryPositionX = _character.facing.x < -0.1f;
		}
	}


	/// <summary>
	/// Drops the carried object, with or without an input timeout for the character.
	/// </summary>
	protected void dropCarried(float angle, float force, bool inputTimesOut) {

		// if the object is a throwable object, trigger beenThrown()
		Throwable throwable = _carriedObject.GetComponent<Throwable>();

		if (throwable) {
			throwable.beenThrown();
		}


		// turn collider back on
		setColliders( _carriedObject, true );

		// play animation and prevent movement momentarily
		_character.animator.SetTrigger("DropItem");

		if (inputTimesOut) {
			_character.inputTimeout();
		}

		// set parent of the object
		_carriedObject.transform.SetParent(this.transform.parent);

		_carriedObjectSpriteRenderer = null;

		// adjust angle based on facing
		float facedThrowAngle = angle;
		float facing = _character.facing.x;

		if (facing < 0) {
			facedThrowAngle = Mathf.Abs(facedThrowAngle - 180);
		}


		// the normalized vector of the corrected throw direction
		Vector2 throwVector = MyUtilities.NormalizedVectorFromAngle( facedThrowAngle );


		// throw the object
		Rigidbody2D rigidbody = _carriedObject.GetComponent<Rigidbody2D>();
		rigidbody.velocity = throwVector * force;
		rigidbody.angularVelocity = force;



//
//		// special case for throwing the Blob
//		Blob blob = GetComponent<Blob>();
//
//		if (blob != null) {
//			blob.thrown();
//		}


		// clear the variable
		_carriedObject = null;
	}

	/// <summary>
	/// Drops the carried object.
	/// </summary>
	protected void dropCarried(float angle, float force) {
		dropCarried( angle, force, true );
	}

	public void drop() {
		dropCarried( dropAngle, dropForce, false );
	}

	/// <summary>
	/// Sets the colliders of the sent game object
	/// </summary>
	/// <param name="gObj">G object.</param>
	/// <param name="b">If set to <c>true</c> b.</param>
	public void setColliders(GameObject gameObject, bool state) {
		
		Collider2D[] colliders = gameObject.GetComponents<Collider2D>();

		for( int i = 0; i < colliders.Length; i++ ) {
			colliders[ i ].enabled = state;
			colliders[ i ].attachedRigidbody.simulated = state;
		}
	}
}
