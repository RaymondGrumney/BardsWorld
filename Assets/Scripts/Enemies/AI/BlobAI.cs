using CommonAssets.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Enemies
{
    public class BlobAI : BaseEnemyAI
    {
        [Header("Blob AI Setting")]
        public List<RandomChoice> RandomChoices = new List<RandomChoice>()
        {
            new RandomChoice()
            {
                Choice="Turn",
                Chance=1
            },
            new RandomChoice()
            {
                Choice="Jump",
                Chance=7
            },
            new RandomChoice()
            {
                Choice="MoveForward",
                Chance=10
            }
        };

        private bool _grounded;
        public float jumpChanceDuringPursuit = 0.01f;


        public override void DefaultBehavior()
        {
            if(nextRandomChoice < Time.time)
            {
                nextRandomChoice += randomChoiceEveryNSeconds;
                MakeRandomChoice(RandomChoices);
            }
        }

        public override bool ForgetBehavior()
        {
            if( base.ForgetBehavior() )
            {
                _targetPoint = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Turn()
        {

            SetFacing(-facing.x);
        }

        public override void IdleBehavior() {  }

        /// <summary>
        /// Chooses the wander location.
        /// </summary>
        protected void ChooseWanderLocation()
        {
            //float x = Random.Range(minimumWanderDistance, maximumWanderDistance) * facing.x + transform.position.x;
            //_gotoPoint = new Vector2(x, transform.position.y);
        }

        public override void PursuitBehavior()
        {
            // if we're not near the target point
            if (!_collider.OverlapPoint((Vector2)_targetPoint)
                && _targetPoint != null)
            {
                FaceTarget();

                float rng = Random.value;

                if (rng > jumpChanceDuringPursuit)
                    MoveForward();
                else
                    Jump();
            }
        }

        public void GroundMe(bool state)
        {
            _grounded = state;
        }

        public override void MoveForward()
        {
            if ( _grounded )
            {
                _rigidbody.velocity = new Vector2(facing.x * speed, _rigidbody.velocity.y);
            }
        }

        public void Jump()
        {
            if (_grounded)
            {
                _rigidbody.velocity = new Vector2(facing.x * speed, speed);
            }
        }
    }
}
