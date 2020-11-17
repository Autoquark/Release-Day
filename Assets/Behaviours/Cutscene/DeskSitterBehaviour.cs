using Assets.Data;
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
    class DeskSitterBehaviour : TalkBehaviour, IInteractable
    {
        public AnimationClip _idleAnimation;
        public AnimationClip _talkingAnimation;
        public Sprite _listeningSprite;
        public TextAsset _conversation;
        public string _name;

        private AnimationClipPlayable _idlePlayable;
        private AnimationClipPlayable _talkingPlayable;

        private PlayableGraph _playableGraph;
        private PlayableOutput _playableOutput;

        private Conversation _conversationData;

        private readonly Lazy<Animator> _animator;
        private readonly Lazy<SpriteRenderer> _spriteRenderer;
        private readonly Lazy<Collider2D> _playerCollider;
        private readonly Lazy<CutsceneControllerBehaviour> _cutsceneController;

        public string Message => "Press E to talk";

        public DeskSitterBehaviour()
        {
            _animator = new Lazy<Animator>(GetComponent<Animator>);
            _spriteRenderer = new Lazy<SpriteRenderer>(GetComponent<SpriteRenderer>);
            _playerCollider = new Lazy<Collider2D>(() => FindObjectOfType<PlayerControllerBehaviour>().GetComponent<Collider2D>());
            _cutsceneController = new Lazy<CutsceneControllerBehaviour>(() => FindObjectOfType<CutsceneControllerBehaviour>());
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

            if (_conversation != null && !string.IsNullOrEmpty(_name))
            {
                _conversationData = JsonUtility.FromJson<Conversation>(_conversation.text);
                _cutsceneController.Value.RegisterSpeaker(_name, this);
            }
        }

        private void OnDestroy()
        {
            _playableGraph.Destroy();
        }

        public bool CanInteract() => _conversation != null && !string.IsNullOrEmpty(_name)
            && Mathf.Abs(_playerCollider.Value.transform.position.x - transform.position.x) <= _playerCollider.Value.bounds.size.x + _spriteRenderer.Value.bounds.extents.x
            && Mathf.Abs(_playerCollider.Value.transform.position.y - transform.position.y) <= 0.5 * _playerCollider.Value.bounds.size.y;

        public void Interact() => StartCoroutine(_cutsceneController.Value.PlayConversation(_conversationData));
    }
}
