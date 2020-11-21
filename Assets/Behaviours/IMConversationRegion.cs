using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class IMConversationRegion : MonoBehaviour
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
            public bool AlreadyPinged { get; set; }
            public bool AlreadyPingedForHints { get; set; }
        }

        private readonly Lazy<ConversationController> _conversationController;
        private readonly Lazy<ConversationLoader> _loader;
        private readonly Lazy<LevelControllerBehaviour> _levelController;
        private bool _conversationButtonPressed = false;
        private float _lastTriggerTime = -999;

        public IMConversationRegion()
        {
            _conversationController = new Lazy<ConversationController>(() => FindObjectOfType<ConversationController>());
            _loader = new Lazy<ConversationLoader>(GetComponent<ConversationLoader>);
            _levelController = new Lazy<LevelControllerBehaviour>(() => GameObject.FindObjectOfType<LevelControllerBehaviour>());
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q) && !_levelController.Value.IsTimeStopped)
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

                var hintsUnlocked = Time.time - data.DelayStartTime > _delayBeforeHintOptions;

                if ((!_requireGrounded || collision.GetComponent<PhysicsObject>().Grounded)
                    && (_hasNonHintOptions || hintsUnlocked))
                {
                    if (_triggerAutomatically || _conversationButtonPressed)
                    {
                        _conversationController.Value.SetConversation(_loader.Value.Conversation, hintsUnlocked);
                        _conversationController.Value.SetVisibility(true);
                        data.HasTriggered = true;
                        _lastTriggerTime = Time.time;
                    }
                    else if(!_triggerAutomatically)
                    {
                        if (hintsUnlocked)
                        {
                            _conversationController.Value.ShowAlertIconThisFrame(data.AlreadyPingedForHints);
                            data.AlreadyPingedForHints = true;
                            data.AlreadyPinged = true;
                        }
                        else
                        {
                            _conversationController.Value.ShowAlertIconThisFrame(data.AlreadyPinged);
                            data.AlreadyPinged = true;
                        }

                    }
                }
            }

            _conversationButtonPressed = false;
        }
    }
}
