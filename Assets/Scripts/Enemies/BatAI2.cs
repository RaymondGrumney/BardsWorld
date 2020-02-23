using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI2 : BaseEnemyAI
{
    public float DistanceDropsFromCeiling = 2f;
    private bool _dropping;

    public float TimeHidesBeforePursuing = 6f;
    private float _timeHiding = 0f;
    private bool _hiding = true;

    private bool _fleeing = false;

    public override void DefaultBehavior() // flee
    {
        if( !_hiding )
        {
            MoveForward();
        }
        else
        {
            IdleBehavior();
        }
    }

    protected override void Awake()
    {
        _timeHiding = TimeHidesBeforePursuing * -2;
        base.Awake();
    }

    public override bool ForgetBehavior()
    {
        return base.ForgetBehavior();
    }

    public override void IdleBehavior() // hide
    {
        // MakeRandomChoice();
    }

    public void DealtDamage(bool state)
    {
        if(state)
        {
            //RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, diff, 100f, _physicsLayer);
        }
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        if ( _fleeing )
        {
            if ( HitCeiling(collision)
                 && collision.gameObject.CompareTag( "Untagged" ))
            {
                _fleeing = false;
                _pursuing = false;
                _hiding = true;
                _timeHiding = Time.time;

                _gotoPoint = _rigidbody.position;
                _rigidbody.velocity = Vector2.zero;
            }
        }
        else if (_hiding) { }
        else if (_pursuing)
        {
            Vector2 diff = (_rigidbody.position - _gotoPoint).normalized;

            if (collision.gameObject.CompareTag("Player"))
            {
                _pursuing = false;
                _fleeing = true;
                _hiding = false;

                _gotoPoint = diff * 100;
            }
            else
            {
                Easily.Knock(this.gameObject).Back(diff * speed);
            }
        }
    }

    public override void PursuitBehavior()
    {
        if(_dropping)
        {
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, Vector2.up, DistanceDropsFromCeiling, _physicsLayer);

            if(!hit)
            {
                _dropping = false;
                _rigidbody.gravityScale = 0;
            }
        }
        else
        {
            FaceTarget();
            MoveForward();
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
        if( target != null 
            && _timeHiding + TimeHidesBeforePursuing < Time.time 
            && !_fleeing )
        {
            _rigidbody.gravityScale = 1;
            _dropping = true;
            _hiding = false;
            base.SetTarget(target);
        }
    }
}
