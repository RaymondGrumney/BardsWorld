using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForward : TargetedMovement {

	public float speed;
	protected Rigidbody2D _rigidbody;
	protected Vector2 _direction;

	// Use this for initialization
	void Start () {
		startRoutine();
	}

	private void startRoutine(){
		_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		_rigidbody.velocity = _direction;
	}

	public override void target(Vector2 target) {
		_direction = target.normalized * speed;

		//_actualSpeed = new Vector2( Mathf.Clamp( movement.x, -speed.x, speed.x), Mathf.Clamp( movement.y, -speed.y, speed.y ) );
	}
}