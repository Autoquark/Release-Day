using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Data;
using Spine.Unity;

namespace Assets.Behaviours
{
    class TeleporterBehaviour : AnimationBase
    {
        public AnimationReferenceAsset activateAnimation, idleAnimation;
        public bool HasBug = false;
        public TeleporterBehaviour SendsTo;
        public AudioClip Sound;

        private float _coolDownEnd = 0.0f;
        private bool _usedOnce = false;
        protected ContactFilter2D Filter;
        private readonly Lazy<AudioSource> _audioSource;

        public TeleporterBehaviour()
        {
            _audioSource = new Lazy<AudioSource>(GetComponent<AudioSource>);
        }

        public void Start()
        {
            Filter.useTriggers = false;
            Filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            _skeletonAnimation.Value.Initialize(false);
            _skeletonAnimation.Value.AnimationState.Complete += AnimationState_Complete;
        }

        private void AnimationState_Complete(Spine.TrackEntry trackEntry) => SetAnimationIfDifferent(idleAnimation);

        private void OnTriggerStay2D(Collider2D hitBy)
        {
            if (_coolDownEnd > Time.time)
                return;

            PlayerControllerBehaviour player = hitBy.GetComponent<PlayerControllerBehaviour>();

            if (SendsTo != null && player != null)
            {
                var capsule = player.GetComponent<CapsuleCollider2D>();

                if (Physics2D.OverlapCapsule(SendsTo.transform.position, capsule.size, capsule.direction, capsule.transform.eulerAngles.z, Filter, new Collider2D[1]) == 0)
                {
                    if (!HasBug)
                    {
                        if (!_usedOnce)
                        {
                            player.GetComponent<Rigidbody2D>().position = SendsTo.transform.position;
                            SendsTo._usedOnce = true;
                        }
                    }
                    else
                    {
                        Instantiate(player.gameObject, SendsTo.transform.position, SendsTo.transform.rotation);
                    }

                    _coolDownEnd = SendsTo._coolDownEnd = Time.time + 1.5f;
                    SetAnimationIfDifferent(activateAnimation, false);
                    SendsTo.SetAnimationIfDifferent(activateAnimation, false);
                    SendsTo.PlaySound(Sound);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D hitBy)
        {
            PlayerControllerBehaviour player = hitBy.GetComponent<PlayerControllerBehaviour>();

            if (player != null)
            {
                // wait for the player to step off the teleporter before triggering again
                _usedOnce = false;
            }
        }

        internal void PlaySound(AudioClip clip)
        {
            if (!_audioSource.Value.isPlaying || _audioSource.Value.clip != clip)
            {
                _audioSource.Value.clip = clip;
                _audioSource.Value.Play();
            }
        }
    }
}
