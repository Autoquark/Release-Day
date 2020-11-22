using Assets.Data;
using Assets.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Behaviours;

namespace Assets.Behaviours.LevelSpecific
{
    class CrashTriggerBehaviour : MonoBehaviour
    {
        public TextAsset _conversation;
        public AudioClip _music;

        private readonly Lazy<ConversationController> _conversationController;
        private readonly Lazy<GameObject> _crashUi;
        private readonly Lazy<LevelControllerBehaviour> _levelController;

        public CrashTriggerBehaviour()
        {
            _conversationController = new Lazy<ConversationController>(() => FindObjectOfType<ConversationController>());
            _crashUi = new Lazy<GameObject>(() => GameObject.Find("CrashUi").transform.Find("Panel").gameObject);
            _levelController = new Lazy<LevelControllerBehaviour>(() => FindObjectOfType<LevelControllerBehaviour>());
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
            _levelController.Value.StopTime(gameObject, true);

            var musicController = FindObjectOfType<MusicController>();
            musicController.SetMusic(_music, false);
            yield return new WaitForSecondsRealtime(5);
            _conversationController.Value.SetConversation(JsonUtility.FromJson<Conversation>(_conversation.text));
            _conversationController.Value.SetVisibility(true);
            yield return new WaitUntil(() => !_conversationController.Value.Visible);
            _levelController.Value.GoToNextLevel();
        }

        private void OnDestroy()
        {
            _levelController.Value.StopTime(gameObject, false);
        }
    }
}
