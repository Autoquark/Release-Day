using System;
using UnityEngine;
using Assets.Data;
using Assets.Extensions;
using UnityEngine.UI;

namespace Assets.Behaviours
{
    class ConversationController : MonoBehaviour
    {
        public GameObject MessagePrefabLeft;
        public GameObject MessagePrefabRight;
        public GameObject MessagePrefabOptions;

        private bool _visible = false;
        private Lazy<Canvas> _canvas;
        private Lazy<GameObject> _scrollPanel;
        private Lazy<ScrollRect> _scrollRect;
        private Conversation _conversation;
        private Conversation _currentNode;
        private float _lastInteraction = 0.0f;
        private bool _newContentAdded = false;

        public ConversationController()
        {
            _canvas = new Lazy<Canvas>(GetComponent<Canvas>);
            _scrollPanel = new Lazy<GameObject>(() => transform.Find("ScrollPanel/ScrollContent").gameObject);
            _scrollRect = new Lazy<ScrollRect>(() => transform.Find("ScrollPanel").GetComponent<ScrollRect>());
        }

        private void Start()
        {
            SetVisibility(false);
        }

        private void Update()
        {
            if (_newContentAdded)
            {
                _scrollRect.Value.verticalNormalizedPosition = 0;
                _newContentAdded = false;
            }

            if (_lastInteraction != 0
                && Time.realtimeSinceStartup > _lastInteraction + 2.0f)
            {
                AdvanceConversation();
            }
        }

        private void AdvanceConversation()
        {
            _lastInteraction = 0.0f;

            if (_currentNode.Next.Length == 1)
            {
                _currentNode = _currentNode.Next[0];
                AddStatementToIM(_currentNode);
                _lastInteraction = Time.realtimeSinceStartup;
            }
            else if (_currentNode.Next.Length > 1)
            {
                AddOptionsToIM(_currentNode.Next);
                _currentNode = null;
            }
            else
            {
            }
        }

        public void SetVisibility(bool visible)
        {
            _visible = visible;

            _canvas.Value.enabled = _visible;
        }

        public void SetConversation(Conversation conv)
        {
            ClearIM();

            _conversation = conv;
            _currentNode = conv;

            AddStatementToIM(_currentNode);

            _lastInteraction = Time.realtimeSinceStartup;

            Time.timeScale = 0;
        }

        private void ClearIM()
        {
            _scrollPanel.Value.transform.DestroyChildren();

            _lastInteraction = 0.0f;
        }

        private void AddStatementToIM(Conversation c)
        {
            bool is_right = c.Speaker != "Alex";

            var msg = Instantiate(is_right ? MessagePrefabRight : MessagePrefabLeft, _scrollPanel.Value.transform);
            msg.transform.localScale = new Vector3(1, 1, 1);
            var cms = msg.GetComponent<ConversationMessageUtil>();
            cms.SetMessage(c, is_right);

            _newContentAdded = true;
        }

        private void AddOptionsToIM(Conversation[] c)
        {
            var msg = Instantiate(MessagePrefabOptions, _scrollPanel.Value.transform);
            msg.transform.localScale = new Vector3(1, 1, 1);
            var cms = msg.GetComponent<ConversationMessageUtil>();
            cms.SetOptions(c, false);

            _newContentAdded = true;
        }

    }
}
