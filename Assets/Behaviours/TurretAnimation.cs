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
        public float LaunchInterval = 0.5f;

        private int _launchCounter = 0;
        private Lazy<Transform> _launchPoint;
        public GameObject Projectile;


        TurretAnimation()
        {
            _launchPoint = new Lazy<Transform>(() => transform.Find("LaunchPoint"));
        }

        private void Update()
        {
            SetAnimationIfDifferent(idle);

            int count = (int)(_skeletonAnimation.Value.AnimationState.Tracks.Items[0].AnimationTime / LaunchInterval);

            if (count != _launchCounter)
            {
                var proj = Instantiate(Projectile, _launchPoint.Value.transform.position, _launchPoint.Value.transform.rotation);
                proj.transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }

            _launchCounter = count;
        } 
    }
}
