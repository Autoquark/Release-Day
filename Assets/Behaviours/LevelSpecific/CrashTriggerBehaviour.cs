﻿using Assets.Data;
using Assets.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Behaviours.LevelSpecific
{
    class CrashTriggerBehaviour : MonoBehaviour
    {
        public TextAsset _conversation;
        public string _nextScene;

        private readonly Lazy<ConversationController> _conversationController;
        private readonly Lazy<GameObject> _crashUi;

        public CrashTriggerBehaviour()
        {
            _conversationController = new Lazy<ConversationController>(() => FindObjectOfType<ConversationController>());
            _crashUi = new Lazy<GameObject>(() => GameObject.Find("CrashUi"));
        }

        private void Start()
        {
            _crashUi.Value.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!collision.HasComponent<PlayerControllerBehaviour>())
            {
                return;
            }

            StartCoroutine(Sequence());
        }

        private IEnumerator Sequence()
        {
            _crashUi.Value.SetActive(true);
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(5);
            _conversationController.Value.SetConversation(JsonUtility.FromJson<Conversation>(_conversation.text));
            _conversationController.Value.SetVisibility(true);
            yield return new WaitUntil(() => !_conversationController.Value.Visible);
            SceneManager.LoadScene(_nextScene);
        }
    }
}
