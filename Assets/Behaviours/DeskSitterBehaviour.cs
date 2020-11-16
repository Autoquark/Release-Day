using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Assets.Behaviours
{
    class DeskSitterBehaviour : MonoBehaviour
    {
        public AnimationClip _idleAnimation;
        public AnimationClip _talkingAnimation;

        private AnimationClipPlayable _idlePlayable;
        private AnimationClipPlayable _talkingPlayable;

        private PlayableGraph _playableGraph;
        private Lazy<Animator> _animator;
        private PlayableOutput _playableOutput;

        public DeskSitterBehaviour()
        {
            _animator = new Lazy<Animator>(GetComponent<Animator>);
        }

        private void Start()
        {
            _playableGraph = PlayableGraph.Create();

            _playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            _playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", _animator.Value);

            _playableGraph.Play();

            _idlePlayable = AnimationClipPlayable.Create(_playableGraph, _idleAnimation);
            _talkingPlayable = AnimationClipPlayable.Create(_playableGraph, _talkingAnimation);

            StartCoroutine(AnimationCoroutine());
        }

        private IEnumerator AnimationCoroutine()
        {
            while (true)
            {
                _playableOutput.SetSourcePlayable(_idlePlayable);
                yield return new WaitForSeconds(2);
                _playableOutput.SetSourcePlayable(_talkingPlayable);
                yield return new WaitForSeconds(2);
            }
        }

        private void OnDestroy()
        {
            _playableGraph.Destroy();
        }
    }
}
