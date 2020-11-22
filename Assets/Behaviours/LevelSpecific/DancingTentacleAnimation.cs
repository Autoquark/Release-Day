using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Audio;

namespace Assets.Behaviours.LevelSpecific
{
    class DancingTentacleAnimation : DecorAnimation
    {
        public AudioMixer mixer;

        private float _oldMinSpeed;

        private void Update()
        {
            mixer.GetFloat("MusicVolume", out var volume);
            if (volume == -80 && MinSpeed > 0)
            {
                _oldMinSpeed = MinSpeed;
                MinSpeed = _targetSpeed = _speed = 0;
            }
            else if(volume > -80 && MinSpeed == 0)
            {
                MinSpeed = _targetSpeed = _speed = _oldMinSpeed;
            }
        }
    }
}
