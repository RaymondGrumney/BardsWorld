using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObj : MonoBehaviour {

	[Tooltip("The targets of the script.")]
	public GameObject[] target;

	[Tooltip("The targets of the script.")]
	public float minMassToActivate = 1;

	// the current sum of the masses on the button
	private float _currentMassOnButton;

	[Range(0,1)]
	/// <summary>
	/// Additional drag for thrown objects on this button.
	/// </summary>
	public float additionalDrag;

	public bool staysActivated = true;
	private bool _activated = false;

	private void OnDrawGizmos()
	{
		foreach(GameObject t in target)
		{
			Gizmos.DrawLine(transform.position, t.transform.position);
		}
	}


	// when an object enters the trigger, add its mass
	void OnTriggerEnter2D(Collider2D other){
		adjustMass( other.attachedRigidbody.mass );
	}

	void OnTriggerStay2D(Collider2D other) {
		Attributes attributes = other.GetComponent<Attributes>();

		if (attributes) {
			if (attributes.slowOnButton) {
				other.attachedRigidbody.velocity *= 1 - additionalDrag;
				other.attachedRigidbody.angularVelocity *= 1 - additionalDrag;
			}
		}
	}

	// when an object leaves the trigger, subtract its mass
	void OnTriggerExit2D(Collider2D other){
		adjustMass( - other.attachedRigidbody.mass );
	}

	/// <summary>
	/// Adjusts the mass on the button and checks against the minimum.
	/// </summary>
	/// <param name="mass">Mass of the object.</param>
	void adjustMass( float mass )
	{
		_currentMassOnButton += mass;

		notifyTargets( _currentMassOnButton >= minMassToActivate );
	}

	protected void notifyTargets(bool state)
	{
		if( !(staysActivated && _activated) )
		{
			for (int i = 0; i < target.Length; i++)
			{
				target[i].GetComponent<ButtonTarget>().Activate(state);
			}

			_activated = true;
		}
	}
}
