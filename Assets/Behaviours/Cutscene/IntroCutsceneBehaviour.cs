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

        void Start()
        {
            StartCoroutine(CutsceneCoroutine());
        }

        IEnumerator CutsceneCoroutine()
        {
            var player = FindObjectOfType<PlayerControllerBehaviour>();
            player.enabled = false;
            var playerPhysics = player.GetComponent<PhysicsObject>();

            var layla = GameObject.Find("Layla");
            yield return WalkToX(playerPhysics,
                layla.transform.position.x + layla.GetComponent<SpriteRenderer>().sprite.bounds.min.x - player.GetComponent<MeshRenderer>().bounds.extents.x,
                player.walkSpeed);

            SpeakersByName["Alex"] = player.transform.Find("DialogueBubble").GetComponent<DialogueBubbleBehaviour>();
            SpeakersByName["Layla"] = layla.transform.Find("DialogueBubble").GetComponent<DialogueBubbleBehaviour>();

            yield return PlayConversation(JsonUtility.FromJson<Conversation>(laylaConversation.text));
        }
    }
}
