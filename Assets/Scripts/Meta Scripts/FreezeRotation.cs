using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour {

	private Quaternion _initialRotation;

	// Use this for initialization
	void Start () {
		_initialRotation = this.transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		this.transform.rotation = _initialRotation;
	}
}
