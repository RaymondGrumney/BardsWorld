﻿using CommonAssets.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base enemy AI.
/// </summary>
public abstract class BaseEnemy : MonoBehaviour 
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
	/// The last character seen.
	/// </summary>
	protected GameObject _lastCharacterSeen;

	/// <summary>
	/// When the character was last seen
	/// </summary>
	protected float _lastSawCharacter;

	/// <summary>
	/// Where the character was last seen
	/// </summary>
	protected Vector2 _targetPoint; // TODO: this should be replaced by _gotoPoint?

	/// <summary>
	/// The collider attached to this game object.
	/// </summary>
	protected Collider2D _collider;


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


	// Use this for initialization
	void Awake () {
		initializationRoutine();
	}

	/// <summary>
	/// Initializes references for this object.
	/// </summary>
	protected void initializationRoutine() {
		_startingPoint = transform.position;
		_gotoPoint = transform.position;
		_targetPoint = transform.position;
		_physicsLayer = gameObject.layer;

		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<Collider2D>();
		//_spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		if ( ForgetBehavior() ) {
			DefaultBehavior();
		} else {
			PursuitBehavior();
		}
	}
		

	/// <summary>
	/// Checks if the enemy has forgotten the character and performs the forget behavior if so
	/// </summary>
	/// <returns><c>true</c>, the enemy has forgotten the character, <c>false</c> otherwise.</returns>
	protected abstract bool ForgetBehavior();

	/// <summary>
	/// Ehat to do if not pursuing the player
	/// </summary>
	protected abstract void DefaultBehavior();

	/// <summary>
	/// Performs the idle behavior.
	/// </summary>
	protected abstract void IdleBehavior();

	/// <summary>
	/// Check if are pursuing a character and performs pursuit behavior if so
	/// </summary>
	protected abstract void PursuitBehavior();


	/// <summary>
	/// Sets this enemy's facing to either right (1) or left (-1).
	/// </summary>
	/// <param name="newFacing">New facing.</param>
	protected void SetFacing( float newFacing ) 
	{	
		newFacing = Mathf.Sign( newFacing );

		if ( facing.x != newFacing ) {
			facing.x = newFacing;

			// rotate Y to 180 if facing left or to 0 if facing right	
			Easily.Flip(gameObject).On("yAxis");
		}
	}

	/// <summary>
	/// Faces the target point.
	/// </summary>
	protected void FaceTargetPoint() 
	{
		SetFacing( Mathf.Sign( ( (Vector3) _targetPoint - transform.position ).x ) );
	}

	/// <summary>
	/// Faces the target.
	/// </summary>
	protected void FaceTarget() 
	{
		SetFacing( Mathf.Sign( ( _lastCharacterSeen.transform.position - transform.position ).x ) );
	}

	/// <summary>
	/// Sets the target .
	/// </summary>
	/// <param name="targetPoint">Target point.</param>
	public virtual void SetTarget(GameObject target) 
	{
		_pursuing = true;
		_lastCharacterSeen = target;
		_targetPoint = target.transform.position;
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
	protected void MoveForward() 
	{
		if (_rigidbody.velocity.x < speed) 
		{
			_rigidbody.velocity = new Vector2( facing.x * speed, _rigidbody.velocity.y);
		}
	}
}
