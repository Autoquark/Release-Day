using Assets;
using Assets.Behaviours;
using Assets.Extensions;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class PlayerControllerBehaviour : PhysicsObject
{
    const float _moveSpeed = 0.1f;
    const float _minSeparationDistance = 0.1f;
    const float _jumpVelocity = 5f;
    readonly Lazy<Rigidbody2D> _rigidbody;
    readonly Lazy<Collider2D> _collider;

    bool _jumpPending = false;
    Vector2 _velocity = Vector2.zero;

    public PlayerControllerBehaviour()
    {
        _rigidbody = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
        _collider = new Lazy<Collider2D>(GetComponent<Collider2D>);
    }

    // Update is called once per frame
    void Update()
    {
        var interactable = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().FirstOrDefault(x => x.CanInteract());
        if (interactable != null && Input.GetKeyDown(KeyCode.E))
        {
            interactable.Interact();
        }

        if (Grounded && Input.GetKeyDown(KeyCode.Space))
        {
            _jumpPending = true;
        }
    }

    protected override void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            WalkIntent = -_moveSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            WalkIntent = _moveSpeed;
        }
        else
        {
            WalkIntent = 0;
        }

        if(_jumpPending)
        {
            YVelocity = _jumpVelocity;
            _jumpPending = false;
        }

        base.FixedUpdate();
    }
}
