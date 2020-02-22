using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	/// <summary>
	/// The speed at which floatable objects move up. 
	/// </summary>
	public float floatiness = 1f;

	/// <summary>
	/// The multiplier on speed while in water.
	/// </summary>
	public float waterSpeedMultiplier = 0.5f;

	/// <summary>
	/// The highest point of the water.
	/// </summary>
	private float _waterLevel;

	/// <summary>
	/// The splash when an object enters.
	/// </summary>
	public GameObject splash;

	/// <summary>
	/// How quickly an object rotates toward a 90° angle.
	/// </summary>
	[Tooltip("How quickly an object rotates toward a 90° angle.")]
	[Range(0f,1f)]
	public float objectRotationSpeed = 0.01f;

	// Initialize
	void Awake() {
		_waterLevel = this.GetComponent<Collider2D>().transform.position.y + this.GetComponent<Collider2D>().bounds.extents.y;
	}

	// when an object enters the water
	void OnTriggerEnter2D(Collider2D other) {

		// damp speed
		if (other.attachedRigidbody) {
			other.attachedRigidbody.velocity *= waterSpeedMultiplier;
		}


		// faye can jump out of water, but not double jump afterward; 
		// therefore we reset double jump and let her "double" jump out of water
		DoubleJump doubleJump = other.GetComponent<DoubleJump>();

		if (doubleJump) {
			doubleJump.ResetDoubleJumped();
		}


		// set animator
		Character character = other.GetComponent<Character>();

		if (character) {
			character.Animator.SetBool( "inWater", true );
//			character.grounded = false;
			character.maxSpeed *= waterSpeedMultiplier;
		}

		// Drop carried objects
		GrabAndCarry grabAndCarry = other.GetComponent<GrabAndCarry>();

		if (grabAndCarry && grabAndCarry.carrying) {
			grabAndCarry.drop();
		}
	}


	// when an object leaves the water
	void OnTriggerExit2D(Collider2D other) {
		// set speed and animator
		Character character = other.GetComponent<Character>();

		if (character) {
			character.Animator.SetBool( "inWater", false );
			character.maxSpeed /= waterSpeedMultiplier;
		}
	}
		

	// when an object is in the water
	void OnTriggerStay2D(Collider2D other) {

		// TODO: make water mass independant. Individual floating objects should define their own level in water.

		Attributes attributes = other.GetComponent<Attributes>();

		// if the object floats, apply a force counter to gravity based on depth in water
		if (attributes && attributes.floats && other.attachedRigidbody) {
//			float otherBottom = other.transform.position.y - other.bounds.extents.y + other.attachedRigidbody.mass / 100;
//			float otherBottom = other.transform.position.y;

			// the bottom of the other collider
//			float otherBottom = other.transform.position.y;
			float otherBottom = other.transform.position.y - other.bounds.extents.y;

			// Where on the other collider the water level should be
			float otherFloatLevel = other.bounds.size.y * attributes.floatOffset;
//			float otherFloatLevel = otherBottom + other.bounds.size.y * attributes.floatOffset;

			// Current depth
//			float depth = _waterLevel - otherBottom;
			float depth = otherBottom - _waterLevel;
//			float depth = otherBottom - otherFloatLevel - _waterLevel;

			// How much force to apply upward on other object based on mass, 
			// TODO: Should mass enter into this?
//			float upwardForce = floatiness * -(_waterLevel - attributes.floatOffset * other.bounds.size.y);
//			float upwardForce = floatiness * (depth); 
			float upwardForce = floatiness * (depth - otherFloatLevel) * other.attachedRigidbody.mass * other.attachedRigidbody.gravityScale;
//			float upwardForce = floatiness * other.attachedRigidbody.mass * other.bounds.extents.y * (depth - otherFloatLevel)* other.attachedRigidbody.gravityScale;


			// add upwardForce
//			other.attachedRigidbody.AddForce( new Vector2( 0, (  floatiness * other.attachedRigidbody.mass * other.bounds.extents.y * depth * other.attachedRigidbody.gravityScale) )  );
			other.attachedRigidbody.velocity = new Vector2 ( other.attachedRigidbody.velocity.x, 
				other.attachedRigidbody.velocity.y + upwardForce );

			// set animator
			Character character = other.GetComponent<Character>();

			if (character) {
				if (!character.IsSwimming && depth > other.bounds.extents.y) {
					character.IsSwimming = true;
					character.Animator.SetBool( "inWater", true );
					character.Grounded = false;
				} 

				if(character.IsSwimming && depth < other.bounds.extents.y) {
					character.IsSwimming = false;
					character.Animator.SetBool( "inWater", false );
				}
			}


			float currentAngle = other.attachedRigidbody.rotation % 0.25f;

			// rotate toward 0
			if (currentAngle != 0f) {
				other.attachedRigidbody.rotation += objectRotationSpeed * Mathf.Sign( 0.125f - currentAngle );
			}
		}
	}
}