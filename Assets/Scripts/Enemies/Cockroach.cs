using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockroach : BaseEnemy {

//	if ( forgetBehavior() ) {
//		defaultBehavior();
//	} else {
//		pursuitBehavior();
//	}

	// what to do if not pursuing character
	protected override void DefaultBehavior()
	{
		Debug.Log(this + " defaultBehaviour() transform.position.x: " + transform.position.x );
		Debug.Log(this + " defaultBehaviour() _startingPoint.x: " + _startingPoint.x );
		Debug.Log(this + " defaultBehaviour() distance: " + Mathf.Abs( transform.position.x - _startingPoint.x ) );
		 
		if ( !isNearEnough(  transform.position.x, _startingPoint.x )) {
			
			_targetPoint = _startingPoint;

			// return to start location
			faceTargetPoint();
			moveForward();

		// if idle
		} else {
			IdleBehavior();
		}
	}

	protected override void IdleBehavior()
	{
		Debug.Log( this + " idleBehaviour()" );
		// does nothing
		// TODO: random animation (clean antenna)
	}

	protected override bool ForgetBehavior()
	{
		// forgets if not eating and character has been outside of sight for timeToForget
		return _lastSawCharacter + timeToForget < Time.time && baitEating == null;
	}

	protected override void PursuitBehavior()
	{
		Debug.Log( this + " pursuitBehavior()" );
		// if eating 
		if (baitEating != null) {

			if (Time.time > timeTouchedBait + howLongEatsBait) {
				Destroy( baitEating );
			}

			// if not positioned near starting point
		} else {
			// move toward target
			faceTargetPoint();
			moveForward();
		}
	}

	void OnCollisionEnter2D( Collision2D other) {
		if (other.gameObject.tag == "Player") {
			// play attack animation

			// NOTE: animation can pause movement for enemy	

		} else { 
			
			// check if bait
			Attributes attributes = other.gameObject.GetComponent<Attributes>();

			if (attributes != null) {
				if (attributes.bait) {
					
					// stop and eat
					if (timeTouchedBait == 0) {
						timeTouchedBait = Time.time;

						// assign to baitEating and it's life script
						baitEating = other.gameObject;
					}
				}
			}
		}
	}

//	void OnCollisionExit2D( Collision2D other ) {
//
//		Attributes attributes = other.gameObject.GetComponent<Attributes>();
//
//		if (attributes != null) {
//			if (attributes.bait) {
//				baitEating = null;
//			}
//		}
//	}
}

