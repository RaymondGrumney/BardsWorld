using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An attack. Can be used on enemies (using a trigger), or on characters ( will need to call to performAttack() ).
/// </summary>
public class ProjectileAttack : MonoBehaviour {

	[Tooltip("The projectile / damage object.")]
	public GameObject attackObject;

	[Tooltip("Where the attack spawns.")]
	public GameObject spawnPoint;
	protected Collider2D _spawnPoint;
	
	[Tooltip("The amount of time between attacks (in seconds).")]
	public float attackCooldown = 0.5f;
	
	// when we can next perform an attack
	protected float nextAttack = 0f;

	// Use this for initialization
	void Start () {
		getComponents();
	}


	void getComponents() {
		// TODO: this doesn't move with the parent?
		_spawnPoint = spawnPoint.GetComponent<Collider2D>();

	}


	// When the player enters the trigger
	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player" && nextAttack < Time.time) {
			performAttack( other.bounds.center );
		}
	}

	/// <summary>
	/// Performs the attack.
	/// </summary>
	public void performAttack( Vector2 vector ) {

		// make the attack object
		GameObject g = Instantiate( attackObject, _spawnPoint.bounds.center, Quaternion.identity );
		TargetedMovement t = g.GetComponent<TargetedMovement>();

		if (t != null) {
			t.target( vector - (Vector2) g.transform.position );
		}

		// change it's Y rotation to this object's Y rotation
		g.transform.Rotate( 0, this.transform.rotation.y, 0 );

		// set the attack object's layer to this object's layer to avoid friendly fire
		g.layer = this.gameObject.layer;

		// set nextAttack
		nextAttack = Time.time + attackCooldown;
	}
}
