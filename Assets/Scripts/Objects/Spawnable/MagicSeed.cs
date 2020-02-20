using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSeed : MonoBehaviour {
	
	/// <summary>
	/// This object's Rididbody2D
	/// </summary>
	private Rigidbody2D rigidbody;

	/// <summary>
	/// The ladder object to spawn upon hitting the stage.
	/// </summary>
	[Tooltip("The ladder object to spawn upon hitting the stage.")]
	public GameObject ladderSpawn;

	/// <summary>
	/// The cage object to spawn upon hitting an enemy.
	/// </summary>
	[Tooltip("The cage object to spawn upon hitting an enemy.")]
	public GameObject cageSpawn;

	// Use this for initialization
	void Start () {
		rigidbody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// upon colliding with another collider
	void OnCollisionEnter2D(Collision2D other){

		// check if enemy
		BaseEnemyAI enemy = other.gameObject.GetComponent<BaseEnemyAI>();

		if (enemy) {
			spawnCage( enemy );
		} else {

			// otherwise check if spawn ladder here
			spawnLadder(other);
		}
	}

	void spawnCage(BaseEnemyAI enemy) {
		// TODO: this
	}

	void spawnLadder(Collision2D other) {
		bool canGrow = other.gameObject.GetComponent<Attributes>().grows;

		if(ladderSpawn && canGrow) {

			// grow the ladder in the direction of the normal of the collision
			Vector3 normal = other.contacts[ 0 ].normal;

			GameObject ladder = Instantiate( ladderSpawn );

			// set position and angle of ladder
			float zAngle = MyUtilities.AngleInDegrees( Vector2.zero, normal * -1 );

			ladder.transform.eulerAngles = new Vector3( 0, 0, zAngle + 90 );
			ladder.transform.position = new Vector3(other.contacts[0].point.x,
													other.contacts[0].point.y,
													this.transform.position.z
												);

			// set lastObjectSpawned
			setLastObjectSpawned( ladder );

			Destroy( this.gameObject );
		}
	}

	/// <summary>
	/// Sets the last object spawned field of the parent power.
	/// </summary>
	/// <param name="ladder">The spawned Ladder.</param>
	static void setLastObjectSpawned( GameObject ladder )
	{
		// get list of powers from Faye
		Magicks powers = GameObject.Find( "Faye" ).GetComponent<Magicks>();
		GameObject[] powerList = powers.powers;

		// find Magic Seed Power(s)
		foreach( GameObject obj in powerList ) {
			
			SpawnPower power = obj.GetComponent<SpawnPower>();

			if (power && power.spawnable.name == "Magic Seed") {
				// set lastObjectSpawned to this ladder
				power.lastObjectSpawned = ladder;
			}
		}
	}
}
