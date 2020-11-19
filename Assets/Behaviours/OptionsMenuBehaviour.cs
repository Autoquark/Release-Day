using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.Behaviours
{
    class OptionsMenuBehaviour : MonoBehaviour
    {
        public AudioMixer _audioMixer;

        private readonly Lazy<LevelControllerBehaviour> _levelController;
        private readonly Lazy<Slider> _musicSlider;
        private readonly Lazy<Slider> _soundSlider;
        private readonly Lazy<MenuRootBehaviour> _menuRoot;

        private bool _slidersInitialized = false;

        public OptionsMenuBehaviour()
        {
            _menuRoot = new Lazy<MenuRootBehaviour>(() => GetComponentInParent<MenuRootBehaviour>());
            _levelController = new Lazy<LevelControllerBehaviour>(() => FindObjectOfType<LevelControllerBehaviour>());
            _musicSlider = new Lazy<Slider>(() => transform.Find("Content/MusicSliderRow/Slider").GetComponent<Slider>());
            _soundSlider = new Lazy<Slider>(() => transform.Find("Content/SoundSliderRow/Slider").GetComponent<Slider>());
        }

        private void Start()
        {
            UpdateAudioSliders();
        }

        private void OnEnable()
        {
            UpdateAudioSliders();
            _levelController.Value.StopTime(gameObject, true);
        }

        private void UpdateAudioSliders()
        {
            _slidersInitialized = true;
            _audioMixer.GetFloat("MusicVolume", out var volume);
            _musicSlider.Value.value = Mathf.Pow(10, volume / 20);
            _audioMixer.GetFloat("SoundVolume", out volume);
            _soundSlider.Value.value = Mathf.Pow(10, volume / 20);
        }

        private void OnDisable()
        {
            _levelController.Value.StopTime(gameObject, false);
        }

        private void OnDestroy()
        {
            _levelController.Value.StopTime(gameObject, false);
        }

        public void OnMusicSliderChange(float value)
        {
            if(!_slidersInitialized)
            {
                return;
            }
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        }

        public void OnSoundSliderChange(float value)
        {
            if (!_slidersInitialized)
            {
                return;
            }
            _audioMixer.SetFloat("SoundVolume", Mathf.Log10(value) * 20);
        }

        public void OnBack()
        {
            _menuRoot.Value.ShowInGameMenu();
        }
    }
}
