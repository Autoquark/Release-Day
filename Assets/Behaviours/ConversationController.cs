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

        public bool Visible => _maskPanel.Value.activeSelf;

        public void ShowAlertIconThisFrame()
        {
            _showAlertIcon = true;
        }

        private bool _showAlertIcon = false;
        private Lazy<GameObject> _maskPanel;
        private Lazy<GameObject> _scrollContent;
        private Lazy<ScrollRect> _scrollRect;
        private Lazy<GameObject> _alertIcon;
        private Lazy<Text> _promptText;
        private Conversation _conversation;
        private Conversation _currentNode;
        private Conversation _selectionsNode;
        private float _lastInteraction = 0.0f;
        private bool _newContentAdded = false;

        public ConversationController()
        {
            _maskPanel = new Lazy<GameObject>(() => transform.Find("MaskPanel").gameObject);
            _scrollContent = new Lazy<GameObject>(() => transform.Find("MaskPanel/ScrollPanel/ScrollContent").gameObject);
            _alertIcon = new Lazy<GameObject>(() => transform.Find("AlertIcon").gameObject);
            _scrollRect = new Lazy<ScrollRect>(() => transform.Find("MaskPanel/ScrollPanel").GetComponent<ScrollRect>());
            _promptText = new Lazy<Text>(() => transform.Find("MaskPanel/PromptPanel").GetComponent<Text>());
        }

        private void Start()
        {
            SetVisibility(false);
        }

        private void Update()
        {
            if (!Visible)
            {
                return;
            }

            _alertIcon.Value.SetActive(false);

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

            if (_currentNode == null
                && _selectionsNode == null
                && Input.GetKeyDown(KeyCode.Q))
            {
                ClearIM();

                SetVisibility(false);
            }
        }

        private void FixedUpdate()
        {
            _alertIcon.Value.SetActive(_showAlertIcon);
            _showAlertIcon = false;
        }

        internal void SelectionMade(int selectedOption)
        {
            RemoveLastStatement();
            _currentNode = _selectionsNode.Next[selectedOption];
            AddStatementToIM(_currentNode);
            AdvanceConversation();
        }

        private void RemoveLastStatement()
        {
            _scrollContent.Value.transform.DestroyChild(_scrollContent.Value.transform.childCount - 1);
        }

        private void AdvanceConversation()
        {
            _lastInteraction = 0.0f;

            int numNextNodes = _currentNode != null ? _currentNode.Next != null ? _currentNode.Next.Length : 0 : 0;

            if (numNextNodes == 1)
            {
                _currentNode = _currentNode.Next[0];
                _selectionsNode = null;
                AddStatementToIM(_currentNode);
                _lastInteraction = Time.realtimeSinceStartup;

                _promptText.Value.text = "(online)";
            }
            else if (numNextNodes > 1)
            {
                AddOptionsToIM(_currentNode.Next);
                _selectionsNode = _currentNode;
                _currentNode = null;

                _promptText.Value.text = "E to select";
            }
            else
            {
                _currentNode = null;

                _promptText.Value.text = "Q to exit";
            }
        }

        public void SetVisibility(bool visible)
        {
            _maskPanel.Value.SetActive(visible);

            Time.timeScale = visible ? 0 : 1;
        }

        public void SetConversation(Conversation conv)
        {
            ClearIM();

            _conversation = conv;
            _currentNode = conv;

            AddStatementToIM(_currentNode);

            _lastInteraction = Time.realtimeSinceStartup;
        }

        private void ClearIM()
        {
            _scrollContent.Value.transform.DestroyChildren();

            _lastInteraction = 0.0f;
        }

        private void AddStatementToIM(Conversation c)
        {
            bool is_right = c.Speaker != "Alex";

            var msg = Instantiate(is_right ? MessagePrefabRight : MessagePrefabLeft, _scrollContent.Value.transform);
            msg.transform.localScale = new Vector3(1, 1, 1);
            var cms = msg.GetComponent<ConversationMessageUtil>();
            cms.SetMessage(c, is_right);

            _newContentAdded = true;
        }

        private void AddOptionsToIM(Conversation[] c)
        {
            var msg = Instantiate(MessagePrefabOptions, _scrollContent.Value.transform);
            msg.transform.localScale = new Vector3(1, 1, 1);
            var cms = msg.GetComponent<ConversationMessageUtil>();
            cms.SetOptions(c, false, this);

            _newContentAdded = true;
        }

    }
}
