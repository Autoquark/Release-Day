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
            float xFloorProbeOffset = _collider.Value.bounds.size.x * 0.5f;
            float xWallProbeOffset = _collider.Value.bounds.size.x / 10;
            float yProbeOffset = _collider.Value.bounds.size.y * 0.7f;

            Vector3 rightFloorProbe = new Vector2(xFloorProbeOffset, -yProbeOffset);
            Vector3 leftFloorProbe = new Vector2(-xFloorProbeOffset, -yProbeOffset);

            bool rightFloor = Physics2D.OverlapPoint((Vector2)(transform.position + rightFloorProbe)) != null;
            bool leftFloor = Physics2D.OverlapPoint((Vector2)(transform.position + leftFloorProbe)) != null;

            RaycastHit2D[] unused = new RaycastHit2D[1];

            bool rightWall = _collider.Value.Cast(Vector2.right, unused, xWallProbeOffset) != 0;
            bool leftWall = _collider.Value.Cast(Vector2.left, unused, xWallProbeOffset) != 0;

            //if (rightFloor)
            //{
            //    Debug.DrawLine(transform.position, transform.position + rightFloorProbe, Color.green, 0, false);
            //}
            //if (leftFloor)
            //{
            //    Debug.DrawLine(transform.position, transform.position + leftFloorProbe, Color.green, 0, false);
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

        private void FixedUpdate()
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