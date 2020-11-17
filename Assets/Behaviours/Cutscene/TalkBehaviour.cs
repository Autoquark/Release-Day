using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours.Cutscene
{
    class TalkBehaviour : MonoBehaviour
    {
        private Lazy<DialogueBubbleBehaviour> _dialogueBubble;

        public TalkBehaviour()
        {
            _dialogueBubble = new Lazy<DialogueBubbleBehaviour>(() => transform.Find("DialogueBubble").GetComponent<DialogueBubbleBehaviour>());
        }

        public virtual void Say(string text)
        {
            _dialogueBubble.Value.ShowDialogue(text);
        }

        public virtual void ShowListening()
        {
            _dialogueBubble.Value.ShowDialogue(null);
        }

        public virtual void EndConversation() { }
    }
}
