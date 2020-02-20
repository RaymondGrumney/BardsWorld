using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksCharacter : BaseEnemyAI {

	private Quaternion _initialRotation;
	public GameObject pivot;
	private Vector3 _pivot;

	// Use this for initialization
	void Start() {
		initializationRoutine();
		_initialRotation = transform.rotation;
		_pivot = pivot.transform.position;
	}


	public override bool ForgetBehavior(){
		return true;

	}


	public override void DefaultBehavior(){
		// return to default angle
	}


	public override void IdleBehavior(){
		// none
	}


	public override void PursuitBehavior(){
		// track target
		transform.RotateAround( _pivot, Vector2.up, MyUtilities.AngleInDegrees( transform.position, _lastCharacterSeen.transform.position ) );
	}
}
