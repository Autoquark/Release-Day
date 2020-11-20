using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Data;
using UnityEngine.Tilemaps;

namespace Assets.Behaviours
{
    class CorpseController : MonoBehaviour
    {
        private readonly Lazy<Tilemap> _tileMap;
        private readonly Lazy<AudioSource> _audioSource;

        CorpseController()
        {
            _tileMap = new Lazy<Tilemap>(FindObjectOfType<Tilemap>);
            _audioSource = new Lazy<AudioSource>(GetComponent<AudioSource>);
        }

        private void Update()
        {
            if (_tileMap.Value.localBounds.SqrDistance(transform.position) > 400)
            {
                GameObject.Destroy(gameObject);
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
