using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnImpact : MonoBehaviour {

	public float forceToDestroy;
	private Rigidbody2D _rigidbody;

	void Start() {
		startRoutine();
	}

	void startRoutine() {
		_rigidbody = this.GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D(Collision2D other) {
		// TODO: account for bounciness on this and other's physics material

		Debug.Log( "Plink : " + _rigidbody.gameObject.name);
		Debug.Log( "Plink : x = " + _rigidbody.velocity.x );
		Debug.Log( "Plink : y = " + _rigidbody.velocity.y );
		Debug.Log( "Plink : mass =" + _rigidbody.mass ) ;
		Debug.Log( "Plink : result = " + ( Mathf.Abs( ( _rigidbody.velocity.x + _rigidbody.velocity.y) * _rigidbody.mass ) ) );

		if( Mathf.Abs ( this._rigidbody.velocity.x ) + Mathf.Abs( this._rigidbody.velocity.y) * this._rigidbody.mass > forceToDestroy ) {
			Destroy( this.gameObject );
		}
	}
}
