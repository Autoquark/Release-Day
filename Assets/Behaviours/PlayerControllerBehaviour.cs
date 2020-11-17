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
    public float runSpeed = 0.1f;
    public float walkSpeed = 0.1f;

    const float _minSeparationDistance = 0.1f;
    private readonly Lazy<Rigidbody2D> _rigidbody;
    private readonly Lazy<Collider2D> _collider;
    private readonly Lazy<PhysicsObject> _physicsObject;
    private readonly Lazy<GameObject> _prompt;

    bool _jumpPending = false;
    Vector2 _velocity = Vector2.zero;

    public PlayerControllerBehaviour()
    {
        _rigidbody = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
        _collider = new Lazy<Collider2D>(GetComponent<Collider2D>);
        _physicsObject = new Lazy<PhysicsObject>(GetComponent<PhysicsObject>);
        _prompt = new Lazy<GameObject>(() => transform.Find("Prompt").gameObject);
    }

    private void Start()
    {
        _physicsObject.Value.PositionOnGround();
    }

    // Update is called once per frame
    void Update()
    {
        var interactable = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().FirstOrDefault(x => x.CanInteract());
        _prompt.Value.SetActive(interactable != null);
        if (interactable != null && Input.GetKeyDown(KeyCode.E))
        {
            _prompt.Value.SetActive(false);
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
            _physicsObject.Value.WalkIntent = -runSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _physicsObject.Value.WalkIntent = runSpeed;
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
