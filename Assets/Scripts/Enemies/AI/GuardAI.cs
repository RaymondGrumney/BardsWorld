using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : BaseEnemyAI {

	[Header("Guard AI Setting")]

	public float maximumWanderDistance = 3f;
	public float minimumWanderDistance = 1f;

	public List<RandomChoice> RandomChoices = new List<RandomChoice>()
	{
		new RandomChoice()
		{
			Choice="ChooseWanderLocation",
			Chance=7
		},
		new RandomChoice()
		{
			Choice="ReturnToHome",
			Chance=3
		},
		new RandomChoice()
		{
			Choice="Idle",
			Chance=3
		},
		new RandomChoice()
		{
			Choice="Turn",
			Chance=3
		}
	};

	// check if forgetten player 
	public override bool ForgetBehavior()
	{
		// if the enemy has forgotten it saw the character, return to starting point
		if ( base.ForgetBehavior() ) 
		{
			_targetPoint = _startingPoint;
			_pursuing = false;

			return true;

		} 
		else 
		{
			return false;
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
					MakeRandomChoice(RandomChoices);
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


	public void ReturnToHome()
	{
		if (_collider.OverlapPoint(_startingPoint))
		{
			MakeRandomChoice(RandomChoices);
			// Or turn?
		}
		else
		{
			_gotoPoint = _startingPoint;
			SetFacing(Mathf.Sign(_startingPoint.x - transform.position.x));
		}
	}

	public void Idle()
	{
		_gotoPoint = transform.position;
	}

	public void Turn()
	{
		SetFacing(-facing.x);
	}

	public override void IdleBehavior() { }


	/// <summary>
	/// Chooses the wander location.
	/// </summary>
	public void ChooseWanderLocation()
	{
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

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (HitWall(collision))
		{
			_gotoPoint = Easily.Clone(transform.position);
		}
	}
}
