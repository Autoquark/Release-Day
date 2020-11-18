using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Data
{
    [Serializable]
    class Conversation
    {
        public String Speaker;
        public String Text;
        public bool IsHint = false;
        public Conversation[] Next = new Conversation[0];
    }
}
