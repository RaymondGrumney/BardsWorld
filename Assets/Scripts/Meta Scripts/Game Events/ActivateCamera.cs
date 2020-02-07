using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCamera : GameEvent {

	private GameObject _cameraObj;

	ActivateCamera( GameObject camera ) {
		this._cameraObj = camera;
	}

	public override void onActivation()
	{
		_gameController.makeOnlyActiveCamera( _cameraObj );
	}

	public override void onUpdate()
	{
		throw new System.NotImplementedException();


	}


	public override bool canBeInterruped()
	{
		throw new System.NotImplementedException();
	}
}
