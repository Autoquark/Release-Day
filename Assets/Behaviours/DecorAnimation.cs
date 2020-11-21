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
    class DecorAnimation : AnimationBase
    {
        public AnimationReferenceAsset idle;
        float _speed = 0f;
        float _targetSpeed = 0f;
        public float MinSpeed = 0.5f;
        public float MaxSpeed = 1;
        public float MaxRunTime = 1f;
        public float MinRunTime = 0.5f;
        public float MaxStopTime = 10f;
        public float MinStopTime = 2f;
        public float StopProbPerSec = 0.5f;
        public float MaxSpeedChange = 0.01f;

        float _nextChange;
        bool _first = true;


        private void FixedUpdate()
        {
            if (_first)
            {
                _nextChange = Time.time + UnityEngine.Random.Range(MinStopTime, MaxStopTime);

                _first = false;
            }

            SetAnimationIfDifferent(idle);

            _skeletonAnimation.Value.timeScale = _speed;

            float diff = _targetSpeed - _speed;
            float adiff = Mathf.Abs(diff);
            if (adiff > 0.001f)
            {
                diff = Mathf.Sign(diff) * Mathf.Min(MaxSpeedChange, adiff);
                _speed += diff;
            }

            if (Time.time > _nextChange)
            {
                if (_targetSpeed == 0)
                {
                    SetSpeed();
                    _nextChange = Time.time + UnityEngine.Random.Range(MinRunTime, MaxRunTime);

                }
                else
                {
                    _targetSpeed = 0;
                    _nextChange = Time.time + UnityEngine.Random.Range(MinStopTime, MaxStopTime);
                }
            }
        }

        private void SetSpeed()
        {
            _targetSpeed = UnityEngine.Random.Range(0, MaxSpeed);
        }
    }
}
