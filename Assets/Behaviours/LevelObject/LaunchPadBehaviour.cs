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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var physics = collision.GetComponent<PhysicsObject>();
            if(physics == null)
            {
                return;
            }

            // Override any existing velocity, otherwise falling onto it results in a smaller jump
            physics.YVelocity = jumpPower;
        }
    }
}
