using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : BaseEnemy {

	[Header("Guard AI Setting")]
	[Tooltip("Likelihood the AI wanders from it's starting point.")]
	public float wanderChance;

	[Tooltip("Likelihood the AI returns to it's starting point.")]
	public float returnToHomeChance;

	[Tooltip("Likelihood the AI stays idle.")]
	public float idleChance;

	[Tooltip("How often the AI makes the choice to wander, return home, or idle.")]
	public float randomChoiceEveryNSeconds = 2f;

	[Tooltip("Likelihood the AI turns while idling.")]
	[Range(0, 1)]
	public float turnChanceDuringIdle = 1f;

	[Tooltip("The angle of the cast while seeking a point to wander to.")]
	[Range(0,1)]
	public float MaxWanderAngle = 0.7f;

	// when the last choice was made
	protected float nextRandomChoice = 0;

	// check if forgetten player 
	protected override bool ForgetBehavior()
	{
		// if the enemy has forgotten it saw the character, return to starting point
		if (_lastSawCharacter < Time.time) {
			_targetPoint = _startingPoint;
			_pursuing = false;

			return true;

		} else {
			return false;
		}
	}


	// idle 
	protected override void IdleBehavior()
	{

		if ( MyUtilities.CalculateChancePerDeltaTime( turnChanceDuringIdle ) ) 
		{
			SetFacing( -facing.x );
		}
	}


	protected override void DefaultBehavior()
	{
		if( !_pursuing ) 
		{
			// if target has been set to starting point
			if (nextRandomChoice < Time.time)
			{
				// if it's time to make another random choice
				if (_collider.OverlapPoint(new Vector2(_gotoPoint.x, transform.position.y))) // don't care about height in level (since he can't jump)
				{
					// we're currently not heading toward a position
					MakeRandomChoice();
				} 
				else 
				{
					// otherwise perform the last choice
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
		Debug.Log("MakeRandomChoice");
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

		Debug.Log("ChooseWanderLocation");
		Vector2 randomDownwardAngle = Vector2.down + ( facing * Random.Range( MaxWanderAngle * 0.1f, MaxWanderAngle ) );
		RaycastHit2D pointInFrontOfThis = Physics2D.Raycast( transform.position, randomDownwardAngle, 10f, _physicsLayer );
		Easily.Instantiate(spoon, pointInFrontOfThis.point);

		float floorYLevel = Physics2D.Raycast( transform.position, Vector2.down, 5f, _physicsLayer ).point.y;

		// if the difference in height between the chosen point and the current floor is neglegible
		if ( Mathf.Abs( pointInFrontOfThis.point.y - floorYLevel ) < 0.1f) 
		{
			// set _wanderPoint to hit position
			_gotoPoint = new Vector2( pointInFrontOfThis.point.x, transform.position.y );
		} 
		else 
		{
			// make another choice 
			nextRandomChoice = 0;
		}
	}

	public GameObject spoon;

	/// <summary>
	/// Performs the last random choice.
	/// </summary>
	protected void PerformLastRandomChoice()
	{
		Debug.Log("PerformLastRandomChoice");
		if ( ! isNearEnough( transform.position.x, _gotoPoint.x ) ) 
		{
			// wander / return home
			// this should determine if we've passed the target point
			if ( ! _collider.OverlapPoint( _gotoPoint ) ) 
			{
				moveForward();
			} 
			else 
			{
				_gotoPoint = transform.position;
			}

		} else {
			// we're at the goto point and/or idling
			IdleBehavior();
		}
	}

	// pursuit behavior
	protected override void PursuitBehavior() 
	{
		// if we're not near the target point
		if ( ! _collider.OverlapPoint( _targetPoint ) ) 
		{
			faceTarget();

			// see if there's a surface immediately ahead of us to walk on
			RaycastHit2D hit = Physics2D.Raycast( this.transform.position, facing + Vector2.down, 1f, _physicsLayer );

			// if the target isn't above us (which would it even be?) 
			if ( hit ) 
			{
				if( ! isNearEnough( hit.point.y, this.transform.position.y ) ) 
				{
					moveForward();
				}
			}

		}
	}
}
