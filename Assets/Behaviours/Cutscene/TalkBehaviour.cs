﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours.Cutscene
{
    class TalkBehaviour : MonoBehaviour
    {
        public string talkerName;
        private readonly Lazy<DialogueBubbleBehaviour> _dialogueBubble;
        private readonly Lazy<CutsceneControllerBehaviour> _cutsceneController;

        public TalkBehaviour()
        {
            _dialogueBubble = new Lazy<DialogueBubbleBehaviour>(() => transform.Find("DialogueBubble").GetComponent<DialogueBubbleBehaviour>());
            _cutsceneController = new Lazy<CutsceneControllerBehaviour>(FindObjectOfType<CutsceneControllerBehaviour>);
        }

        protected virtual void Start()
        {
            _cutsceneController.Value?.RegisterSpeaker(talkerName, this);
        }

        public void Remark(string text)
        {
            _dialogueBubble.Value.ShowRemark(text);
        }

        public virtual void Say(string text)
        {
            _dialogueBubble.Value.ShowCutsceneDialogue(text);
        }

        public virtual void ShowListening()
        {
            _dialogueBubble.Value.ShowCutsceneDialogue(null);
        }

        public virtual void EndConversation() { }
    }
}
