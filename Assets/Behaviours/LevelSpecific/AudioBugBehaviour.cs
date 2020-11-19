using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Behaviours.LevelSpecific
{
    class AudioBugBehaviour : MonoBehaviour
    {
        public AudioMixer _audioMixer;

        private readonly Lazy<GameObject> _soundWave;

        public AudioBugBehaviour()
        {
            _soundWave = new Lazy<GameObject>(() => transform.Find("SoundWave").gameObject);
        }

        private void Update()
        {
            _audioMixer.GetFloat("MusicVolume", out var value);
            _soundWave.Value.gameObject.SetActive(value > -80);
        }
    }
}
