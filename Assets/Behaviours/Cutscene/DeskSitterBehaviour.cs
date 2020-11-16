using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Assets.Behaviours.Cutscene
{
    class DeskSitterBehaviour : TalkBehaviour
    {
        public AnimationClip _idleAnimation;
        public AnimationClip _talkingAnimation;
        public Sprite _listeningSprite;

        private AnimationClipPlayable _idlePlayable;
        private AnimationClipPlayable _talkingPlayable;

        private PlayableGraph _playableGraph;
        private readonly Lazy<Animator> _animator;
        private readonly Lazy<SpriteRenderer> _spriteRenderer;
        private PlayableOutput _playableOutput;

        public DeskSitterBehaviour()
        {
            _animator = new Lazy<Animator>(GetComponent<Animator>);
            _spriteRenderer = new Lazy<SpriteRenderer>(GetComponent<SpriteRenderer>);
        }

        public override void Say(string text)
        {
            base.Say(text);
            _playableGraph.Play();
            _playableOutput.SetSourcePlayable(_talkingPlayable);
        }

        public override void ShowListening()
        {
            base.ShowListening();
            _playableGraph.Stop();
            _spriteRenderer.Value.sprite = _listeningSprite;
        }

        public override void EndConversation()
        {
            base.EndConversation();
            _playableGraph.Play();
            _playableOutput.SetSourcePlayable(_idlePlayable);
        }

        private void Start()
        {
            _playableGraph = PlayableGraph.Create();

            _playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            _playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", _animator.Value);

            _playableGraph.Play();

            _idlePlayable = AnimationClipPlayable.Create(_playableGraph, _idleAnimation);
            _talkingPlayable = AnimationClipPlayable.Create(_playableGraph, _talkingAnimation);

            _playableOutput.SetSourcePlayable(_idlePlayable);
        }

        private void OnDestroy()
        {
            _playableGraph.Destroy();
        }
    }
}
