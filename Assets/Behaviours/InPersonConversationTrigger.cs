using Assets.Behaviours.Cutscene;
using Assets.Data;
using Assets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class InPersonConversationTrigger : MonoBehaviour
    {
        public TextAsset _conversation;

        private readonly Lazy<CutsceneControllerBehaviour> _cutsceneController;

        public InPersonConversationTrigger()
        {
            _cutsceneController = new Lazy<CutsceneControllerBehaviour>(FindObjectOfType<CutsceneControllerBehaviour>);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!collision.HasComponent<PlayerControllerBehaviour>())
            {
                return;
            }

            _cutsceneController.Value.PlayConversation(JsonUtility.FromJson<Conversation>(_conversation.text));

            Destroy(gameObject);
        }
    }
}
