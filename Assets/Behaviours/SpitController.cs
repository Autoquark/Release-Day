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
    class SpitController : MonoBehaviour
    {
        public float Speed = 7.0f;

        private void FixedUpdate()
        {
            Vector3 move = new Vector3(transform.localScale.x * Speed, 0, 0);

            transform.position += move * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerControllerBehaviour player = collision.GetComponent<PlayerControllerBehaviour>();
            if (player != null)
            {
                player.KillPlayer();
            }

            GameObject.Destroy(gameObject);
        }
    }
}
