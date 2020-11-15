using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Behaviours
{
    class EnemyBehaviour : PhysicsObject
    {
        const float _moveSpeed = 0.07f;
        const float _forceScale = 10;

        readonly Lazy<Rigidbody2D> _rigidbody;
        readonly Lazy<Collider2D> _collider;
        enum MoveIntent
        {
            None,
            Left,
            Right
        }

        MoveIntent _moveIntent = MoveIntent.None;

        bool _direction = false;

        public EnemyBehaviour()
        {
            _rigidbody = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
            _collider = new Lazy<Collider2D>(GetComponent<Collider2D>);
        }

        void Update()
        {
            float xFloorProbeOffset = _collider.Value.bounds.size.x * 0.6f;
            float yFloorProbeOffset = _collider.Value.bounds.size.y * 0.6f;

            float xWallProbeOffset = _collider.Value.bounds.size.x / 10;

            Vector3 rightFloorProbeStart = new Vector2(xFloorProbeOffset, 0);
            Vector3 rightFloorProbeEnd = new Vector2(xFloorProbeOffset, -yFloorProbeOffset);
            Vector3 leftFloorProbeStart = new Vector2(-xFloorProbeOffset, 0);
            Vector3 leftFloorProbeEnd = new Vector2(-xFloorProbeOffset, -yFloorProbeOffset);

            bool rightFloor = Physics2D.Raycast((Vector2)(transform.position + rightFloorProbeStart), Vector2.down, yFloorProbeOffset).collider != null;
            bool leftFloor = Physics2D.Raycast((Vector2)(transform.position + leftFloorProbeStart), Vector2.down, yFloorProbeOffset).collider != null;

            RaycastHit2D[] unused = new RaycastHit2D[1];

            bool rightWall = _collider.Value.Cast(Vector2.right, unused, xWallProbeOffset) != 0;
            bool leftWall = _collider.Value.Cast(Vector2.left, unused, xWallProbeOffset) != 0;

            //if (rightFloor)
            //{
            //    Debug.DrawLine(transform.position + rightFloorProbeStart, transform.position + rightFloorProbeEnd, Color.green, 0, false);
            //}
            //if (leftFloor)
            //{
            //    Debug.DrawLine(transform.position + leftFloorProbeStart, transform.position + leftFloorProbeEnd, Color.green, 0, false);
            //}
            //if (rightWall)
            //{
            //    Debug.DrawLine(transform.position, transform.position + new Vector3(xFloorProbeOffset, 0, 0), Color.green, 0, false);
            //}
            //if (leftWall)
            //{
            //    Debug.DrawLine(transform.position, transform.position + new Vector3(-xFloorProbeOffset, 0, 0), Color.green, 0, false);
            //}

            if (!leftFloor && !rightFloor)
            {
                _moveIntent = MoveIntent.None;

                return;
            }

            if (_direction)
            {
                // right

                if (!rightFloor || rightWall)
                {
                    _direction = false;
                }
            }
            else
            {
                if (!leftFloor || leftWall)
                {
                    _direction = true;
                }
            }

            _moveIntent = _direction ? MoveIntent.Right : MoveIntent.Left;
        }

        protected override void FixedUpdate()
        {
            switch(_moveIntent)
            {
                case MoveIntent.None:
                    WalkIntent = 0.0f;
                    break;

                case MoveIntent.Right:
                    WalkIntent = _moveSpeed;
                    break;

                case MoveIntent.Left:
                    WalkIntent = -_moveSpeed;
                    break;
            }

            base.FixedUpdate();
        }
    }
}