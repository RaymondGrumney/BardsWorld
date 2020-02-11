using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MakesNoise {

	public float doubleJumpPower;

	private bool _doubleJumped = false;
	private Rigidbody2D _myRigidBody;
	private Jump _jump;
	private Character _character;
	private GrabAndCarry _grabAndCarry;

	private Animator _animator;

	// Use this for initialization
	void Start () {
		_myRigidBody = GetComponent<Rigidbody2D>();
		_jump = GetComponent<Jump>();
		_character = GetComponent<Character>();
		_grabAndCarry = GetComponent<GrabAndCarry>();
		_animator = GetComponent<Animator>();
	}


	// Update is called once per frame
	void Update () {
		
		if ( !_character.grounded && !_doubleJumped  && Joypad.Read.Buttons.jump ) {
			// can't double jump if carrying an object
			if (_grabAndCarry) {
				if( !_grabAndCarry.carrying) {
					doubleJump();
				}
			} else {

				// if there's no grab and carry script attached, we can always double jump
				doubleJump();
			}
		}
	}

	void doubleJump()
	{
		_doubleJumped = true;
		_myRigidBody.velocity = new Vector2( _myRigidBody.velocity.x, doubleJumpPower );

		playNoise();

		_animator.SetTrigger( "Jump" );
	}

	public void resetDoubleJumped() {
		_doubleJumped = false;
	}
}
