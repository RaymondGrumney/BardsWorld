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
	public float randomChoiceEveryNSeconds = 2;

	[Tooltip("Likelihood the AI turns while idling.")]
	public float turnChanceDuringIdle = 1;

	[Tooltip("The angle of the cast while seeking a point to wander to.")]
	[Range(0,1)]
	public float wanderAngle = 0.7f;

	// when the last choice was made
	protected float nextRandomChoice = 0;

	// check if forgetten player 
	protected override bool forgetBehavior(){
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
	protected override void idleBehavior()
	{
		if ( MyUtilities.CalcChancePerDeltaTime( turnChanceDuringIdle ) ) {
			setFacing( -facing.x );
		}
	}


	protected override void defaultBehavior(){

		if( !_pursuing ) {
			// if target has been set to starting point
			if (nextRandomChoice < Time.time){
				// if it's time to make another random choice
				if ( _collider.OverlapPoint(_gotoPoint ) ) {
					// we're currently not heading toward a position
					makeRandomChoice();
				} else {
					// otherwise perform the last choice
					performLastRandomChoice();
				}
			} else {
				// execute whatever the last choice was
				performLastRandomChoice();
			}
		}
	}

	/// <summary>
	/// Makes a random choice.
	/// </summary>
	protected void makeRandomChoice()	
	{
		// set random choice timer
		nextRandomChoice = Time.time + randomChoiceEveryNSeconds;

		// how many choices we have
		float choices = wanderChance + returnToHomeChance + idleChance;
		// generate a random number
		float r = Random.Range( 0f, choices );

		if (r < wanderChance) {
			// wander from starting point
			chooseWanderLocation();
		} else if (r < wanderChance + returnToHomeChance) {
			// go back to starting point
			_gotoPoint = _startingPoint;
			setFacing( Mathf.Sign( _startingPoint.x - transform.position.x ) );
		} else {
			// idle 
			_gotoPoint = transform.position;
		}
	}


	/// <summary>
	/// Chooses the wander location.
	/// </summary>
	protected void chooseWanderLocation()
	{
		// a random downward angle
		Vector2 downwardAngle = Vector2.down * Random.Range( wanderAngle * 0.1f, wanderAngle );
		// a point in front of the enemy
		RaycastHit2D hit = Physics2D.Raycast( transform.position, facing + downwardAngle, 5f, _physicsLayer );
		// the current y level of the floor
		float floorLevel = Physics2D.Raycast( transform.position, Vector2.down, 5f, _physicsLayer ).point.y;

		// if the difference in height between the chosen point and the current floor is neglegible
		if ( Mathf.Abs( hit.point.y - floorLevel ) < 0.1f) {
			// set _wanderPoint to hit position
			_gotoPoint = new Vector2( hit.point.x, transform.position.y );
			//setFacing( _facing.x );
		} else {
			// make another choice 
			nextRandomChoice = 0;
		}
	}

	/// <summary>
	/// Performs the last random choice.
	/// </summary>
	protected void performLastRandomChoice()
	{

		if ( ! isNearEnough( transform.position.x, _gotoPoint.x ) ) {
			// wander / return home

			// this should determine if we've passed the target point
			if ( ! _collider.OverlapPoint( _gotoPoint ) ) {
				moveForward();
			} else {
				_gotoPoint = transform.position;
			}

		} else {
			// we're at the goto point and/or idling
			idleBehavior();
		}
	}

	// pursuit behavior
	protected override void pursuitBehavior() {

		// if we're not near the target point
		if ( ! _collider.OverlapPoint( _targetPoint ) ) {

			//Vector2 vector = Vector2.Lerp( transform.position, _targetPoint, 1 );

			// set facing to the direction the target is 
			faceTarget();

			// see if there's a surface immediately ahead of us to walk on
			RaycastHit2D hit = Physics2D.Raycast( this.transform.position, facing + Vector2.down, 1f, _physicsLayer );

			// if the target isn't above us (which would it even be?) 
			if ( hit != null ) {
				if( ! isNearEnough( hit.point.y, this.transform.position.y ) ) {
					moveForward();
				}
			}

		}
	}
}
