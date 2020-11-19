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
    class TurretAnimation : AnimationBase
    {
        public AnimationReferenceAsset idle;
        public float LaunchMoment = 2.0f;

        private bool _wasBelow = true;
        private Lazy<Transform> _launchPoint;
        public GameObject Projectile;


        TurretAnimation()
        {
            _launchPoint = new Lazy<Transform>(() => transform.Find("LaunchPoint"));
        }

        private void Update()
        {
            SetAnimationIfDifferent(idle);

            bool isBelow = _skeletonAnimation.Value.AnimationState.Tracks.Items[0].AnimationTime < LaunchMoment;

            if (_wasBelow && !isBelow)
            {
                var proj = Instantiate(Projectile, _launchPoint.Value.transform.position, _launchPoint.Value.transform.rotation);
                proj.transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }

            _wasBelow = isBelow;
        } 
    }
}
