using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : ButtonTarget {

	public float[] rotations;
	public float rotationPerFrameInDegrees = 1f;

	private bool _rotating = false;
	private int _currentRotation = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_rotating) {
			if (( transform.localRotation.eulerAngles.z ) - rotations[ _currentRotation ] < 0f ) {

			}
		}
	}

	public override void Activate(bool state) {
		if (!_rotating && state) {
			_rotating = true;
			_currentRotation = (_currentRotation + 1) % rotations.Length;
		}
	}



	/// <summary>
	/// The angle in degrees between two vector2s
	/// </summary>
	/// <returns>The angle in degrees.</returns>
	/// <param name="a">The first Vector2</param>
	/// <param name="b">The second Vector2</param>
	private float myAngleInDegrees(Vector2 a, Vector2 b) {

		Vector2 c = a - b;
		float angle = Mathf.Atan2( c.y, c.x );
		angle *= Mathf.Rad2Deg * -2;

		// do not want negative results
		if (angle < 0) {
			angle += 360;
		}

		Debug.Log( "myAngleInDegrees Returned: " + ( angle ) );
		return angle;
	}

	/// <summary>
	/// The angle in degrees between two vector2s
	/// </summary>
	/// <returns>The angle in degrees.</returns>
	/// <param name="a">The first Vector2</param>
	/// <param name="b">The second Vector2</param>
	private float myAngleInDegrees(Vector3 a, Vector3 b) {
		return myAngleInDegrees( (Vector2) a, (Vector2) b );
	}


	/// <summary>
	/// The angle in degrees between two vector2s
	/// </summary>
	/// <returns>The angle in degrees.</returns>
	/// <param name="a">The first Vector2</param>
	/// <param name="b">The second Vector2</param>
	private float myAngleInDegrees(Vector2 a, Vector3 b) {
		return myAngleInDegrees( a, (Vector2) b );
	}


	/// <summary>
	/// The angle in degrees between two vector2s
	/// </summary>
	/// <returns>The angle in degrees.</returns>
	/// <param name="a">The first Vector2</param>
	/// <param name="b">The second Vector2</param>
	private float myAngleInDegrees(Vector3 a, Vector2 b) {
		return myAngleInDegrees( (Vector2) a, b );
	}
}
