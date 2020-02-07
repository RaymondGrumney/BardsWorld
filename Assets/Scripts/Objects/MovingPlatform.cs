using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public GameObject startPosition;
	public GameObject endPosition;
	public GameObject platform;

	public float _frequency;

//	[Tooltip("The speed at which the object moves in terms of game units per fixed delta time.")]
//	public float speed = 1f;

	private Vector3 _startPosition;
	private Vector3 _endPosition;
	private Transform _platformTransform;
	private Rigidbody2D _platformRigidbody;


	// Use this for initialization
	void Awake  () {
		initializeVariables();
	}


	protected void initializeVariables() {
		_startPosition = startPosition.GetComponent<Collider2D>().bounds.center;
		_endPosition = endPosition.GetComponent<Collider2D>().bounds.center;

		_platformTransform = platform.transform;
		_platformRigidbody = platform.GetComponent<Rigidbody2D>();

		// calculate frequency
//		float pathDistance = Vector3.Distance( _startPosition, _endPosition );
//		float speedPerDeltaTime = speed * Time.fixedDeltaTime;							// convert speed to per fixed delta time
//		_frequency = pathDistance * speedPerDeltaTime;									// frequency = 
	}


	// Update is called once per frame
	void FixedUpdate () {
		movePlatform();
	}

	/// <summary>
	/// Moves the platform.
	/// </summary>
	protected void movePlatform() {
		
		// calculate, as a number between 1 and -1, the total offset from the midpoint
		float offsetFromMidpoint = MyUtilities.Oscillation( 1, _frequency, 0, Time.time );


		// make that value a value between 0 and 1 with 0.5 being the middle
		offsetFromMidpoint *= 0.5f;
		offsetFromMidpoint += 0.5f;

		// calculate and assign new position
		Vector3 newPosition = Vector3.Lerp( _startPosition, _endPosition, offsetFromMidpoint);

//		_platformTransform.position = newPosition;
		Vector3 newVelocity = ( newPosition - _platformTransform.position );
		_platformRigidbody.velocity = newVelocity;
	}
}
