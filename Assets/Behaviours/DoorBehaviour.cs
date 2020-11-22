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
        public bool _goToNextScene = false;

        public string Message => "Press E to enter";

        private readonly Lazy<AudioSource> _audioSource;
        private readonly Lazy<LevelControllerBehaviour> _levelController;

        public DoorBehaviour()
        {
            _audioSource = new Lazy<AudioSource>(GetComponent<AudioSource>);
            _levelController = new Lazy<LevelControllerBehaviour>(FindObjectOfType<LevelControllerBehaviour>);
        }

        public bool CanInteractWith(PlayerControllerBehaviour player)
        {
            return (_linkedDoor != null || _goToNextScene)
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
                _levelController.Value.GoToNextLevel();
            }

            _audioSource.Value.Play();
        }
    }
}
