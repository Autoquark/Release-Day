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
            int num_inserted = 0;

            if (_currentNode.Next.Length > 0)
            {
                foreach(var c in _currentNode.Next)
                {
                    num_inserted++;

                    AddNodeToIM(c);
                }
            }

            if (num_inserted == 1)
            {
                _lastInteraction = Time.realtimeSinceStartup;
                _currentNode = _currentNode.Next[0];
            }
            else
            {
                _lastInteraction = 0.0f;
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

            AddNodeToIM(_currentNode);

            _lastInteraction = Time.realtimeSinceStartup;

            Time.timeScale = 0;
        }

        private void ClearIM()
        {
            foreach(var child in _scrollPanel.Value.transform.Children())
            {
                GameObject.Destroy(child.gameObject);
            }

            _lastInteraction = 0.0f;
        }

        private void AddNodeToIM(Conversation c)
        {
            var msg = Instantiate(c.Speaker == "Alex" ? MessagePrefabLeft : MessagePrefabRight, _scrollPanel.Value.transform);
            msg.transform.localScale = new Vector3(1, 1, 1);
            var cms = msg.GetComponent<ConversationMessageUtil>();
            cms.SetMessage(c);

            _newContentAdded = true;
        }
    }
}
