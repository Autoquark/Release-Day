using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Behaviours
{
    class DoorBehaviour : MonoBehaviour, IInteractable
    {
        public DoorBehaviour _linkedDoor;
        public string _goToScene;

        private Lazy<Collider2D> _playerCollider;
        private Lazy<Collider2D> _collider;

        public DoorBehaviour()
        {
            _playerCollider = new Lazy<Collider2D>(() => FindObjectOfType<PlayerControllerBehaviour>().GetComponent<Collider2D>());
            _collider = new Lazy<Collider2D>(GetComponent<Collider2D>);
        }

        public string Message => "Press E to enter";

        public bool CanInteract() => _linkedDoor != null || !string.IsNullOrWhiteSpace(_goToScene)
            && Mathf.Abs(_playerCollider.Value.transform.position.x - transform.position.x) <= 0.5 * _playerCollider.Value.bounds.size.x
            && Mathf.Abs(_playerCollider.Value.transform.position.y - transform.position.y) <= 0.5 * _playerCollider.Value.bounds.size.y;

        public void Interact()
        {
            if (_linkedDoor != null)
            {
                _playerCollider.Value.GetComponent<Rigidbody2D>().position = _linkedDoor.transform.position;
            }
            else
            {
                SceneManager.LoadScene(_goToScene);
            }
        }
    }
}
