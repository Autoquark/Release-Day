using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Data;

namespace Assets.Behaviours
{
    class ConversationLoader : MonoBehaviour
    {
        public TextAsset InputFile;
        public Conversation Conversation;

        private void Start()
        {
            Conversation = JsonUtility.FromJson<Conversation>(InputFile.text);
        }
    }
}
