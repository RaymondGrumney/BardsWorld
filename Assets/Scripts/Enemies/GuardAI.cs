using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : BaseEnemyAI {

	[Header("Guard AI Setting")]
	[Tooltip("Likelihood the AI wanders from it's starting point.")]
	public float wanderChance;

	[Tooltip("Likelihood the AI returns to it's starting point.")]
	public float returnToHomeChance;

	[Tooltip("Likelihood the AI stays idle.")]
	public float idleChance;

	[Tooltip("Likelihood the AI turns while idling.")]
	[Range(0, 1)]
	public float turnChanceDuringIdle = 1f;

	public float maximumWanderDistance = 3f;
	public float minimumWanderDistance = 1f;



	// check if forgetten player 
	public override bool ForgetBehavior()
	{
		// if the enemy has forgotten it saw the character, return to starting point
		if (_lastSawCharacter + timeToForget < Time.time) {
			_targetPoint = _startingPoint;
			_pursuing = false;

			return true;

		} else {
			return false;
		}
	}


	// idle 
	public override void IdleBehavior()
	{ 
		if ( MyUtilities.CalculateChancePerDeltaTime( turnChanceDuringIdle * Time.fixedDeltaTime ) ) 
		{
			SetFacing( -facing.x );
		}
	}


	public override void DefaultBehavior()
	{
		if( !_pursuing ) 
		{
			// if target has been set to starting point
			if (nextRandomChoice < Time.time)
			{
				// if it's time to make another random choice
				if ( _collider.OverlapPoint( new Vector2( _gotoPoint.x, transform.position.y ))) // don't care about height in level (since he can't jump)
				{
					// we're currently not heading toward a position
					MakeRandomChoice();
				} 
				else  // perform the previous choice
				{
					PerformLastRandomChoice();
				}
			} 
			else 
			{
				// execute whatever the last choice was
				PerformLastRandomChoice();
			}
		}
	}

	/// <summary>
	/// Makes a random choice.
	/// </summary>
	protected void MakeRandomChoice()	
	{
		// set random choice timer
		nextRandomChoice = Time.time + randomChoiceEveryNSeconds;

		// how many choices we have
		float choices = wanderChance + returnToHomeChance + idleChance;
		// generate a random number
		float r = Random.Range( 0f, choices );

		if (r < wanderChance) 
		{
			// wander from starting point
			ChooseWanderLocation();
		} 
		else if (r < wanderChance + returnToHomeChance) 
		{
			// go back to starting point
			_gotoPoint = _startingPoint;
			SetFacing( Mathf.Sign( _startingPoint.x - transform.position.x ) );
		} 
		else 
		{
			// idle 
			_gotoPoint = transform.position;
		}
	}


	/// <summary>
	/// Chooses the wander location.
	/// </summary>
	protected void ChooseWanderLocation()
	{
		// RaycastHit2D hit = Physics2D.Raycast(transform.position, facing, minimumWanderDistance);

		float x = Random.Range(minimumWanderDistance, maximumWanderDistance) * facing.x + transform.position.x;
		_gotoPoint = new Vector2(x, transform.position.y);
	}

	/// <summary>
	/// Performs the last random choice.
	/// </summary>
	protected void PerformLastRandomChoice()
	{
		if ( ! IsNearEnough( transform.position.x, _gotoPoint.x ) ) 
		{
			// wander / return home
			// this should determine if we've passed the target point
			if ( ! _collider.OverlapPoint( _gotoPoint ) ) 
			{
				MoveForward();
			} 
			else 
			{
				_gotoPoint = transform.position;
			}

		}
		else // we're at the goto point and/or idling
		{
			IdleBehavior();
		}
	}

	// pursuit behavior
	public override void PursuitBehavior() 
	{
		// if we're not near the target point
		if ( ! _collider.OverlapPoint( (Vector2) _targetPoint ) 
			&& _targetPoint != null ) 
		{
			FaceTarget();
			MoveForward();
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		// logic taken from https://gamedev.stackexchange.com/questions/11782/have-a-simple-enemy-detecting-he-touch-a-wall-to-make-him-stop-turn-around
		bool hitWall = false;
		foreach (ContactPoint2D contactPoint in collision.contacts)
		{
			float dot = Vector3.Dot(contactPoint.normal, transform.up);
			if (dot < 1.0f && dot > -1.0f)
			{
				hitWall = true;
				break;  //No need to keep checking once you've found a wall
			}
		}

		if (hitWall)
		{
			//At least one collision was not with the floor (or ceiling)
			//Handle wall collisions here
			_gotoPoint = Easily.Clone(transform.position);
		}
	}
}
