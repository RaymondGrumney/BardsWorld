using CommonAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : MonoBehaviour
{
    [Tooltip("How much it bounces the attacker back on attack.")]
    public Vector2 knockBackForce = DefaultValues.KnockBackForce;
    [Tooltip("How much it bounces back on attack (multiplied by attack value.")]
    public float stunLength = 0.25f;

    public AudioClip impactSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // bounce it back
        // determine if bounce left or right
        //Vector2 them = collision.attachedRigidbody.transform.position;
        //float diff = Mathf.Sign(transform.position.x - them.x);

        //Vector2 theirVelocity = collision.attachedRigidbody.velocity;


        //collision.attachedRigidbody.velocity = new Vector2(-knockBackForce.x * diff, theirVelocity.y + knockBackForce.y);

        KnockBack.Knock(collision).Back(knockBackForce).From(gameObject);

        // TODO: this should
        collision.SendMessageUpwards("Stun", stunLength);
        collision.SendMessage("DisableDamage");

        // Play Impact Sound
        SoundHelper.Play(impactSound, transform.position);
    }
}
