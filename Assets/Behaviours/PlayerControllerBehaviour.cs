using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerControllerBehaviour : MonoBehaviour
{
    const float _moveSpeed = 40;
    readonly Lazy<Rigidbody2D> _rigidbody;

    public PlayerControllerBehaviour()
    {
        _rigidbody = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var interactable = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().FirstOrDefault(x => x.CanInteract());
        if(interactable != null && Input.GetKeyDown(KeyCode.E))
        {
            interactable.Interact();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // https://answers.unity.com/questions/696068/difference-between-forcemodeforceaccelerationimpul.html
            // Basically, I think Impulse simulates an instantaneous force whereas Force simulates a constant force over the duration of the fixed update interval
            _rigidbody.Value.AddForce(Vector2.left * _moveSpeed, ForceMode2D.Force);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _rigidbody.Value.AddForce(Vector2.right * _moveSpeed, ForceMode2D.Force);
        }
    }
}
