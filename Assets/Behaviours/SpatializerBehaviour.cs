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
    class SpatializerBehaviour : MonoBehaviour
    {
        public bool Spatialize = true;
        public float Volume = 1.0f;
        public float MaxHeard = 30f;
        public float FallDist = 15f;
        public float PanRange = 6f;

        private bool _first = true;

        private readonly Lazy<AudioSource[]> _audioSource;
        private readonly Lazy<Camera> _camera;

        SpatializerBehaviour()
        {
            _audioSource = new Lazy<AudioSource[]>(GetComponents<AudioSource>);
            _camera = new Lazy<Camera>(GameObject.FindObjectOfType<Camera>);
        }

        private void Update()
        {
            foreach (var audio in _audioSource.Value)
            {
                if (_first)
                {
                    audio.volume = Volume;
                }

                if (Spatialize)
                {
                    var rel_pos = (Vector2)(transform.position - _camera.Value.transform.position);
                    var dist = rel_pos.magnitude;
                    var rel_vol = dist > MaxHeard ? 0 : 1 / (1 + dist / FallDist);
                    audio.volume = rel_vol * Volume;

                    var pan = Mathf.Clamp(rel_pos.x / PanRange, -1, 1);
                    audio.panStereo = pan;
                }
            }

            _first = false;
        }
    }
}
