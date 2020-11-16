using Assets.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    abstract class CutsceneControllerBehaviour : MonoBehaviour
    {
        protected PlayerControllerBehaviour PlayerControllerBehaviour => _playerControllerBehaviour.Value;

        private Lazy<PlayerControllerBehaviour> _playerControllerBehaviour;
        protected IDictionary<string, DialogueBubbleBehaviour> SpeakersByName { get; } = new Dictionary<string, DialogueBubbleBehaviour>();

        public CutsceneControllerBehaviour()
        {
            _playerControllerBehaviour = new Lazy<PlayerControllerBehaviour>(() => FindObjectOfType<PlayerControllerBehaviour>());
        }

        protected IEnumerator WalkToX(PhysicsObject obj, float x, float speed)
        {
            while (Mathf.Abs(obj.transform.position.x - x) > 0.01f)
            {
                obj.WalkIntent = Mathf.Clamp(x - obj.transform.position.x,
                    -speed,
                    speed);
                yield return null;
            }
            obj.WalkIntent = 0;
        }

        protected IEnumerator PlayConversation(Conversation conversation)
        {
            while (conversation != null)
            {
                SpeakersByName[conversation.Speaker].SetText(conversation.Text);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                yield return null;
                SpeakersByName[conversation.Speaker].SetText(null);
                conversation = conversation.Next?.SingleOrDefault();
            }
        }
    }
}
