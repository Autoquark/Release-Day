using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours.LevelSpecific
{
    class SoundWaveBehaviour : MonoBehaviour
    {
        public float _pushStrength = 35;

        private const float _retriggerDelay = 0.5f;

        private float _maxScale = 1.5f;
        private float _scaleRate = 3;
        private float _retriggerTime = 0f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var physicsObject = collision.GetComponent<PhysicsObject>();
            if(Time.fixedTime < _retriggerTime || physicsObject == null)
            {
                return;
            }

            var direction = (collision.transform.position - transform.position).normalized;

            // Overwrite velocity for a consistent bounce. Reduce Y effect since there's no vertical drag and this can easily send the player off the top of the map
            physicsObject.YVelocity = direction.y * _pushStrength * 0.4f;
            physicsObject.XVelocity += direction.x * _pushStrength;

            _retriggerTime = Time.fixedTime + _retriggerDelay;
        }

        private void FixedUpdate()
        {
            transform.localScale += new Vector3(_scaleRate * Time.fixedDeltaTime, _scaleRate * Time.fixedDeltaTime);
            if(transform.localScale.x > _maxScale)
            {
                transform.localScale = Vector3.one;
            }
        }
    }
}
