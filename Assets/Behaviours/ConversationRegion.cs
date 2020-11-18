﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class ConversationRegion : MonoBehaviour
    {
        const float _retriggerDelay = 1;

        public bool _requireGrounded = true;
        public bool _triggerAutomatically = true;
        public bool _hasNonHintOptions = true;
        public float _delayBeforeHintOptions = 30;

        class Info
        {
            public bool HasTriggered { get; set; } = false;
            public float? DelayStartTime { get; set; }
        }

        private Lazy<ConversationController> _conversationController;
        private Lazy<ConversationLoader> _loader;
        private bool _conversationButtonPressed = false;
        private float _lastTriggerTime = -999;

        public ConversationRegion()
        {
            _conversationController = new Lazy<ConversationController>(() => FindObjectOfType<ConversationController>());
            _loader = new Lazy<ConversationLoader>(GetComponent<ConversationLoader>);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                _conversationButtonPressed = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            var data = LevelDataStore.GetOrCreate<Info>(gameObject.name);

            if ((!_triggerAutomatically || !data.HasTriggered)
                && collision.GetComponent<PlayerControllerBehaviour>() != null
                && Time.time - _lastTriggerTime >= _retriggerDelay)
            {
                if (!data.DelayStartTime.HasValue)
                {
                    data.DelayStartTime = Time.time;
                }

                if ((!_requireGrounded || collision.GetComponent<PhysicsObject>().Grounded)
                    && (_hasNonHintOptions || Time.time - data.DelayStartTime > _delayBeforeHintOptions))
                {
                    if (_triggerAutomatically || _conversationButtonPressed)
                    {
                        _conversationController.Value.SetConversation(_loader.Value.Conversation);
                        _conversationController.Value.SetVisibility(true);
                        data.HasTriggered = true;
                        _lastTriggerTime = Time.time;
                    }
                    else if(!_triggerAutomatically)
                    {
                        _conversationController.Value.ShowAlertIconThisFrame();
                    }
                }
            }

            _conversationButtonPressed = false;
        }
    }
}
