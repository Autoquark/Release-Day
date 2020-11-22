using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Behaviours.Cutscene
{
    class PlayerTalkBehaviour : TalkBehaviour
    {
        private readonly Lazy<PlayerAnimationBehaviour> _playerAnimation;

        public PlayerTalkBehaviour()
        {
            _playerAnimation = new Lazy<PlayerAnimationBehaviour>(GetComponent<PlayerAnimationBehaviour>);
        }

        public override void Say(string text)
        {
            _playerAnimation.Value.IsTalking = true;
            base.Say(text);
        }

        public override void ShowListening()
        {
            _playerAnimation.Value.IsTalking = false;
            base.ShowListening();
        }

        public override void EndConversation()
        {
            _playerAnimation.Value.IsTalking = false;
            base.EndConversation();
        }
    }
}
