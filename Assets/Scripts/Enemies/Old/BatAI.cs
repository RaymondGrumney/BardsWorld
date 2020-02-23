using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : BaseEnemyAI {

	[Header("Bat AI Settings")]
	[Tooltip("How high we fly.")]
	public float flyHeight;
	[Tooltip("Our upward velocity.")]
	public float flightStrength;
	[Tooltip("The Bat's gravity.")]
	public float flightGravity;
	[Tooltip("The bat's maximum falling velocity.")]
	[Range(0f, -10f)]
	public float fallingVelocity;
	[Tooltip("Whether the bat follows the character.")]
	public bool homesOnCharacter;

	// the sign (+/-) of the position of the character the bat spotted (used to see if we've passed that character by)
	private float _seenSign;

	// whether the bat is hiding
	private bool _hiding;
	public float HideTime = 0f;
	private float _lastHide = 0f;


	[Tooltip("Length of time the Bat remains active before fleeing (in seconds, 0 = infinity).")]
	public float awakeTime;
	// when the bat saw the character
	private float _timeAwakened;


	void Start() 
	{
		InitializationRoutine();
	}


	public override bool ForgetBehavior() 
	{	
		return !_pursuing;
			
		// move hiding here?
		// return true only if has hidden?
	}

	public void Hide()
	{
		_pursuing = false;
		_lastHide = Time.time;
		_rigidbody.velocity = Vector2.zero;
		_rigidbody.gravityScale = -1; // fall up to make sure we're contacting the ceiling
	}

	/// <summary>
	/// Raises the collision enter2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		// when the bat hits the player
		Impact(other);
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		Impact(other);
	}

	private void Impact(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!_hiding)
			{
				FlyAwayFrom(other);
			}
		}
		else
		{
			if (_hiding)
			{
				if (Ceiling(other))
				{
					Hide();
				}
			}
			else
			{
				// fly away from other objects and stop pursuing player?
				SetFacing(this.transform.position.x - other.transform.position.x);
				_pursuing = false; // does this let him bounce off objects?
				_rigidbody.gravityScale = -flightGravity;
			}
		}
	}

	private bool Ceiling(Collision2D collision)
	{
		// logic taken from https://gamedev.stackexchange.com/questions/11782/have-a-simple-enemy-detecting-he-touch-a-wall-to-make-him-stop-turn-around
		foreach (ContactPoint2D contactPoint in collision.contacts)
		{
			float dot = Vector3.Dot(contactPoint.normal, transform.right);

			if (dot < 1.0f && dot > -1.0f)
			{
				return true;
			}
		}

		return false;
	}

	private void FlyAwayFrom(Collision2D other)
	{
		ForgetPlayer();
		SetFacing ( this.transform.position.x - other.transform.position.x );
		_rigidbody.velocity = facing * speed;
	}

	private void ForgetPlayer()
	{
		// the bat forgets the character when it hits them
		_hiding = true;
		_pursuing = false;
		// time to fly up
		_rigidbody.gravityScale = -flightGravity;
	}

	/// <summary>
	/// The AI's default behavior.
	/// </summary>
	public override void DefaultBehavior() 
	{
		if( Time.time > _lastHide + HideTime && _hiding )
		{
			_hiding = false;
		};
	}

	/// <summary>
	/// The AI's idle behavior
	/// </summary>
	public override void IdleBehavior() {
		// do nothing
	}		

	/// <summary>
	/// the AI's pursuit.
	/// </summary>
	public override void PursuitBehavior() 
	{
		if ( _pursuing ) 
		{
			// make sure gravity is turned on 
			_rigidbody.gravityScale = flightGravity;


			// turn to face the character if flagged
			if (homesOnCharacter) 
			{
				FaceTarget();
			} 
			else if( GetSign() != _seenSign ) 
			{
				ForgetPlayer();
			}

			MoveForward();

			if (_rigidbody.velocity.y < fallingVelocity) 
			{
				_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, fallingVelocity);
			}

			// drop
			// add force based on the distance from the ground
			RaycastHit2D hit = Physics2D.Raycast( this.transform.position, Vector2.down, 100f, _physicsLayer );
			float fromFlyHeight = hit.distance - flyHeight;
			float diffHeightFromPlayer = this.transform.position.y - ((Vector3) _targetPoint).y;

			if (diffHeightFromPlayer < 0) 
			{
				_rigidbody.AddForce( Vector2.up * - diffHeightFromPlayer * flightStrength );
			}

			_rigidbody.AddForce( Vector2.up * flightStrength / fromFlyHeight );


			if (awakeTime > 0) 
			{
				if (_timeAwakened + awakeTime < Time.time) {
					ForgetPlayer();
				}
			}
		}
	}

	/// <summary>
	/// Sets the target.
	/// </summary>
	/// <param name="target">Target.</param>
	public override void SetTarget( GameObject target ) 
	{
		if( !_pursuing && !_hiding ) 
		{
			_pursuing = true;
			_targetPoint = target.transform.position;
			_lastSawCharacter = Time.time + timeToForget;

			FaceTarget();

			_seenSign = GetSign();

			_timeAwakened = Time.time;
		}
	}

	private float GetSign()
		=> Mathf.Sign(((Vector3) _targetPoint - transform.position).x);
}
