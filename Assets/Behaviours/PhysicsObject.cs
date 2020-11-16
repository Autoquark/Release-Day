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
        public const float MinimumMoveDistance = 0.001f;
        public const float MinSeparationDistance = 0.03f;

        public bool Grounded { get; private set; } = false;
        public Vector2 MovementLastFrame { get; private set; }
        public float WalkIntent { get; set; }
        public float YVelocity { get; set; }

        protected Rigidbody2D Rigidbody => _rigidbody.Value;
        protected float MaxStepAngle { get; set; } = 45;
        protected ContactFilter2D Filter;

        private Lazy<Rigidbody2D> _rigidbody;

        public PhysicsObject()
        {
            _rigidbody = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
        }

        public void PositionOnGround()
        {
            var results = new List<RaycastHit2D>();
            Rigidbody.Cast(Vector2.down, Filter, results, 10);
            Rigidbody.position += Vector2.down * results.MinOrDefault(x => Mathf.Max(0, x.distance - MinSeparationDistance));

            Rigidbody.Cast(Vector2.down, Filter, results, 10);
        }

        private void Start()
        {
            Filter.useTriggers = false;
            Filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        }

        protected virtual void FixedUpdate()
        {
            MovementLastFrame = Vector2.zero;

            var colliders = new List<Collider2D>();
            _rigidbody.Value.OverlapCollider(Filter, colliders);
            if (colliders.Any(x => Rigidbody.IsTouching(x)))
            {
                Debug.LogWarning(this + " is overlapping one or more colliders that are solid to it");
            }

            // Handle walking, including up or down a slope
            var remainingDistance = Mathf.Abs(WalkIntent);
            while (remainingDistance >= MinimumMoveDistance)
            {
                var right = WalkIntent > 0;
                var distanceMoved = 0f;

                // Try stepping down if grounded
                if (Grounded && MaxStepAngle > 0.0001f)
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

                distanceMoved = MoveX(MaxStepAngle, remainingDistance, right);
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

            var results = new List<RaycastHit2D>();
            Rigidbody.Cast(new Vector2(0, frameVelocity), Filter, results, Mathf.Abs(frameVelocity) + MinSeparationDistance);
            Grounded = frameVelocity < 0 && results.Any();

            var distance = results.MinOrDefault(x => Mathf.Max(0, x.distance - MinSeparationDistance), Mathf.Abs(frameVelocity));

            var movement = new Vector2(0, frameVelocity).normalized * distance;
            Rigidbody.position += movement;
            MovementLastFrame += movement;

            if (results.Any())
            {
                YVelocity = 0;
            }
        }

        private float MoveX(float climbAngle, float maxDistance, bool right)
        {
            var direction = right ? Vector2.right.RotateClockwise(-climbAngle) : Vector2.left.RotateClockwise(climbAngle);

            var results = new List<RaycastHit2D>();
            Rigidbody.Cast(direction, Filter, results, maxDistance + MinSeparationDistance);
            var distance = results.MinOrDefault(x => Mathf.Max(0, x.distance - MinSeparationDistance), maxDistance);

            if (distance > MinimumMoveDistance)
            {
                Rigidbody.position += direction * distance;
                MovementLastFrame += direction * distance;
                return distance;
            }

            return 0;
        }
    }
}
