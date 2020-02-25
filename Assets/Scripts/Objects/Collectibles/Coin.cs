using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	[Tooltip("How fast the coin rotates.")]
	public float rotationSpeed = 2;

	void Awake() 
	{
//		_level = GameObject.Find( "Level Info" ).GetComponent<Level>();
	}

	private void FixedUpdate()
	{
		transform.Rotate(0, rotationSpeed, 0);
	}

	public void Collect(GameObject other)
	{
		// TODO: this?
//		_level.addCollectible(this);
	}
}
