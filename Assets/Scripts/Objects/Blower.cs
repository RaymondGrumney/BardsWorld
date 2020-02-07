using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object that applies a force to objects in it's trigger.
/// </summary>
public class Blower : MonoBehaviour {


	/// <summary>
	/// The angle the blower moves objects at.
	/// </summary>
	[Tooltip("The angle the blower moves objects at.")]
	[Range(0,360)]
	public float blowAngle = 0;

	/// <summary>
	/// The force at which the blower pushes objects.
	/// </summary>
	[Tooltip("The force at which the blower pushes objects.")]
	public float blowForce = 1;

	// When an object is in contact with the attached trigger
	void OnTriggerStay2D(Collider2D other) {
		// get the rigidbody and character script of the other
		Rigidbody2D rigidbody = other.GetComponent<Rigidbody2D>();
		Moveable moveable = other.GetComponent<Moveable>();



		// if the object is a character
//		if ( moveable ) {
//
//			// if it's blob, tell the blob not to stick
//			Blob blob = other.GetComponent<Blob>();
//
//			if (!blob || !blob.grounded) {
//
//			// calculate the move direction and pass it to the character
//				Vector2 moveDir = calcMoveDir();
//				moveable.moveVector = moveDir * blowForce;
//			}
//
//		// if it's not a character but it has a rigidbody
//		} else 
			if (rigidbody) {

			// calculate the move direction and apply it to the rigidbody
			Vector2 moveDir = calcMoveDir();

			rigidbody.velocity = ( (Vector2) rigidbody.velocity + ( moveDir * blowForce ) );
		}
	}

	void OnTriggerExit2D(Collider2D other) {

	}

	/// <summary>
	/// Calculates the move direction.
	/// </summary>
	/// <returns>The move direction.</returns>
	protected Vector2 calcMoveDir() {
		return MyUtilities.NormalizedVectorFromAngle( blowAngle + transform.localRotation.eulerAngles.z );
	}
}

