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
        public bool _requireGrounded = true;
        public bool _triggerAutomatically = true;
        public bool _hasNonHintOptions = true;
        public float _delayBeforeHintOptions = 30;

        class Info
        {
            public bool HasTriggered { get; set; } = false;
            public float? DelayStartTime { get; set; }
        }

        private Lazy<Collider2D> _playerCollider;
        private Lazy<ConversationController> _conversationController;
        private Lazy<ConversationLoader> _loader;

        public ConversationRegion()
        {
            _playerCollider = new Lazy<Collider2D>(() => FindObjectOfType<PlayerControllerBehaviour>().GetComponent<Collider2D>());
            _conversationController = new Lazy<ConversationController>(() => FindObjectOfType<ConversationController>());
            _loader = new Lazy<ConversationLoader>(GetComponent<ConversationLoader>);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            var data = LevelDataStore.GetOrCreate<Info>(gameObject.name);
            if (_triggerAutomatically && data.HasTriggered)
                return;

            if (collision == _playerCollider.Value)
            {
                if (!data.DelayStartTime.HasValue)
                {
                    data.DelayStartTime = Time.time;
                }

                if ((!_requireGrounded || collision.GetComponent<PhysicsObject>().Grounded)
                    && (_triggerAutomatically || Input.GetKeyDown(KeyCode.Q))
                    && (_hasNonHintOptions || Time.time - data.DelayStartTime > _delayBeforeHintOptions))
                {
                    _conversationController.Value.SetConversation(_loader.Value.Conversation);
                    _conversationController.Value.SetVisibility(true);
                    data.HasTriggered = true;
                }
            }
        }
    }
}
