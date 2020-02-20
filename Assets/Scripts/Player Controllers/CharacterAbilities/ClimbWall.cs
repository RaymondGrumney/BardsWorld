using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbWall : MonoBehaviour {

	public float climbSpeed;
	Character playerObj;

	// Use this for initialization
	void Start () {

		playerObj = GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if ( playerObj.InputCheck() ) {
			
		}
	}
}
