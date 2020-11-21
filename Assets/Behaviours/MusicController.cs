using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Behaviours
{
    class MusicController : MonoBehaviour
    {
        private readonly Lazy<AudioSource> _audioSource;

        public MusicController()
        {
            _audioSource = new Lazy<AudioSource>(GetComponent<AudioSource>);
        }

        public void SetMusic(AudioClip clip, bool loop = true)
        {
            if (_audioSource.Value.clip == clip)
            {
                return;
            }

            _audioSource.Value.clip = clip;
            _audioSource.Value.loop = loop;
            _audioSource.Value.Play();
        }

        private void Awake()
        {
            // If we already have a music player, let that one keep existing so the music isn't restarted
            if(FindObjectsOfType<MusicController>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            // Unparent ourselves so DontDestroyOnLoad will work - we're only in the prefab for convenience, we don't need to be in the hierarchy
            transform.parent = null;
            SetMusic(FindObjectOfType<LevelControllerBehaviour>().levelMusic);
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            DontDestroyOnLoad(gameObject);
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            SetMusic(FindObjectOfType<LevelControllerBehaviour>().levelMusic);
        }
    }
}
