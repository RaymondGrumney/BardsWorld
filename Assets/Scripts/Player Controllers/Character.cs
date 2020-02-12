using System.Collections;
using UnityEngine;

/// <summary>
/// A character object, concerned with moving the character and other user input.
/// </summary>
public class Character : Moveable {

	/// <summary>
	/// Whether this character is active.
	/// </summary>
	[Tooltip("Whether this character is active.")]
	public bool isActive = false;

	/// <summary>
	/// Whether the character is swimming.
	/// </summary>
	/// <value><c>true</c> if is swimin; otherwise, <c>false</c>.</value>
	public bool isSwimming { set; get; }


	[Header("Movement")]
	/// <summary>
	/// This character's maximum speed.
	/// </summary>
	[Tooltip("This character's maximum speed.")]
	public float maxSpeed = 3;
	public float horizontal;

	/// <summary>
	/// The standard control timeout length (on attack, etc).
	/// </summary>
	[Tooltip("The standard control timeout length (on attack, etc).")]
	public float standardTimeout;


	/// <summary>
	/// If the character is currently receiving input. Turn on / off to control if the character is receiving input
	/// </summary>
	[Tooltip("Whether the character is currently receiving input.")]
	private bool _receivingInput = true;

	public bool receivingInput{
		get{ return _receivingInput; }
		set{ _receivingInput = value; }
	}

	/// <summary>
	/// The next Time.time players can control this object
	/// </summary>
	protected float _inputTimeOut = 0;				// to prevent input during actions
	private float _idleSince = 0;					// counts length of time since last input


	/// <summary>
	/// Whether the caracter can currently move. Adjust this in other scripts to prevent character movement.
	/// </summary>
	private bool _canMove = true;	

	/// <summary>
	/// Whether the caracter is currently allowed to move.
	/// </summary>
	public bool canMove{
		get{ return _canMove; }
		set{ _canMove = value; }
	}


	/// <summary>
	/// Whether the character is currently touching ground.
	/// </summary>
	protected bool _grounded = true;

//	public bool

	/// <summary>
	/// This character's animator.
	/// </summary>
	protected Animator _animator;						// this character's animator

	public Animator animator {
		get{ return _animator; }
	}

	/// <summary>
	/// the normalized the character is "facing".
	/// </summary>
	protected Vector3 _facing = Vector3.right;

	/// <summary>
	/// The normalized direction this character is "Facing."
	/// </summary>
	public Vector3 facing {
		get{ return _facing; }
	}

	/// <summary>
	/// The sprite renderer.
	/// </summary>
	protected SpriteRenderer _spriteRenderer;


	void Start() {
		// get private components
		startRoutine();
	}


	// set private variables
	protected void startRoutine() {
		_animator = GetComponent<Animator>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// updated every frame
	void FixedUpdate(){

		if ( isActive ) {
			standardUpdateActions();

			if ( _receivingInput ) {
				checkMovement();
			}
		} 

		base.FixedUpdate();
	}


	// updated every frame after Update()
	void LateUpdate() {

		// tell the animinator what your vertical speed is
		if (_animator) {
			_animator.SetFloat( "vSpeed", _rigidbody.velocity.y );
		}
	}


	/// <summary>
	/// Times out input by the standard timeout
	/// </summary>
	public void inputTimeout() {
		_inputTimeOut = Time.time + standardTimeout; 
	}

	/// <summary>
	/// Times out input by the sent vale (in seconds).
	/// </summary>
	/// <param name="value">How long to time out input.</param>
	public void inputTimeout(float value) {
		_inputTimeOut = Time.time + value; 
	}


	// Actions 
	protected void standardUpdateActions() {
		horizontal = Joypad.Read.Buttons.horizontal;
	}


	// move based on player movement
	private void checkMovement(){
		
		// start with _moveVector and current y velocity
//		Vector2 moveSpeed = _moveVector;
		Vector2 moveSpeed = Vector2.zero;

		moveSpeed.y += _rigidbody.velocity.y;

		if (inputCheck() && _canMove) {
			float h = Joypad.Read.Buttons.horizontal;

			// if the value on the horizontal axis is +/- 0.1
			if (Mathf.Abs( h ) > 0.1f) {

				// update facing
				_facing = new Vector2( Mathf.Ceil( h ), 0 );

				// move the character
				moveSpeed.x += h * maxSpeed;
					
				// flip the sprite if moving leftward
				_spriteRenderer.flipX = h < 0.1f;
			} else {
				// stop horizontal movement if the player is not moving the character
//				stop();
			}

			// set Params of animator
			_animator.SetFloat( "Speed", Mathf.Abs( h ) );
		} else {
			//NOTE: should this be uncommented? to let blowers blow?

			// set Params of animator
			_animator.SetFloat( "Speed", 0 );
		}

		// move the character and reset _moveVector
		_rigidbody.velocity = moveSpeed;
//		_moveVector = Vector2.zero;

//		_moveVector += moveSpeed;
	}


	// checks current acctions (currently this is handled by individual scripts
	private void checkActions() {

	}

	/// <summary>
	/// If this character is currently receiving input
	/// </summary>
	/// <returns><c>true</c>, if input is being accepted, <c>false</c> otherwise.</returns>
	public bool inputCheck() {
		
		// it is currently receiving input if:
		//   it's marked active
		//   it's marked as receiving input (obviously)
		//   it's time out value is in the past
		return isActive && _receivingInput && _inputTimeOut < Time.time;
	}



	/// <summary>
	/// Stops movement for this character
	/// </summary>
	public virtual void stop()
	{
		if (_grounded) {
//			_rigidbody.velocity = _moveVector + new Vector2( 0, _rigidbody.velocity.y ); // TODO: why would rb.vel.y not == 0 if grounded?
//			_moveVector = Vector2.zero;
			_moveVector = _moveVector + new Vector2( 0, _rigidbody.velocity.y ); // TODO: why would rb.vel.y not == 0 if grounded?

		}

		_animator.SetFloat( "Speed", 0 );
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="Character"/> is grounded.
	/// </summary>
	/// <value><c>true</c> if grounded; otherwise, <c>false</c>.</value>
	public bool grounded {
		get{ return _grounded; }
		set {
			_grounded = value;

			// also let the animator know the character is grounded.
			_animator.SetBool( "Grounded", value );
		}
	}


	protected override void MoveByVector(){

		// modify velocity
		_rigidbody.velocity += _moveVector;

		// reset vector to 0
		_moveVector = Vector2.zero;
	}
}
