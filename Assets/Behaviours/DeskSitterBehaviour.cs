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

            StartCoroutine(AnimationCoroutine());
        }

        private IEnumerator AnimationCoroutine()
        {
            while (true)
            {
                PlayClip(_idleAnimation);
                yield return new WaitForSeconds(2);
                PlayClip(_talkingAnimation);
                yield return new WaitForSeconds(2);
            }
        }

        private void PlayClip(AnimationClip clip)
        {
            var clipPlayable = AnimationClipPlayable.Create(_playableGraph, clip);
            var toDestroy = _playableOutput.GetSourcePlayable();
            //if(toDestroy != null)
            {
                //_playableGraph.DestroyPlayable();
            }
            _playableOutput.SetSourcePlayable(clipPlayable);
        }
    }
}
