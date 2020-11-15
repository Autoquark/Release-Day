using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class ConversationRegion : MonoBehaviour
    {
        private Lazy<Collider2D> _playerCollider;
        private Lazy<ConversationController> _conversationController;

        private bool _alreadyTriggered = false;

        public ConversationRegion()
        {
            _playerCollider = new Lazy<Collider2D>(() => FindObjectOfType<PlayerControllerBehaviour>().GetComponent<Collider2D>());
            _conversationController = new Lazy<ConversationController>(() => FindObjectOfType<ConversationController>());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_alreadyTriggered)
                return;

            if (collision == _playerCollider.Value)
            {
                _conversationController.Value.ToggleVisibility();
            }
        }
    }
}
