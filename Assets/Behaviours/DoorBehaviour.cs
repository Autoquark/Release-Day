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

        public string Message => "Press E to enter";

        public bool CanInteractWith(PlayerControllerBehaviour player)
        {
            return (_linkedDoor != null || !string.IsNullOrWhiteSpace(_goToScene))
                && Mathf.Abs(player.transform.position.x - transform.position.x) <= 0.5 * player.GetComponent<Collider2D>().bounds.size.x
                && Mathf.Abs(player.transform.position.y - transform.position.y) <= 0.5 * player.GetComponent<Collider2D>().bounds.size.y;
        }

        public void InteractWith(PlayerControllerBehaviour player)
        {
            if (_linkedDoor != null)
            {
                player.GetComponent<Rigidbody2D>().position = _linkedDoor.transform.position;
            }
            else
            {
                SceneManager.LoadScene(_goToScene);
            }
        }
    }
}
