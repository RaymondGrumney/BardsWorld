using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturningCamera : MonoBehaviour {

	private GameObject _camera;
	private GameObject _target;


	public ReturningCamera( GameObject camera, GameObject target) {
		this._camera = camera;
		this._target = target;
	}
}
