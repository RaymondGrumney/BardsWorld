using CommonAssets.Utilities;
using System.Collections;
using UnityEngine;

/// <summary>
/// A character object, concerned with moving the character and other user input.
/// </summary>
public class Character : MonoBehaviour
{
	/// <summary>
	/// Whether this character is active.
	/// </summary>
	[Tooltip("Whether this character is active.")]
	public bool isActive = false;
	public bool SetActive(bool state = true) { return isActive = state; }

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
	private float _idleSince = 0;					// counts length of time since last input


	/// <summary>
	/// Whether the caracter can currently move. Adjust this in other scripts to prevent character movement.
	/// </summary>
	private bool _canMove = true;	

	/// <summary>
	/// Whether the caracter is currently allowed to move.
	/// </summary>
	public bool CanMove{
		get => _canMove;
		set => _canMove = value; 
	}


	/// <summary>
	/// Whether the character is currently touching ground.
	/// </summary>
	protected bool _grounded = true;

//	public bool

	/// <summary>
	/// This character's animator.
	/// </summary>
	protected Animator _animator;

	public Animator Animator {
		get{ return _animator; }
	}

	/// <summary>
	/// the normalized the character is "facing".
	/// </summary>
	protected Vector2 _facing = Vector3.right;

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
	protected Rigidbody2D _rigidbody;

	void Start() {
		// get private components
		startRoutine();
	}


	// set private variables
	protected void startRoutine() {
		_animator = GetComponent<Animator>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_rigidbody = GetComponent<Rigidbody2D>();
	}


	// updated every frame
	void FixedUpdate()
	{
		if ( isActive ) {
			standardUpdateActions();

			if ( _receivingInput ) {
				checkMovement();
			}
		} 
	}


	// updated every frame after Update()
	void LateUpdate()
	{
		// tell the animinator what your vertical speed is
		if (_animator) {
			_animator.SetFloat( "vSpeed", _rigidbody.velocity.y );
		}
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

		//moveSpeed.y += _rigidbody.velocity.y;

		if (inputCheck() && _canMove) {
			
			float h = Joypad.Read.Buttons.horizontal;

			// if the value on the horizontal axis is +/- 0.1
			if (Mathf.Abs( h ) > 0.1f)
			{
				// move the character
				_rigidbody.velocity = new Vector2(h * maxSpeed, _rigidbody.velocity.y);

				// check if new facing and rotate
				Vector2 newFacing = new Vector2(Mathf.Ceil(h), 0);

				if (_facing != newFacing)
				{
					_facing = newFacing;
					Easily.Flip(gameObject).On("yAxis");
				}
			}
			else {
				// stop horizontal movement if the player is not moving the character
//				stop();
			}

			// set Params of animator
			_animator.SetFloat( "Speed", Mathf.Abs( h ) );
		} else {
			// set Params of animator
			_animator.SetFloat( "Speed", 0 );
		}
	}


	// checks current acctions (currently this is handled by individual scripts
	private void checkActions() {
		
	}

	/// <summary>
	/// If this character is currently receiving input
	/// </summary>
	/// <returns><c>true</c>, if input is being accepted, <c>false</c> otherwise.</returns>
	public bool inputCheck()
		// it is currently receiving input if:
		//   it's marked active
		//   it's marked as receiving input (obviously)
		//   it's time out value is in the past
		=> isActive && _receivingInput;
		//return isActive && _receivingInput && _inputTimeOut < Time.time;



	/// <summary>
	/// Stops movement for this character
	/// </summary>
	public virtual void stop()
	{
		if (_grounded) 
		{
			_rigidbody.velocity = new Vector2( 0, _rigidbody.velocity.y ); // TODO: why would rb.vel.y not == 0 if grounded?
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
}
