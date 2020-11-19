using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Data;
using UnityEngine.Tilemaps;
using Spine.Unity;

namespace Assets.Behaviours
{

    class EnemyAnimation : AnimationBase
    {
        public AnimationReferenceAsset idle;

        EnemyAnimation()
        {
        }

        private void Update()
        {
            SetAnimationIfDifferent(idle);
        }
    }
}
