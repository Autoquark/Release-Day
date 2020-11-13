using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class DoorBehaviour : MonoBehaviour, IInteractable
    {
        public DoorBehaviour linkedDoor;

        private Lazy<Collider2D> _playerCollider;
        private Lazy<Collider2D> _collider;

        public DoorBehaviour()
        {
            _playerCollider = new Lazy<Collider2D>(() => FindObjectOfType<PlayerControllerBehaviour>().GetComponent<Collider2D>());
            _collider = new Lazy<Collider2D>(GetComponent<Collider2D>);
        }

        public string Message => "Press E to enter";

        public bool CanInteract() => Mathf.Abs(_playerCollider.Value.transform.position.x - transform.position.x) <= 0.5 * _playerCollider.Value.bounds.size.x
            && Mathf.Abs(_playerCollider.Value.transform.position.y - transform.position.y) <= 0.5 * _playerCollider.Value.bounds.size.y;

        public void Interact()
        {
            _playerCollider.Value.GetComponent<Rigidbody2D>().position = linkedDoor.transform.position;
        }
    }
}
