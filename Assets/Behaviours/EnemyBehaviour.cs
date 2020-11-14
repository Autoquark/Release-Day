using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    const float _moveSpeed = 1.5f;
    const float _forceScale = 10;

    readonly Lazy<Rigidbody2D> _rigidbody;
    readonly Lazy<Collider2D> _collider;
    bool _direction = false;

    public EnemyBehaviour()
    {
        _rigidbody = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
        _collider = new Lazy<Collider2D>(GetComponent<Collider2D>);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

        if (rightFloor)
        {
            Debug.DrawLine(transform.position, transform.position + rightFloorProbe, Color.green, 0, false);
        }
        if (leftFloor)
        {
            Debug.DrawLine(transform.position, transform.position + leftFloorProbe, Color.green, 0, false);
        }
        if (rightWall)
        {
            Debug.DrawLine(transform.position, transform.position + new Vector3(xFloorProbeOffset, 0, 0), Color.green, 0, false);
        }
        if (leftWall)
        {
            Debug.DrawLine(transform.position, transform.position + new Vector3(-xFloorProbeOffset, 0, 0), Color.green, 0, false);
        }

        if (!leftFloor && !rightFloor)
            return;

        if (_direction)
        {
            // right

            if (!rightFloor || rightWall)
            {
                _direction = false;

                return;
            }
        }
        else
        {
            if (!leftFloor || leftWall)
            {
                _direction = true;

                return;
            }
        }

        Vector2 force = Vector2.right * _forceScale * (_direction ? 1 : -1);
        float hv = _rigidbody.Value.velocity.x;

        Debug.DrawLine(transform.position + new Vector3(0, 0.1f, 0), transform.position + new Vector3(hv, 0.1f, 0), Color.blue, 0, false);

        if (Mathf.Abs(hv) < _moveSpeed)
        {
            _rigidbody.Value.AddForce(force, ForceMode2D.Force);
        }
    }

    private void FixedUpdate()
    {
    }
}
