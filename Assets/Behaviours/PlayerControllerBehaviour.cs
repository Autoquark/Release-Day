using Assets;
using Assets.Behaviours;
using Assets.Extensions;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class PlayerControllerBehaviour : MonoBehaviour
{
    public float jumpVelocity = 6f;
    public float walkSpeed = 0.1f;

    const float _minSeparationDistance = 0.1f;
    readonly Lazy<Rigidbody2D> _rigidbody;
    readonly Lazy<Collider2D> _collider;
    readonly Lazy<PhysicsObject> _physicsObject;

    bool _jumpPending = false;
    Vector2 _velocity = Vector2.zero;

    public PlayerControllerBehaviour()
    {
        _rigidbody = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
        _collider = new Lazy<Collider2D>(GetComponent<Collider2D>);
        _physicsObject = new Lazy<PhysicsObject>(GetComponent<PhysicsObject>);
    }

    private void Start()
    {
        _physicsObject.Value.PositionOnGround();
    }

    // Update is called once per frame
    void Update()
    {
        var interactable = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().FirstOrDefault(x => x.CanInteract());
        if (interactable != null && Input.GetKeyDown(KeyCode.E))
        {
            interactable.Interact();
        }

        if (_physicsObject.Value.Grounded && Input.GetKeyDown(KeyCode.Space))
        {
            _jumpPending = true;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _physicsObject.Value.WalkIntent = -walkSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _physicsObject.Value.WalkIntent = walkSpeed;
        }
        else
        {
            _physicsObject.Value.WalkIntent = 0;
        }

        if(_jumpPending)
        {
            _physicsObject.Value.YVelocity = jumpVelocity;
            _jumpPending = false;
        }
    }

    private void OnDisable()
    {
        _physicsObject.Value.WalkIntent = 0;
    }
}
