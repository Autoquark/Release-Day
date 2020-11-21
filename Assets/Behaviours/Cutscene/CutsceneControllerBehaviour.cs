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
    class CutsceneControllerBehaviour : MonoBehaviour
    {
        protected PlayerControllerBehaviour PlayerControllerBehaviour => _playerControllerBehaviour.Value;

        private readonly Lazy<PlayerControllerBehaviour> _playerControllerBehaviour;
        protected IDictionary<string, TalkBehaviour> SpeakersByName { get; } = new Dictionary<string, TalkBehaviour>();

        public CutsceneControllerBehaviour()
        {
            _playerControllerBehaviour = new Lazy<PlayerControllerBehaviour>(() => FindObjectOfType<PlayerControllerBehaviour>());
        }

        public void RegisterSpeaker(string name, TalkBehaviour talkBehaviour) => SpeakersByName[name] = talkBehaviour;

        public IEnumerator WalkToX(PhysicsObject obj, float x, float speed)
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

        public void PlayConversation(Conversation conversation)
        {
            StartCoroutine(PlayConversationCoroutine(conversation));
        }

        private IEnumerator PlayConversationCoroutine(Conversation conversation)
        {
            var wasEnabled = _playerControllerBehaviour.Value.enabled;
            _playerControllerBehaviour.Value.enabled = false;

            // Wait a frame in case the conversation was triggered by pressing E, so it does not register as pressed immediately
            yield return null;
            while (conversation != null)
            {
                var speaker = SpeakersByName[conversation.Speaker];
                speaker.Say(conversation.Text);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                yield return null;
                speaker.ShowListening();
                conversation = conversation.Next?.SingleOrDefault();
            }
            foreach (var speaker in SpeakersByName.Values)
            {
                speaker.EndConversation();
            }

            _playerControllerBehaviour.Value.enabled = wasEnabled;
        }
    }
}
