using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezeyDistort : MonoBehaviour {

	[Tooltip("The scale of the distortion to the x axis.")]
	/// <summary>
	/// The scale of the distortion to the x axis.
	/// </summary>
	public float xDistort = 0.1f;

	[Tooltip("The scale of the distortion to the y axis.")]
	/// <summary>
	/// The scale of the distortion to the y axis.
	/// </summary>
	public float yDistort = 0.1f;

	[Tooltip("How many seconds it takes to complete an in/out cycle on the x axis.")]
	/// <summary>
	/// How many seconds it takes to complete an in/out cycle on the x axis.
	/// </summary>
	public float xPeriod = 1f;

	[Tooltip("How many seconds it takes to complete an in/out cycle on the y axis.")]
	/// <summary>
	/// How many seconds it takes to complete an in/out cycle on the y axis.
	/// </summary>
	public float yPeriod = 1f;

	private float _height;
	private float _width;

	// initialize
	void Start() {
		_height = transform.localScale.y;
		_width = transform.localScale.x;
	}

	// Update is called once per frame
	void Update () {
		float x = _width + MyUtilities.Oscillation( xDistort, xPeriod, 0, Time.time );
		float y = _height + MyUtilities.Oscillation( yDistort, yPeriod, 0, Time.time );

		transform.localScale = new Vector3(x, y, transform.localScale.z);
	}
}
