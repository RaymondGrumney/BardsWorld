using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeOnContact : MonoBehaviour {

	public GameObject iceRaft;

	// when an object comes into contact with this object
	void OnCollisionEnter2D(Collision2D other) {
		BaseEnemyAI enemy = other.gameObject.GetComponent<BaseEnemyAI>();

		if(enemy) {
			// freeze the enemy
			Destroy( this.gameObject );
			// TODO: freeze enemy
		}
	}

	void OnTriggerEnter2D(Collider2D other) {

		Water water = other.gameObject.GetComponent<Water>();

		// spawn an ice raft in water
		if (water) {
			GameObject raft = Instantiate( iceRaft );

			raft.transform.position = this.transform.position;
			raft.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity;

			Destroy( this.gameObject );
		}
	}

//	void OnDestroy(){}
}
