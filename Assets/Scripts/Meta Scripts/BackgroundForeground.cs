using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundForeground : MonoBehaviour {

	[Tooltip("The speed at which this object scrolls (in additon to the master scroll speed set by the camera)")]
	public float scrollSpeed;

	// this object's initial position
	private Vector3 _initialPosition;


	// Use this for initialization
	void Start () {
		_initialPosition = transform.position;
	}

	/// <summary>
	/// This objects position at initialization.
	/// </summary>
	/// <returns>The position.</returns>
	public Vector3 initialPosition() {
		return _initialPosition;
	}
}
