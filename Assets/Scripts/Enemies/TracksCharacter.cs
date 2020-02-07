using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksCharacter : BaseEnemy {

	private Quaternion _initialRotation;
	public GameObject pivot;
	private Vector3 _pivot;

	// Use this for initialization
	void Start() {
		initializationRoutine();
		_initialRotation = transform.rotation;
		_pivot = pivot.transform.position;
	}


	protected override bool forgetBehavior(){
		return true;

	}


	protected override void defaultBehavior(){
		// return to default angle
	}


	protected override void idleBehavior(){
		// none
	}


	protected override void pursuitBehavior(){
		// track target
		transform.RotateAround( _pivot, Vector2.up, MyUtilities.AngleInDegrees( transform.position, _lastCharacterSeen.transform.position ) );
	}
}
