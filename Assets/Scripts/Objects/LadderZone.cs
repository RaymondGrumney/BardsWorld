using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderZone : MonoBehaviour {

	// the ladder layer of the Physics engine
	// we use the ladder layer to ignore pass-through objects (floors, usuall) while on the ladder
//	private int _ladderLayer;

	// initialize this
	void Start(){
//		_ladderLayer = this.gameObject.layer;
	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerStay2D(Collider2D other) {
//		ClimbLadder climbLadder = other.GetComponent<ClimbLadder>();
//
//		if( climbLadder ) {
//			climbLadder.setOnLadder( true );
//		}
	}

	/// <summary>
	/// Raises the trigger exit2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerExit2D(Collider2D other) {
//		ClimbLadder climbLadder = other.GetComponent<ClimbLadder>();
//
//		if( climbLadder ) {
//			climbLadder.setOnLadder( false );
//		}


	}

}
