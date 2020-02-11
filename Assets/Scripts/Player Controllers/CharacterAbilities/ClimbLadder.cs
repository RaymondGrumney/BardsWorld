using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A character's ability to climb a ladder
/// </summary>
public class ClimbLadder : MonoBehaviour {

	/// <summary>
	/// The character script for this character.
	/// </summary>
	private Character _character;

	/// <summary>
	/// The speed at which the character climbs..
	/// </summary>
	[Tooltip("The speed at which the character climbs.")]
	public float climbSpeed;

	/// <summary>
	/// If the character is currently on a ladder.
	/// </summary>
	/// <value><c>true</c> if on ladder; otherwise, <c>false</c>.</value>
	public bool isOnLadder {
		get{ return gameObject.layer == ladderLayer; }
	}

	/// <summary>
	/// If currently climbing
	/// </summary>
	private bool climbing = false;

	/// <summary>
	/// The character's rigidbody.
	/// </summary>
	private Rigidbody2D _rigidbody;

	/// <summary>
	/// The default gravity scale for the character.
	/// </summary>
	private float _initialGravityScale;

	/// <summary>
	/// The default physics layer 
	/// </summary>
	private int _initialPhysicsLayer;

	private int ladderLayer;

	private GrabAndCarry _grabAndCarry;

	// Use this for initialization
	void Start () {
		// get components
		_rigidbody = GetComponent<Rigidbody2D>();
		_character = GetComponent<Character>();
		_grabAndCarry = GetComponent<GrabAndCarry>();

		// get initial settings
		_initialGravityScale = _rigidbody.gravityScale;
		_initialPhysicsLayer = this.gameObject.layer;

		// get the ladder layer
		ladderLayer = LayerMask.NameToLayer( "Ladder" );
	}

	// Update is called once per frame
	void LateUpdate () {

		// if in the ladder layer and not carrying anything
		if ( _character.gameObject.layer == ladderLayer && !_grabAndCarry.carrying ) {
				
			// 
			if ( _character.inputCheck() && _character.canMove ) {

				_rigidbody.gravityScale = 0;
				_character.animator.SetBool( "Climbing", true );

				float v = Joypad.Read.Buttons.vertical;
				_rigidbody.velocity = new Vector2( _rigidbody.velocity.x, v * climbSpeed );
			}
		} else {
			_character.animator.SetBool( "Climbing", false );
		}
	}


	/// <summary>
	/// Sets the layer of this object and it's children
	/// </summary>
	/// <param name="layer">Layer.</param>
	void setLayer(int layer)
	{
		

		// set this gameObject's layer
		gameObject.layer = layer;

		// set gravity
		if (layer == ladderLayer) {
			_rigidbody.gravityScale = 0;
		} else {
			_rigidbody.gravityScale = _initialGravityScale;
		}


		// set each child object's layer
		for( int i = 0; i < transform.childCount; i++ ) {
			GameObject g = transform.GetChild( i ).gameObject;
			g.layer = layer;
		}
	}

	// when on a trigger
	void OnTriggerStay2D(Collider2D other) {

		// TODO: it would be more efficient to move this to the ladder object, since it will have few OnTriggerStay2D events to check

		LadderZone ladder = other.GetComponent<LadderZone>();

		if ( ladder && _character.gameObject.layer != ladderLayer ) {

			if (verticalInput()) {
				//_holdExitTrigger = true;
				setLayer( ladderLayer );
			}

		} else if( _character.gameObject.layer == ladderLayer ){
			// setLayer( _initialPhysicsLayer );

		}
	}

	void OnTriggerExit2D(Collider2D other) {
		
		Debug.Log( "EXIT: " + other.gameObject );

		LadderZone ladder = other.GetComponent<LadderZone>();

		if (ladder) {
			setLayer( _initialPhysicsLayer );
		}
	}

	bool verticalInput() {
		return Mathf.Abs( Joypad.Read.Buttons.vertical ) > 0.1f ;
	}
}
