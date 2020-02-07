using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRaft : MonoBehaviour {

	// TODO expand out to initial size starting from width of ice spell

	/// <summary>
	/// This object's Rididbody2D
	/// </summary>
	private Rigidbody2D rigidbody;

	[Tooltip("The bounce multiplier when contacting other ice rafts.")]
	/// <summary>
	/// The bounce multiplier when contacting other ice rafts.
	/// </summary>
	public float otherBounce = 0.25f;

	void Start() {
		rigidbody = this.GetComponent<Rigidbody2D>();
	}

	void Update() {

	}

	void OnCollisionEnter2D(Collision2D other) {
		IceRaft otherRaft = other.gameObject.GetComponent<IceRaft>();

		if (otherRaft) {
			rigidbody.velocity  = - otherBounce  *  (otherRaft.transform.position - this.transform.position);
		}

	}

	void OnCollisionStay2D(Collision2D other) {
		IceRaft otherRaft = other.gameObject.GetComponent<IceRaft>();

		if (otherRaft) {
			rigidbody.velocity = - (otherRaft.transform.position - this.transform.position);
		}
	}
}
