using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Data;
using UnityEngine.Tilemaps;
using Spine.Unity;
using Animation = Spine.Animation;

namespace Assets.Behaviours
{

    class AnimationBase : MonoBehaviour
    {
        protected readonly Lazy<SkeletonAnimation> _skeletonAnimation;

        protected AnimationBase()
        {
            _skeletonAnimation = new Lazy<SkeletonAnimation>(GetComponent<SkeletonAnimation>);
        }

        protected void SetAnimationIfDifferent(Animation animation, bool loop = true)
        {
            if (_skeletonAnimation.Value == null || _skeletonAnimation.Value.state == null)
            {
                return;
            }

            if (_skeletonAnimation.Value.state.GetCurrent(0)?.Animation == animation)
            {
                return;
            }

            _skeletonAnimation.Value.state.SetAnimation(0, animation, loop);
            _skeletonAnimation.Value.AnimationState.TimeScale = 1;
        }
    }
}
