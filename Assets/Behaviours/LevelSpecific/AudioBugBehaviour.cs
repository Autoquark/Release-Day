using Assets.Behaviours.Cutscene;
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
        private readonly Lazy<TalkBehaviour> _player;

        public AudioBugBehaviour()
        {
            _player = new Lazy<TalkBehaviour>(() => FindObjectOfType<PlayerControllerBehaviour>().GetComponent<TalkBehaviour>());
            _soundWave = new Lazy<GameObject>(() => transform.Find("SoundWave").gameObject);
        }

        private void Start()
        {
            _audioMixer.GetFloat("MusicVolume", out var value);
            if(value <= -80)
            {
                _audioMixer.SetFloat("MusicVolume", 0);
                _player.Value.Remark("Hey, why did the music turn itself back on?");
            }
        }

        private void Update()
        {
            _audioMixer.GetFloat("MusicVolume", out var value);
            _soundWave.Value.gameObject.SetActive(value > -80);
        }
    }
}
