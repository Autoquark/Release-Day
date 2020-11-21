using Assets.Common;
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
        const float XVelocityDecayFactor = 0.05f;

        public bool Grounded { get; private set; } = false;
        public Vector2 MovementLastFrame { get; private set; }

        /// <summary>
        /// The sign and magnitude of this value indicate the direction and magnitude of the movement along floors (if grounded) or left/right in midair that this PhysicsObject
        /// intends to make per second.
        /// </summary>
        public float WalkIntent { get; set; }
        /// <summary>
        /// The object's vertical velocity, which is modified by gravity each FixedUpdate and becomes zero upon encountering a vertical collision
        /// </summary>
        public float YVelocity { get; set; }
        /// <summary>
        /// The object's horizontal velocity, which decays exponentially each FixedUpdate and becomes zero upon encountering a vertical collision
        /// </summary>
        public float XVelocity { get; set; }

        protected Rigidbody2D Rigidbody => _rigidbody.Value;
        protected float MaxStepAngle { get; set; } = 45;
        protected ContactFilter2D Filter;

        private readonly Lazy<Rigidbody2D> _rigidbody;

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
            var updateWalkIntent = WalkIntent;
            if(Mathf.Sign(XVelocity) != Mathf.Sign(WalkIntent))
            {
                var tempXVelocity = XVelocity + MathEx.ClosestToZero(-XVelocity, WalkIntent);
                updateWalkIntent += MathEx.ClosestToZero(XVelocity, -WalkIntent);
                XVelocity = tempXVelocity;
            }

            var remainingDistance = Mathf.Abs(updateWalkIntent + XVelocity) * Time.fixedDeltaTime;
            while (remainingDistance >= MinimumMoveDistance)
            {
                var right = (updateWalkIntent + XVelocity) > 0;
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

            XVelocity *= Mathf.Pow(XVelocityDecayFactor, Time.fixedDeltaTime);
            if(Mathf.Abs(XVelocity) <= MinimumMoveDistance)
            {
                XVelocity = 0;
            }

            // Handle vertical movement
            YVelocity += Physics2D.gravity.y * Time.fixedDeltaTime;
            var frameVelocity = YVelocity * Time.fixedDeltaTime;

            var results = new List<RaycastHit2D>();
            var distance = CastForMovement(new Vector2(0, frameVelocity), results, Mathf.Abs(frameVelocity));
            Grounded = frameVelocity < 0 && results.Any();

            var movement = new Vector2(0, frameVelocity).normalized * distance;
            Rigidbody.position += movement;
            MovementLastFrame += movement;

            if (results.Any())
            {
                YVelocity = 0;
            }
        }

        private float CastForMovement(Vector2 direction, List<RaycastHit2D> results, float moveDistance)
        {
            Rigidbody.Cast(direction, Filter, results, moveDistance + MinSeparationDistance);

//            results.RemoveAll(x => Vector2.Angle(x.point - (Vector2)transform.position, direction) > 90);
            results.RemoveAll(x => Vector2.Angle(x.normal, direction) < 90);

            return results.MinOrDefault(x => Mathf.Max(0, x.distance - MinSeparationDistance), moveDistance); 
        }

        private float MoveX(float climbAngle, float maxDistance, bool right)
        {
            var direction = right ? Vector2.right.RotateClockwise(-climbAngle) : Vector2.left.RotateClockwise(climbAngle);

            var results = new List<RaycastHit2D>();
            var distance = CastForMovement(direction, results, maxDistance);

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
