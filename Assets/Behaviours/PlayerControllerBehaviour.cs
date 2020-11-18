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
    public float jumpGracePeriod = 0.1f;

    const float _minSeparationDistance = 0.1f;
    private readonly Lazy<Rigidbody2D> _rigidbody;
    private readonly Lazy<Collider2D> _collider;
    private readonly Lazy<PhysicsObject> _physicsObject;
    private readonly Lazy<GameObject> _prompt;

    private bool _jumpPending = false;
    private float _lastGroundedTime = -999;
    private bool _jumpedSinceLastGrounded = false;

    static List<PlayerControllerBehaviour> _allPlayers = new List<PlayerControllerBehaviour>();

    public static PlayerControllerBehaviour FirstPlayer()
    {
        return _allPlayers.FirstOrDefault<PlayerControllerBehaviour>();
    }

    public static IEnumerable<PlayerControllerBehaviour> AllPlayers()
    {
        return _allPlayers;
    }

    public PlayerControllerBehaviour()
    {
        _rigidbody = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
        _collider = new Lazy<Collider2D>(GetComponent<Collider2D>);
        _physicsObject = new Lazy<PhysicsObject>(GetComponent<PhysicsObject>);
        _prompt = new Lazy<GameObject>(() => transform.Find("Prompt").gameObject);
    }

    private void Start()
    {
        _allPlayers.Add(this);
        _physicsObject.Value.PositionOnGround();
    }

    private void OnDestroy()
    {
        _allPlayers.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        var interactable = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().FirstOrDefault(x => x.CanInteractWith(this));
        _prompt.Value.SetActive(interactable != null);
        if (interactable != null && Input.GetKeyDown(KeyCode.E))
        {
            _prompt.Value.SetActive(false);
            interactable.InteractWith(this);
        }

        if (Input.GetKeyDown(KeyCode.Space)
            && (_physicsObject.Value.Grounded || Time.fixedTime - _lastGroundedTime < jumpGracePeriod)
            && !_jumpedSinceLastGrounded)
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
            _jumpedSinceLastGrounded = true;
        }
        else if(_physicsObject.Value.Grounded)
        {
            _lastGroundedTime = Time.fixedTime;
            _jumpedSinceLastGrounded = false;
        }
    }

    private void OnDisable()
    {
        _physicsObject.Value.WalkIntent = 0;
    }
}
