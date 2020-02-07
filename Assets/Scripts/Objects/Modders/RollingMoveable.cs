using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingMoveable : MonoBehaviour {

	/// <summary>
	/// This object's rigidbody.
	/// </summary>
	protected Rigidbody2D _rigidbody;

	protected Moveable _moveable;

	// initialization
	protected void Awake () {
		_rigidbody = GetComponent<Rigidbody2D>();
		_moveable = GetComponent<Moveable>();
	}
	/// <summary>
	/// How fast the object spins
	/// </summary>
	public float spinSpeed;
	
	// Update is called once per frame
	void Update () {
		float adjustment = 0f;

		if (_moveable) {
			adjustment = _moveable.moveVector.x;
		}


		_rigidbody.angularVelocity = ( _rigidbody.velocity.x - adjustment ) * -spinSpeed;

	}
}
