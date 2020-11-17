using Assets.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours.Cutscene
{
    class IntroCutsceneBehaviour : CutsceneControllerBehaviour
    {
        public TextAsset laylaConversation;
        public TextAsset henryConversation;
        public TextAsset myraConversation;
        public TextAsset colinConversation;
        public TextAsset francesConversation;

        void Start()
        {
            StartCoroutine(CutsceneCoroutine());
        }

        IEnumerator CutsceneCoroutine()
        {
            var player = FindObjectOfType<PlayerControllerBehaviour>();
            player.enabled = false;

            var frances = GameObject.Find("Frances");

            SpeakersByName["Alex"] = player.GetComponent<TalkBehaviour>();
            SpeakersByName["Frances"] = frances.GetComponent<TalkBehaviour>();

            yield return PlayConversation(JsonUtility.FromJson<Conversation>(francesConversation.text));
            player.enabled = true;
        }
    }
}
