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
        class Info
        {
            public bool hasTriggered = false;
        }

        private Lazy<Collider2D> _playerCollider;
        private Lazy<ConversationController> _conversationController;
        private Lazy<ConversationLoader> _loader;

        private bool _alreadyTriggered = false;

        public ConversationRegion()
        {
            _playerCollider = new Lazy<Collider2D>(() => FindObjectOfType<PlayerControllerBehaviour>().GetComponent<Collider2D>());
            _conversationController = new Lazy<ConversationController>(() => FindObjectOfType<ConversationController>());
            _loader = new Lazy<ConversationLoader>(GetComponent<ConversationLoader>);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var data = LevelDataStore.GetOrCreate<Info>(gameObject.name);
            if (data.hasTriggered)
                return;

            if (collision == _playerCollider.Value)
            {
                _conversationController.Value.SetConversation(_loader.Value.Conversation);
                _conversationController.Value.SetVisibility(true);
                data.hasTriggered = true;
            }
        }
    }
}
