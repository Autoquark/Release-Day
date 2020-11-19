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
        public float _pushStrength = 50;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var physicsObject = collision.GetComponent<PhysicsObject>();
            if(physicsObject == null)
            {
                return;
            }

            var direction = (collision.transform.position - transform.position).normalized;
            physicsObject.YVelocity += direction.y * _pushStrength;
            physicsObject.XVelocity += direction.x * _pushStrength;
        }
    }
}
