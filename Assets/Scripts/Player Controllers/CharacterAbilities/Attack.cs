using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An attack. Can be used on enemies (using a trigger), or on characters ( will need to call to performAttack() ).
/// </summary>
public class Attack : MonoBehaviour
{
	[Tooltip("The projectile / damage object.")]
	public GameObject attackObject;

	[Tooltip("Where the attack spawns.")]
	public GameObject spawnPoint;

	protected Animator _animator;

	[Tooltip("The amount of time between attacks (in seconds).")]
	public float attackCooldown = 0.5f;

	// when we can next perform an attack
	protected float nextAttack = 0f;

	// Use this for initialization
	void Start()
	{
		GetComponents();
	}

	void GetComponents()
	{
		// TODO: this doesn't move with the parent?
		_animator = GetComponentInParent<Animator>();
	}


	void Update()
	{
		if (Joypad.Read.Buttons.Held("attack") && nextAttack < Time.time)
		{
			performAttack();
		}
	}

	/// <summary>
	/// Performs the attack.
	/// </summary>
	public void performAttack()
	{
		// make the attack object
		GameObject spawnedAttack = Instantiate(attackObject, spawnPoint.transform);

		// change it's Y rotation to this object's Y rotation
		spawnedAttack.transform.Rotate(0, this.transform.rotation.y, 0);

		// set the attack object's layer to this object's layer to avoid friendly fire
		spawnedAttack.layer = this.gameObject.layer;

		// set next attack time
		nextAttack = Time.time + attackCooldown;
	}
}
