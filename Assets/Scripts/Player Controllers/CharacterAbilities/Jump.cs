using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MakesNoise {

	public float jumpPower;

	private Rigidbody2D _rigidBody;
	private Character _character;

	private Animator _animator;

	// Use this for initialization
	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
		_character = GetComponent<Character>();
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		// checks if the character is accepting input and can move
		if ( _character.inputCheck() && _character.canMove ) {
			// checks if the character is grounded and if the player has pressed jump
			if( _character.controllingPlayer.action2Down && _character.grounded ){
				jump();
			}
		}
	}

	void jump() {

//		_character.moveVector +=  new Vector2( 0, jumpPower );
		_rigidBody.velocity =  new Vector2( _rigidBody.velocity.x, jumpPower );

		playNoise();

		_animator.SetTrigger( "Jump" );
	}
}
