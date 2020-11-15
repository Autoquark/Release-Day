using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Animation = Spine.Animation;

namespace Assets.Behaviours
{
    class PlayerAnimationBehaviour : MonoBehaviour
    {
        public AnimationReferenceAsset run, idle, jumpStart, jumpApex, fall;

        private Lazy<PlayerControllerBehaviour> _playerController;
        private Lazy<SkeletonAnimation> _skeletonAnimation;

        public PlayerAnimationBehaviour()
        {
            _playerController = new Lazy<PlayerControllerBehaviour>(GetComponent<PlayerControllerBehaviour>);
            _skeletonAnimation = new Lazy<SkeletonAnimation>(GetComponent<SkeletonAnimation>);
        }

        private void Update()
        {
            if(_playerController.Value.MovementLastFrame.y > 0)
            {
                SetAnimationIfDifferent(jumpStart);
            }
            else if(_playerController.Value.MovementLastFrame.y < 0 && !_playerController.Value.Grounded)
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
            }

            if(_playerController.Value.MovementLastFrame.x != 0)
            {
                var facingRight = _playerController.Value.MovementLastFrame.x > 0;
                _skeletonAnimation.Value.Skeleton.ScaleX = facingRight ? -1 : 1;
            }
        }

        private void SetAnimationIfDifferent(Animation animation)
        {
            if (_skeletonAnimation.Value.state.GetCurrent(0)?.Animation == animation)
            {
                return;
            }

            _skeletonAnimation.Value.state.SetAnimation(0, animation, true);
        }
    }
}
