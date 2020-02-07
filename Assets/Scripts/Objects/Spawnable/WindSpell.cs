using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpell : Throwable {

	public GameObject spawnable;

	protected Rigidbody2D _rigidbody;
	protected Collider2D _collider; 

	// instantiate
	void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<Collider2D>();
	}

	void Update() {
//		if (_rigidbody.simulated) {
//			Destroy( this.gameObject );
//		}
	}

	/// <summary>
	/// Raises the collision enter 2d event.
	/// </summary>
	/// <param name="other">The other Collider.</param>
	void OnCollisionEnter2D(Collision2D other) {
		if (!other.gameObject.GetComponent<Moveable>() && !other.gameObject.GetComponent<Attributes>().carriable && this.enabled) {
			this.enabled = false;

			// spawn Blast at same position as this object 
			GameObject spawnedObject = Instantiate( spawnable, this.transform.parent );
			spawnedObject.transform.position = this.transform.position;

			// rotate based on impact
			Vector2 impactNormal = other.contacts[ 0 ].normal;
			spawnedObject.transform.eulerAngles = new Vector3( 0, 0, MyUtilities.AngleInDegrees( impactNormal ) - 90 );

			// destroy this game object
			Destroy( this.gameObject );
		}
	}

	public override void beenThrown()
	{
		this.gameObject.layer = LayerMask.NameToLayer( "Passthrough 2" );
	}
}
