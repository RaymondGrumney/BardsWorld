using CommonAssets.Utilities;
using UnityEngine;

/// <summary>
/// An attack. Can be used on enemies (using a trigger), or on characters ( will need to call to performAttack() ).
/// </summary>
public class MeleeAttack: MonoBehaviour {

	[Tooltip("The damage object.")]
	public GameObject attackObject;

	[Tooltip("Where the attack spawns.")]
	public GameObject spawnPoint;

	[Tooltip("The amount of time between attacks (in seconds).")]
	public float attackCooldown = 0.5f;

	[Tooltip("A hack to account for prototype's facing.")]
	public Vector3 rotationFix;

	// when we can next perform an attack
	protected float nextAttack = 0f;

	// Use this for initialization
	private void Start () 
	{
		GetComponents();
	}


	private void GetComponents()
	{

	}


	// When the player enters the trigger
	private void OnTriggerStay2D(Collider2D other) 
	{
		if (other.CompareTag("Player") && nextAttack < Time.time) 
		{
			PerformAttack( other.bounds.center );
		}
	}

	/// <summary>
	/// Performs the attack.
	/// </summary>
	private void PerformAttack( Vector2 vector ) 
	{
		// make the attack object
		GameObject g = Easily.Instantiate( attackObject, spawnPoint.transform.position );
		g.transform.parent = gameObject.transform;
		g.transform.rotation = gameObject.transform.rotation;
		//g.transform.Rotate(rotationFix);

		//TargetedMovement t = g.GetComponent<TargetedMovement>();

		//if (t != null) {
		//	t.target( vector - (Vector2) g.transform.position );
		//}

		// set the attack object's layer to this object's layer to avoid friendly fire
		g.layer = this.gameObject.layer;

		// set nextAttack
		nextAttack = Time.time + attackCooldown;
	}
}
