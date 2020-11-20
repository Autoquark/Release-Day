using System;
using UnityEngine;
using Assets.Data;
using Assets.Extensions;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Behaviours
{
    class ConversationController : MonoBehaviour
    {
        public GameObject MessagePrefabLeft;
        public GameObject MessagePrefabRight;
        public GameObject MessagePrefabOptions;

        public bool Visible => _rootPanel.Value.activeSelf;

        public void ShowAlertIconThisFrame()
        {
            _showAlertIcon = true;
        }

        private bool _showAlertIcon = false;
        private readonly Lazy<GameObject> _rootPanel;
        private readonly Lazy<GameObject> _scrollContent;
        private readonly Lazy<ScrollRect> _scrollRect;
        private readonly Lazy<GameObject> _alertIcon;
        private readonly Lazy<Text> _promptText;
        private Conversation _conversation;
        private Conversation _currentNode;
        private Conversation _selectionsNode;
        private bool _newContentAdded = false;
        private readonly Lazy<LevelControllerBehaviour> _levelController;

        public ConversationController()
        {
            _rootPanel = new Lazy<GameObject>(() => transform.Find("MessageRoot").gameObject);
            _scrollContent = new Lazy<GameObject>(() => _rootPanel.Value.transform.Find("Background/ScrollPanel/Viewport/ScrollContent").gameObject);
            _alertIcon = new Lazy<GameObject>(() => transform.Find("AlertIcon").gameObject);
            _scrollRect = new Lazy<ScrollRect>(() => _rootPanel.Value.transform.Find("Background/ScrollPanel").GetComponent<ScrollRect>());
            _promptText = new Lazy<Text>(() => _rootPanel.Value.transform.Find("Background/PromptPanel/Text").GetComponent<Text>());
            _levelController = new Lazy<LevelControllerBehaviour>(() => GameObject.FindObjectOfType<LevelControllerBehaviour>());
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

            if (Input.GetKeyDown(KeyCode.E) && _currentNode != null)
            {
                AdvanceConversation();
            }

            int numNextNodes = _currentNode?.Next.Length ?? 0;

            if (_selectionsNode == null
                && numNextNodes == 0
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
            int numNextNodes = _currentNode?.Next.Length ?? 0;
            _selectionsNode = null;

            if (numNextNodes == 1)
            {
                _currentNode = _currentNode.Next[0];
                AddStatementToIM(_currentNode);
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
            }
        }

        public void SetVisibility(bool visible)
        {
            _rootPanel.Value.SetActive(visible);

            _levelController.Value.StopTime(gameObject, visible);
        }

        public void SetConversation(Conversation conv)
        {
            ClearIM();

            _conversation = conv;
            _currentNode = conv;

            AddStatementToIM(_currentNode);

            _promptText.Value.text = _currentNode.Next.Any() ? "E to advance" : "Q to exit";
        }

        private void ClearIM()
        {
            _scrollContent.Value.transform.DestroyChildren();
        }

        private void AddStatementToIM(Conversation c)
        {
            bool is_right = c.Speaker != "Alex";

            var msg = Instantiate(is_right ? MessagePrefabRight : MessagePrefabLeft, _scrollContent.Value.transform);
            msg.transform.localScale = new Vector3(1, 1, 1);
            var cms = msg.GetComponent<ConversationMessageUtil>();
            cms.SetMessage(c, is_right);

            _promptText.Value.text = _currentNode.Next.Any() ? "E to advance" : "Q to exit";

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
