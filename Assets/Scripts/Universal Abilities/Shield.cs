using CommonAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Tooltip("How much it bounces the attacker back on attack.")]
    public Vector2 knockBackForce = DefaultValues.KnockBackForce;
    [Tooltip("How much it bounces back on attack (multiplied by attack value.")]
    public float stunLength = 0.25f;

    private float ShieldDownUntil = 0;


    public AudioClip impactSound;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( Time.time > ShieldDownUntil && collision.CompareTag("Attack") )
        {
            KnockBack.Knock(collision).Back(knockBackForce).From(gameObject);

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
