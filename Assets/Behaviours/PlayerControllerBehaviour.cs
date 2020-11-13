using Assets;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerControllerBehaviour : MonoBehaviour
{
    const float _moveSpeed = 0.1f;
    const float _minSeparationDistance = 0.05f;
    readonly Lazy<Rigidbody2D> _rigidbody;
    readonly Lazy<Collider2D> _collider;

    bool _jumpPending = false;

    public PlayerControllerBehaviour()
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
        var interactable = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().FirstOrDefault(x => x.CanInteract());
        if (interactable != null && Input.GetKeyDown(KeyCode.E))
        {
            interactable.Interact();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    private void FixedUpdate()
    {
        var baseMovement = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // https://answers.unity.com/questions/696068/difference-between-forcemodeforceaccelerationimpul.html
            // Basically, I think Impulse simulates an instantaneous force whereas Force simulates a constant force over the duration of the fixed update interval
            // _rigidbody.Value.AddForce(Vector2.left * _moveSpeed, ForceMode2D.Force);
            baseMovement = Vector2.left * _moveSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //_rigidbody.Value.AddForce(Vector2.right * _moveSpeed, ForceMode2D.Force);
            baseMovement = Vector2.right * _moveSpeed;
        }

        var results = new List<RaycastHit2D>();
        var filter = new ContactFilter2D().NoFilter();
        filter.useTriggers = false;

        // Check if falling
        var grounded = _collider.Value.Cast(Vector2.down, filter, results, _minSeparationDistance + 0.01f) != 0;
        if(!grounded)
        {
            baseMovement += Physics2D.gravity * Time.fixedDeltaTime;
        }

        if(baseMovement == Vector2.zero)
        {
            return;
        }

        _collider.Value.Cast(baseMovement, filter, results, baseMovement.magnitude);
        var bestDistance = results.MinOrDefault(x => Mathf.Max(x.distance - _minSeparationDistance, 0), baseMovement.magnitude);
        var movement = baseMovement;

        // Try to move down a slope
        if (grounded)
        {
            var downMovement = Vector2.Lerp(Vector2.down * baseMovement.magnitude, baseMovement, 0.5f);
            _collider.Value.Cast(downMovement, filter, results, baseMovement.magnitude);
            var downDistance = results.MinOrDefault(x => Mathf.Max(x.distance - _minSeparationDistance, 0), baseMovement.magnitude);
            if (downDistance >= bestDistance)
            {
                bestDistance = downDistance;
                movement = downMovement;
            }

            var upMovement = Vector2.Lerp(Vector2.up * baseMovement.magnitude, baseMovement, 0.5f);
            _collider.Value.Cast(upMovement, new ContactFilter2D().NoFilter(), results, baseMovement.magnitude);

            var upDistance = results.MinOrDefault(x => Mathf.Max(x.distance - _minSeparationDistance, 0), upMovement.magnitude);
            if (upDistance > bestDistance)
            {
                bestDistance = upDistance;
                movement = upMovement;
            }
        }

        movement = movement.normalized * bestDistance;

        _rigidbody.Value.MovePosition(_rigidbody.Value.position + movement);
    }
}
