using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : BaseEnemyAI
{
    [Header("Bat AI Settings")]
    [Tooltip("How far the bat will drop from the ceiling at the begining of pursuit.")]
    public float DistanceDropsFromCeiling = 5f;
    /// <summary>
    /// Whether the Bat is dropping (at begining of Pursuit loop)
    /// </summary>
    private bool _dropping;

    [Tooltip("How long the bat will hide after hitting the player and finding a roost.")]
    public float TimeHidesBeforePursuing = 6f;
    /// <summary>
    /// When the bat started hiding.
    /// </summary>
    private float _timeStopsHiding = 0f;
    /// <summary>
    /// If the bat is hiding.
    /// </summary>
    private bool _hiding = true;

    [Tooltip("How the bat bounces when hitting a surface during flight.")]
    public Vector2 BounceForceOnCollision = DefaultValues.KnockBackForce;
    [Tooltip("How long the Bat is inactive after hitting a surface during flight.")]
    public float StunLengthOnCollision = 0.2f;

    /// <summary>
    /// Whether the bat is fleeing the player.
    /// </summary>
    private bool _fleeing = false;

    protected override void Update()
    {
        SetFacing(Mathf.Sign(_rigidbody.velocity.x));
        base.Update();
    }

    public override void DefaultBehavior()
    {
        if (!_hiding)
        {
            MoveForward(); // flee
        }
        else
        {
            IdleBehavior();
        }
    }

    public override void IdleBehavior() // hide
    {
        // MakeRandomChoice();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_fleeing)
        {
            if ( HitCeiling(collision)
                 && collision.gameObject.CompareTag("Untagged"))
            {
                Hide();
            }
        }
        else if (_pursuing)
        {
            if ( collision.gameObject.CompareTag("Player"))
            {
                Flee();
            }
            else
            {
                BounceOffSurface();
            }
        }
    }

    /// <summary>
    /// Hide on the ceiling before pursuing again.
    /// </summary>
    private void Hide()
    {
        _fleeing = false;
        _pursuing = false;
        _hiding = true;
        _timeStopsHiding = Time.time + TimeHidesBeforePursuing;

        _gotoPoint = _rigidbody.position;
        _rigidbody.velocity = Vector2.zero;
        _animator.SetBool("Flying", false);
    }

    private void BounceOffSurface()
    {
        float reverseVelocity_Y = Mathf.Sign(_rigidbody.velocity.y);
        Vector2 approximateNextFramePosition = (Vector2)transform.position + (_rigidbody.velocity.normalized * speed);
        Vector2 fixedBounceForce = new Vector2(-BounceForceOnCollision.x, BounceForceOnCollision.y * reverseVelocity_Y);
        Easily.Knock(this.gameObject)
              .Back(fixedBounceForce)
              .From(approximateNextFramePosition)
              .StunningFor(StunLengthOnCollision);
    }

    private void Flee()
    {
        float reverseVelocity_X = -Mathf.Sign(_rigidbody.velocity.x);
        _gotoPoint = _rigidbody.position + new Vector2(reverseVelocity_X, 1) * 1000; // * 1000 so it's far away
        
        _pursuing = false;
        _fleeing = true;
        _hiding = false;
    }

    public override void PursuitBehavior()
    {
        if (_dropping)
        {
            StartFlyingIfFallenFarEnough();
        }
        else
        {
            MoveForward();
        }
    }

    private void StartFlyingIfFallenFarEnough()
    {
        RaycastHit2D hit = Physics2D.Raycast( gameObject.transform.position
                                            , Vector2.up
                                            , DistanceDropsFromCeiling
                                            , _physicsLayer);

        if ( !hit )
        {
            _dropping = false;
            _rigidbody.gravityScale = 0;
            _animator.SetBool("Flying", true);
        }
    }

    public override void MoveForward()
    {
        Vector2 diff = (_rigidbody.position - _gotoPoint).normalized * -speed;
        _rigidbody.velocity += diff / Time.deltaTime;

        float ClampedVelocity_X = Mathf.Clamp(_rigidbody.velocity.x, -speed, speed);
        float ClampedVelocity_Y = Mathf.Clamp(_rigidbody.velocity.y, -speed, speed);
        _rigidbody.velocity = new Vector2(ClampedVelocity_X, ClampedVelocity_Y);
    }

    public override void SetTarget(GameObject target)
    {
        if ( target != null
             && DoneHiding
             && _hiding )
        {
            Drop();
            base.SetTarget( target );
        }
    }

    private void Drop()
    {
        _rigidbody.gravityScale = 1;
        _dropping = true;
        _hiding = false;
    }

    private bool DoneHiding
        => _timeStopsHiding < Time.time;
}
