using Assets.Extensions;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class PhysicsObject : MonoBehaviour
    {
        const float _minimumMoveDistance = 0.001f;
        const float _minSeparationDistance = 0.02f;
        const float _minimumGroundNormalY = 0.65f;

        public bool Grounded { get; private set; } = false;

        protected Rigidbody2D Rigidbody => _rigidbody.Value;
        protected float YVelocity { get; set; }
        protected float MaxStepAngle { get; set; } = 45;
        protected ContactFilter2D Filter;

        protected float WalkIntent { get; set; }

        private Lazy<Rigidbody2D> _rigidbody;

        public PhysicsObject()
        {
            _rigidbody = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
        }

        private void Start()
        {
            Filter.useTriggers = false;
            Filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        }

        protected virtual void FixedUpdate()
        {
            var results = new List<RaycastHit2D>();

            // Handle walking, including up or down a slope
            var remainingDistance = Mathf.Abs(WalkIntent);
            while (remainingDistance >= _minimumMoveDistance)
            {
                var right = WalkIntent > 0;
                var distanceMoved = 0f;

                // Try stepping down if grounded
                if (Grounded)
                {
                    distanceMoved = MoveX(-MaxStepAngle, remainingDistance, right);
                    if(distanceMoved > 0)
                    {
                        remainingDistance -= distanceMoved;
                        continue;
                    }
                }

                distanceMoved = MoveX(0, remainingDistance, right);
                if (distanceMoved > 0)
                {
                    remainingDistance -= distanceMoved;
                    continue;
                }

                remainingDistance -= MoveX(MaxStepAngle, remainingDistance, right);
                if (distanceMoved > 0)
                {
                    remainingDistance -= distanceMoved;
                    continue;
                }

                break;
            }

            // Handle vertical movement
            YVelocity += Physics2D.gravity.y * Time.fixedDeltaTime;
            var frameVelocity = YVelocity * Time.fixedDeltaTime;

            results = new List<RaycastHit2D>();
            Rigidbody.Cast(new Vector2(0, frameVelocity), Filter, results, Mathf.Abs(frameVelocity));
            Grounded = frameVelocity < 0 && results.Select(x => x.normal)
                .Any(normal => normal.y >= _minimumGroundNormalY);

            var distance = results.MinOrDefault(x => Mathf.Max(0, x.distance - _minSeparationDistance), Mathf.Abs(frameVelocity));

            Rigidbody.position += new Vector2(0, frameVelocity).normalized * distance;

            if (results.Any())
            {
                YVelocity = 0;
            }
        }

        private float MoveX(float climbAngle, float maxDistance, bool right)
        {
            var direction = right ? Vector2.right.RotateClockwise(-climbAngle) : Vector2.left.RotateClockwise(climbAngle);

            var results = new List<RaycastHit2D>();
            Rigidbody.Cast(direction, Filter, results, maxDistance);
            var distance = results.MinOrDefault(x => Mathf.Max(0, x.distance - _minSeparationDistance), maxDistance);

            if (distance > _minimumMoveDistance)
            {
                Rigidbody.position += direction * distance;
                return distance;
            }

            return 0;
        }
    }
}
