﻿using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class PlayerAnimationBehaviour : AnimationBase
    {
        public AnimationReferenceAsset run, idle, jumpStart, jumpUp, jumpApex, fall;

        public float runAnimationSpeedFactor = 30;

        private readonly Lazy<PhysicsObject> _physicsObject;

        public PlayerAnimationBehaviour()
        {
            _physicsObject = new Lazy<PhysicsObject>(GetComponent<PhysicsObject>);
        }

        private void Update()
        {
            if(_physicsObject.Value.MovementLastFrame.y > 0)
            {
                SetAnimationIfDifferent(jumpStart);
            }
            else if(_physicsObject.Value.MovementLastFrame.y < -PhysicsObject.MinimumMoveDistance && !_physicsObject.Value.Grounded)
            {
                SetAnimationIfDifferent(fall);
            }

            if (_physicsObject.Value.MovementLastFrame == Vector2.zero)
            {
                SetAnimationIfDifferent(idle);
            }
            else if (_physicsObject.Value.Grounded)
            {
                SetAnimationIfDifferent(run);
                _skeletonAnimation.Value.AnimationState.TimeScale = runAnimationSpeedFactor * Mathf.Abs(_physicsObject.Value.MovementLastFrame.x);
            }

            if(_physicsObject.Value.WalkIntent != 0)
            {
                var facingRight = _physicsObject.Value.WalkIntent > 0;
                _skeletonAnimation.Value.Skeleton.ScaleX = facingRight ? -1 : 1;
            }
        }
    }
}
