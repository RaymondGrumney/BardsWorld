using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ButtonTarget {

	[Tooltip("Where the object moves to when 'Open', relative to it's starting position.")]
	/// <summary>
	/// The offset from the object's original position when open.
	/// </summary>
	public Vector3 openOffset;


	[Tooltip("The speed at which the object opens.")]
	/// <summary>
	/// The speed at which the object opens.
	/// </summary>
	public float openSpeed;

	[Tooltip("The speed at which the object closes.")]
	/// <summary>
	/// The speed at which the object closes.
	/// </summary>
	public float closeSpeed;

	/// <summary>
	/// The initial position of the object.
	/// </summary>
	private Vector3 _initialPosition;
//
//	/// <summary>
//	/// The calculated open position.
//	/// </summary>
//	private Vector3 _openPosition; // TODO: this shouldn't be pre-calculated

	/// <summary>
	/// If the door has been activated
	/// </summary>
	private bool _activated = false;

	/// <summary>
	/// If the door is stationary
	/// </summary>
	private bool _stationary = true;


	// Use this for initialization
	void Start () {
		_initialPosition = transform.position;
//		_openPosition = _initialPosition + openOffset;
	}
	
	// Update is called once per frame
	void Update () {

		if (_activated) {
			Vector2 openPosition = _initialPosition + openOffset;

			moveTo( openPosition );

		} else {

			moveTo( _initialPosition );

		}
	}

	/// <summary>
	/// Moves to the given position.
	/// </summary>
	/// <param name="distance">Distance.</param>
	/// <param name="targetPositin">Target positin.</param>
	void moveTo( Vector3 targetPosition )
	{
		float distance = Vector2.Distance( transform.position, targetPosition );

		if (distance > 0.01f) {
			float x = targetPosition.x - transform.position.x;
			float y = targetPosition.y - transform.position.y;
			float z = 0;

			Vector3 movement = new Vector3( x, y, z ) * closeSpeed * Time.deltaTime;

			transform.Translate( movement );

			playNoise();
		}
	}

	public override void Activate(bool state) {
		_activated = state;
	}
}
