using System;
using UnityEngine;
using Assets.Data;
using Assets.Extensions;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Behaviours
{
    class ConversationController : MonoBehaviour
    {
        public GameObject MessagePrefabLeft;
        public GameObject MessagePrefabRight;
        public GameObject MessagePrefabOptions;
        public Sprite AlertImage1;
        public Sprite AlertImage2;
        public AudioClip AlertSound;
        public AudioClip OpenSound;
        public AudioClip CloseSound;
        public AudioClip DoSound;

        public bool Visible => _rootPanel.Value.activeSelf;

        public void ShowAlertIconThisFrame(bool suppressPing)
        {
            _showAlertIconThisFrame = true;
            _suppressPing = suppressPing;
        }

        private bool _showAlertIconThisFrame = false;
        private bool _showAlertIconLastFrame = false;
        private bool _suppressPing = false;
        private readonly Lazy<GameObject> _rootPanel;
        private readonly Lazy<GameObject> _scrollContent;
        private readonly Lazy<ScrollRect> _scrollRect;
        private readonly Lazy<GameObject> _alertIcon;
        private readonly Lazy<Image> _alertIconInner;
        private readonly Lazy<Text> _promptText;
        private readonly Lazy<AudioSource> _audioSource;
        private Conversation _conversation;
        private Conversation _currentNode;
        private Conversation _selectionsNode;
        private bool _newContentAdded = false;
        private bool _unlockHints = false;
        private readonly Lazy<LevelControllerBehaviour> _levelController;

        public ConversationController()
        {
            _rootPanel = new Lazy<GameObject>(() => transform.Find("MessageRoot").gameObject);
            _scrollContent = new Lazy<GameObject>(() => _rootPanel.Value.transform.Find("Background/ScrollPanel/Viewport/ScrollContent").gameObject);
            _alertIcon = new Lazy<GameObject>(() => transform.Find("AlertIcon").gameObject);
            _alertIconInner = new Lazy<Image>(() => _alertIcon.Value.transform.Find("AlertIconInner").GetComponent<Image>());
            _scrollRect = new Lazy<ScrollRect>(() => _rootPanel.Value.transform.Find("Background/ScrollPanel").GetComponent<ScrollRect>());
            _promptText = new Lazy<Text>(() => _rootPanel.Value.transform.Find("Background/PromptPanel/Text").GetComponent<Text>());
            _levelController = new Lazy<LevelControllerBehaviour>(() => GameObject.FindObjectOfType<LevelControllerBehaviour>());
            _audioSource = new Lazy<AudioSource>(GetComponent<AudioSource>);
        }

        private void Start()
        {
            SetVisibility(false);
        }

        private void Update()
        {
            if (_showAlertIconThisFrame)
            {
                float adj_t = Time.time / 1.3f;
                bool which = (adj_t - Mathf.Floor(adj_t)) > 0.8f;

                Sprite spr = which ? AlertImage2 : AlertImage1;

                _alertIconInner.Value.sprite = spr;

                if (!_suppressPing)
                {
                    PlaySound(AlertSound);
                }
            }

            _showAlertIconLastFrame = _showAlertIconThisFrame;

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
            _alertIcon.Value.SetActive(_showAlertIconThisFrame);
            _showAlertIconThisFrame = false;
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
                AddOptionsToIM(_currentNode.Next.Where(x => !x.IsHint || _unlockHints));
                _selectionsNode = _currentNode;
                _currentNode = null;

                _promptText.Value.text = "W/S or Up/Down select, E to confirm";
            }
            else
            {
                _currentNode = null;
            }

            PlaySound(DoSound);
        }

        public void SetVisibility(bool visible)
        {
            bool was_active = _rootPanel.Value.activeSelf;

            if (was_active != visible)
            {
                _rootPanel.Value.SetActive(visible);

                _levelController.Value.StopTime(gameObject, visible);

                if (visible)
                {
                    PlaySound(OpenSound);
                    PlayerControllerBehaviour player = PlayerControllerBehaviour.FirstPlayer();
                    if (player != null)
                    {
                        player.StopFootsteps();
                    }
                }
                else
                {
                    PlaySound(CloseSound);
                }
            }
        }

        private void PlaySound(AudioClip clip)
        {
            if (!_audioSource.Value.isPlaying || _audioSource.Value.clip != clip)
            {
                _audioSource.Value.clip = clip;
                _audioSource.Value.Play();
            }
        }

        public void SetConversation(Conversation conv, bool unlockHints = false)
        {
            ClearIM();

            _conversation = conv;
            _currentNode = conv;
            _unlockHints = unlockHints;

            AddStatementToIM(_currentNode);

            _promptText.Value.text = _currentNode.Next.Any() ? "E to advance" : "Q to close";
        }

        private void ClearIM()
        {
            _scrollContent.Value.transform.DestroyChildren();
        }

        private void AddStatementToIM(Conversation c)
        {
            GreyExistingStatements();

            bool is_right = c.Speaker != "Alex";

            var msg = Instantiate(is_right ? MessagePrefabRight : MessagePrefabLeft, _scrollContent.Value.transform);
            msg.transform.localScale = new Vector3(1, 1, 1);
            var cms = msg.GetComponent<ConversationMessageUtil>();
            cms.SetMessage(c, is_right);

            _promptText.Value.text = _currentNode.Next.Any() ? "E to advance" : "Q to close";

            _newContentAdded = true;
        }

        private void AddOptionsToIM(IEnumerable<Conversation> options)
        {
            GreyExistingStatements();

            var msg = Instantiate(MessagePrefabOptions, _scrollContent.Value.transform);
            msg.transform.localScale = new Vector3(1, 1, 1);
            var cms = msg.GetComponent<ConversationMessageUtil>();
            cms.SetOptions(options.ToArray(), false, this);

            _newContentAdded = true;
        }

        private void GreyExistingStatements()
        {
            foreach(var obj in _scrollContent.Value.transform.Children())
            {
                var ctrl = obj.transform.GetComponent<ConversationMessageUtil>();
                ctrl.Grey();
            }
        }
    }
}
