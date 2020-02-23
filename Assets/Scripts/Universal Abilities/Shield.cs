using CommonAssets;
using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Tooltip("How much it bounces the attacker back on attack.")]
    public Vector2 knockBackForce = DefaultValues.KnockBackForce;

    [Tooltip("How long the attacker gets stunned for.")]
    public float stunLength = 0.25f;


    [Tooltip("How much it bounces the defender back on attack.")]
    public Vector2 selfKnockBackForce = DefaultValues.SelfKnockBackForce;

    [Tooltip("How long the defender gets stunned for.")]
    public float selfStunLength = 0.1f;

    public AudioClip impactSound;

    private float ShieldDownUntil = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( Time.time > ShieldDownUntil && collision.CompareTag("Attack") )
        {
            Easily.Knock(collision).Back(knockBackForce).From(gameObject).StunningFor(stunLength);
            Easily.Knock(GetComponentInParent<Rigidbody2D>())
                  .Back(selfKnockBackForce)
                  .From(collision)
                  .StunningFor(selfStunLength);

            collision.SendMessage("DisableDamage");

            // Play Impact Sound
            SoundHelper.Play(impactSound, transform.position);
        }
    }

    public void DropShield(float forNSeconds)
    {
        ShieldDownUntil = Time.time + forNSeconds;
    }
}
