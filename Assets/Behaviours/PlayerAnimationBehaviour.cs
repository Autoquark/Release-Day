using Spine.Unity;
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
        public bool flipX = false;

        public float runAnimationSpeedFactor = 30;

        private readonly Lazy<PhysicsObject> _playerController;

        public PlayerAnimationBehaviour()
        {
            _playerController = new Lazy<PhysicsObject>(GetComponent<PhysicsObject>);
        }

        private void Update()
        {
            if(_playerController.Value.MovementLastFrame.y > 0)
            {
                SetAnimationIfDifferent(jumpStart);
            }
            else if(_playerController.Value.MovementLastFrame.y < -PhysicsObject.MinimumMoveDistance && !_playerController.Value.Grounded)
            {
                SetAnimationIfDifferent(fall);
            }

            if (_playerController.Value.MovementLastFrame == Vector2.zero)
            {
                SetAnimationIfDifferent(idle);
            }
            else if (_playerController.Value.Grounded)
            {
                SetAnimationIfDifferent(run);
                _skeletonAnimation.Value.AnimationState.TimeScale = runAnimationSpeedFactor * Mathf.Abs(_playerController.Value.MovementLastFrame.x);
            }

            if(_playerController.Value.WalkIntent != 0)
            {
                var shouldFlip = (_playerController.Value.WalkIntent > 0) ^ flipX;
                _skeletonAnimation.Value.Skeleton.ScaleX = shouldFlip ? -1 : 1;
            }
        }
    }
}
