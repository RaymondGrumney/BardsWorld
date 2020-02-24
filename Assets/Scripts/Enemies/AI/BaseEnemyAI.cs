using CommonAssets.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The base enemy AI.
/// </summary>
public abstract class BaseEnemyAI : MonoBehaviour 
{
	[Header("Base AI Settings")]
	[Header("Movement Settings")]

	/// <summary>
	/// How fast this enemy moves.
	/// </summary>
	[Tooltip("How fast this enemy moves.")]
	public float speed = 2.5f;

	/// <summary>
	/// How far the AI is willing to move (in world units).
	/// </summary>
	[Tooltip("How far the AI is willing to move (in world units).")]
	public float moveMargin = 0.01f;

	/// <summary>
	/// Whether this enemy avoids falling off a cliff
	/// </summary>
	[Tooltip("Whether this enemy avoids falling off a cliff.")]
	public bool avoidsFalling;

	/// <summary>
	/// This object's rigid body.
	/// </summary>
	protected Rigidbody2D _rigidbody;

	/// <summary>
	/// Which direction this object is facing.
	/// </summary>
	public Vector2 facing = new Vector2( 1, 0 );


	/// <summary>
	/// Where the enemy started in this level.
	/// </summary>
	protected Vector2 _startingPoint;

    /// <summary>
    /// Where this enemy is currently going.
    /// </summary>
    protected Vector2 _gotoPoint;

	/// <summary>
	/// The physics layer this object is on.
	/// </summary>
	protected int _physicsLayer;


	/// <summary> 
	/// Whether the enemy is pursuing a character. 
	/// </summary>
	protected bool _pursuing;


	[Header("Pursuit Settings")]
	/// <summary>
	/// How long this enemy pursues the last seen character.
	/// </summary>
	[Tooltip("How long this enemy pursues the last seen character.")]
	public float timePursuesCharacter;

	/// <summary>
	/// How long this enemy takes to notice the character in it's field of vision.
	/// </summary>
	[Tooltip("How long this enemy takes to notice the character in it's field of vision.")]
	public float timeToNoticeCharacter;

	/// <summary>
	/// How long this enemy takes to forget it saw a character.
	/// </summary>
	[Tooltip("How long this enemy takes to forget it saw a character.")]
	public float timeToForget = 3;

	/// <summary>
	/// When the character was last seen
	/// </summary>
	protected float _lastSawCharacter;

	/// <summary>
	/// Where the character was last seen
	/// </summary>
	protected Vector2? _targetPoint; // TODO: this should be replaced by _gotoPoint?

	/// <summary>
	/// The collider attached to this game object.
	/// </summary>
	protected Collider2D _collider;

	//TODO: move bait code into it's own script
	[Header("Bait Settings")]
	/// <summary>
	/// If this enemy is attracted to bait.
	/// </summary>
	public bool attractedToBait = true;

	/// <summary>
	/// How long this enemey takes to eat the bait.
	/// </summary>
	public float howLongEatsBait = 2f;

	/// <summary>
	/// When the enemy touched the bait it's currently touching.
	/// </summary>
	protected float timeTouchedBait;

	/// <summary>
	/// The bait this enemy is eating.
	/// </summary>
	protected GameObject baitEating;

	public GameObject debugObject;

	[Tooltip("How often the AI makes the choice to wander, return home, or idle.")]
	public float randomChoiceEveryNSeconds = 2f;

	// when the last choice was made
	protected float nextRandomChoice = 0f;

	/// <summary>
	/// Whether the enemy AI is active
	/// </summary>
	private bool _active = true;

	protected Animator _animator;

	// Use this for initialization
	protected virtual void Awake () 
	{
		InitializationRoutine();
	}

	/// <summary>
	/// Initializes references for this object.
	/// </summary>
	protected void InitializationRoutine() 
	{
		_startingPoint = transform.position;
		_gotoPoint = transform.position;
		_targetPoint = transform.position;
		_physicsLayer = gameObject.layer;
		_lastSawCharacter = -timeToForget * 2;

		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<Collider2D>();
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	protected virtual void Update() 
	{
		if (_active)
		{
			if (ForgetBehavior())
			{
				DefaultBehavior();
			}
			else
			{
				PursuitBehavior();
			}
		}
	}


	/// <summary>
	/// Checks if the enemy has forgotten the character and performs the forget behavior if so
	/// </summary>
	/// <returns><c>true</c>, the enemy has forgotten the character, <c>false</c> otherwise.</returns>
	public virtual bool ForgetBehavior() 
	{
		return _lastSawCharacter + timeToForget < Time.time;
	}

	public void SetActive(bool state)
	{
		_active = state;
	}

	/// <summary>
	/// What to do if not pursuing the player
	/// </summary>
	public abstract void DefaultBehavior();

	/// <summary>
	/// Performs the idle behavior.
	/// </summary>
	public abstract void IdleBehavior();

	/// <summary>
	/// Check if are pursuing a character and performs pursuit behavior if so
	/// </summary>
	public virtual void PursuitBehavior()
	{
		// if we're not near the target point
		if (!_collider.OverlapPoint((Vector2)_targetPoint)
			&& _targetPoint != null)
		{
			FaceTarget();
			MoveForward();
		}
	}


	/// <summary>
	/// Sets this enemy's facing to either right (1) or left (-1).
	/// </summary>
	/// <param name="newFacing">New facing.</param>
	protected void SetFacing( float newFacing ) 
	{	
		newFacing = Mathf.Sign( newFacing );

		if ( facing.x != newFacing ) 
		{
			facing.x = newFacing;

			// rotate Y to 180 if facing left or to 0 if facing right	
			Easily.Flip(gameObject).On("yAxis");
		}
	}

	/// <summary>
	/// Faces the target.
	/// </summary>
	protected void FaceTarget() 
	{
		Vector2 diff = (Vector2)_targetPoint - (Vector2)transform.position;
		float facing = Mathf.Sign( diff.x );
		SetFacing( facing );
	}

	/// <summary>
	/// Sets the target .
	/// </summary>
	/// <param name="targetPoint">Target point.</param>
	public virtual void SetTarget(GameObject target) 
	{
		_pursuing = true;
		_targetPoint = target.transform.position;
		_gotoPoint = (Vector2) _targetPoint;
		_lastSawCharacter = Time.time + timeToForget;
	}


	/// <summary>
	/// if A is near enough to B
	/// </summary>
	/// <returns><c>true</c>, if a is near enough to be, by the moveMargin, <c>false</c> otherwise.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	protected bool IsNearEnough(Vector2 a, Vector2 b) 
	{
		return MyUtilities.IsNearEnough( a, b, moveMargin );
	}


	/// <summary>
	/// if A is near enough to B
	/// </summary>
	/// <returns><c>true</c>, if a is near enough to be, by the moveMargin, <c>false</c> otherwise.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	protected bool IsNearEnough(float a, float b) 
	{
		return MyUtilities.IsNearEnough( a, b, moveMargin );
	}


	/// <summary>
	/// Moves towards _gotoPoint or _targetPoint
	/// </summary>
	public virtual void MoveForward()
	{
		bool doMove = true; // defaults to move this frame

		if (avoidsFalling)
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, facing + Vector2.down, 7f, _physicsLayer);
			Easily.Instantiate(debugObject, transform.position + (Vector3)(facing + Vector2.down) * 4f);

			if (hit.collider == null) // did not find ground
			{
				doMove = false;
				_gotoPoint = transform.position;
				// TODO: reset target?
			}
		}

		if (doMove)
		{
			if (_rigidbody.velocity.x < speed)
			{
				_rigidbody.velocity = new Vector2(facing.x * speed, _rigidbody.velocity.y);
			}
			else
			{

			}
		}
	}

	public void ThisHurtYou(GameObject that)
	{
		_targetPoint = that.transform.position;
		_lastSawCharacter = Time.time;
	}

	protected virtual void MakeRandomChoice( List<RandomChoice> choices
		                                     , float? rando = null
		                                     , int i = 0
		                                     , float previousChance = 0
		                                     , float? totalChance = null ) // TODO: this probably doesn't need to be recursive
	{
		if(totalChance == null)
		{
			totalChance = 0;
			foreach(RandomChoice c in choices)
			{
				totalChance += c.Chance;
			}
		}

		if (rando == null)
		{
			rando = Random.Range(0f, (float)totalChance);
		}

		string choice;

		if (totalChance - previousChance > 0)
		{
			if (rando < choices[i].Chance + previousChance)
			{
				choice = choices[i].Choice;

				if (choice != null)
				{
					nextRandomChoice += randomChoiceEveryNSeconds;
					Type thisType = this.GetType();
					MethodInfo theMethod = thisType?.GetMethod(choice);
					theMethod?.Invoke(this, null); // null ref exception?
				}
			}
			else
			{
				MakeRandomChoice( choices
								, rando
					            , i + 1
								, previousChance + choices[i].Chance
								, totalChance );
			}
		}
	}

	protected bool HitWall(Collision2D collision)
	{
		// logic taken from https://gamedev.stackexchange.com/questions/11782/have-a-simple-enemy-detecting-he-touch-a-wall-to-make-him-stop-turn-around
		foreach (ContactPoint2D contactPoint in collision.contacts)
		{
			float dot = Vector3.Dot(contactPoint.normal, transform.up);
			if (dot < 1.0f && dot > -1.0f)
			{
				return true;
			}
		}

		return false;
	}

	protected bool HitCeiling(Collision2D collision)
	{
		foreach (ContactPoint2D contactPoint in collision.contacts)
		{
			float dot = Vector3.Dot(contactPoint.normal, transform.right);
			Vector2 diff = (Vector2)transform.position - collision.contacts[0].point;
			if (dot < 1.0f && dot > -1.0f && diff.y < 0)
			{
				return true;
			}
		}

		return false;
	}

	protected bool HitFloor(Collision2D collision)
	{
		foreach (ContactPoint2D contactPoint in collision.contacts)
		{
			float dot = Vector3.Dot(contactPoint.normal, transform.right);
			Vector2 diff = (Vector2)transform.position.normalized - collision.contacts[0].point.normalized;
			if (dot > 1.0f && dot < -1.0f && diff.y < 0)
			{
				return true;
			}
		}

		return false;
	}
}

[Serializable]
public class RandomChoice
{
	[SerializeField] public string Choice;
	[SerializeField] public float Chance;
}
