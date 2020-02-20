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
	/// <summary>
	/// Sets whether the character is active.
	/// </summary>
	/// <param name="state">True or False</param>
	/// <returns></returns>
	public bool SetActive(bool state = true) { return isActive = state; }

	/// <summary>
	/// Whether the character is swimming.
	/// </summary>
	/// <value><c>true</c> if is swimin; otherwise, <c>false</c>.</value>
	public bool IsSwimming { set; get; }
	

	[Header("Movement")]
	/// <summary>
	/// This character's maximum speed.
	/// </summary>
	[Tooltip("This character's maximum speed.")]
	public float maxSpeed = 3;
	/// <summary>
	/// This character's maximum speed.
	/// </summary>
	public float horizontal;



	/// <summary>
	/// If the character is currently receiving input. Turn on / off to control if the character is receiving input
	/// </summary>
	[Tooltip("Whether the character is currently receiving input.")]
	public bool ReceivingInput = true;

	/// <summary>
	/// The how long it's been sine last input
	/// </summary>
	private float _idleSince = 0;


	/// <summary>
	/// Whether the caracter can currently move. Adjust this in other scripts to prevent character movement.
	/// </summary>
	public bool CanMove = true;	


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
	protected Vector2 _facing = Vector2.right;

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
			StandardUpdateActions();

			if ( ReceivingInput ) {
				CheckMovement();
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
	protected void StandardUpdateActions() 
	{
		horizontal = Joypad.Read.Buttons.horizontal;
	}


	// move based on player movement
	private void CheckMovement()
	{
		if (InputCheck() && CanMove) 
		{
			float h = Joypad.Read.Buttons.horizontal;

			// if the value on the horizontal axis is +/- 0.1
			if ( Mathf.Abs( h ) > 0.1f )
			{
				Vector2 newFacing = new Vector2(Mathf.Ceil(h), 0);
				if (_facing != newFacing)
				{
					_facing = newFacing;
					Easily.Flip(gameObject).On("yAxis");
				}
			}

			if (!Ducking())
			{
				_rigidbody.velocity = new Vector2(h * maxSpeed, _rigidbody.velocity.y);
				_animator.SetFloat("Speed", Mathf.Abs(h));
			}
		} else {
			// set Params of animator
			_animator.SetFloat( "Speed", 0 );
		}
	}

	/// <summary>
	/// If this character is currently receiving input
	/// </summary>
	/// <returns><c>true</c>, if input is being accepted, <c>false</c> otherwise.</returns>
	public bool InputCheck()
		// it is currently receiving input if:
		//   it's marked active
		//   it's marked as receiving input (obviously)
		//   it's time out value is in the past
		=> isActive && ReceivingInput;



	/// <summary>
	/// Stops movement for this character
	/// </summary>
	public virtual void Stop()
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
	public bool Grounded {
		get{ return _grounded; }
		set {
			_grounded = value;

			// also let the animator know the character is grounded.
			_animator.SetBool( "Grounded", value );
		}
	}

	public bool Ducking() 
	{
		if ( Joypad.Read.Buttons.Held("down") )
		{
			_animator.SetBool("Ducking", true);
			return true;
		}

		if ( !Joypad.Read.Buttons.Held("down") )
		{
			_animator.SetBool("Ducking", false);
			return false;
		}

		return Joypad.Read.Buttons.Held("down");
	}
}
