using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the camera to a target object.
/// </summary>
public class MoveCamera : GameEvent {

	[Tooltip("The object to target with the camera.")]
	/// <summary>
	/// The object to target with the camera.
	/// </summary>
	public GameObject target;


	[Tooltip("The camera to move.")]
	/// <summary>
	/// The camera to move.
	/// </summary>
	public GameObject cameraObj;


	[Tooltip("If the game controller should hide other cameras before returning.")]
	/// <summary>
	/// The camera to move.
	/// </summary>
	public bool useOnlyThisCamera;


	[Tooltip("A MoveCamera object to hand the previous target info to.")]
	/// <summary>
	/// A MoveCamera object to hand the previous target info to.
	/// </summary>
	public MoveCamera returningCameraObject;


	// triggers next event stage after timeToView elapses ( + an additional delay? )

	public override void onUpdate()
	{
		throw new System.NotImplementedException();

		// if we reach the target, move to next event
	}

	public override void onActivation()
	{

		CameraController cameraController = cameraObj.GetComponent<CameraController>();

		if ( useOnlyThisCamera ) {
			createActivateCamera();
		}

		// create returning camera to return to character at end of chain
		createReturningCamera( cameraController.target );

		// set camera target
		cameraController.target = this.target;

	}


	public override bool canBeInterruped()
	{
		throw new System.NotImplementedException();
	}

	/// <summary>
	/// Creates an event to return the camera target to it's original target at the end of a event chain.
	/// </summary>
	/// <param name="target">The caera's original target.</param>
	private void createReturningCamera( GameObject target ) {
		throw new System.NotImplementedException();

		// find the last event in the chain
		GameEvent lastEvent = getLastEventInChain();


		// assuming the last event isn't already a returnin camera, create one targetting the given object
		if (lastEvent.GetType() != typeof(ReturningCamera)) {

			// create event object with ReturningCamera event
			GameObject returningCameraObject = new GameObject();
			returningCameraObject.name = "Returning Camera";

			ReturningCamera returningCamera = returningCameraObject.AddComponent<ReturningCamera>() as ReturningCamera;
			returningCamera = new ReturningCamera( cameraObj, target );
		}
	}

	/// <summary>
	/// Gets the last event in the event chain.
	/// </summary>
	/// <returns>The last GameEvent.</returns>
	private GameEvent getLastEventInChain()
	{
		throw new System.NotImplementedException();

		// temporary event 
		GameEvent workingEvent = nextEvent;

		// travel down the chain to the last event
		while( workingEvent.nextEvent ) {
			workingEvent = workingEvent.nextEvent;
		}

		// return the last event
		return workingEvent;
	}


				private void createActivateCamera(  ) {
					
		
		
	}
}
