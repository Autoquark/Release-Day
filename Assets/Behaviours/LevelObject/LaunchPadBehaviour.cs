using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours.LevelObject
{
    class LaunchPadBehaviour : MonoBehaviour
    {
        public float jumpPower = 10;
        public AudioClip LaunchSound;
        private readonly Lazy<AudioSource> _audioSource;

        LaunchPadBehaviour()
        {
            _audioSource = new Lazy<AudioSource>(GetComponent<AudioSource>);
        }

        internal void PlaySound(AudioClip clip)
        {
            if (!_audioSource.Value.isPlaying || _audioSource.Value.clip != clip)
            {
                _audioSource.Value.clip = clip;
                _audioSource.Value.Play();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var physics = collision.GetComponent<PhysicsObject>();
            if(physics == null)
            {
                return;
            }

            // Override any existing velocity, otherwise falling onto it results in a smaller jump
            physics.YVelocity = jumpPower;
            PlaySound(LaunchSound);
        }
    }
}
