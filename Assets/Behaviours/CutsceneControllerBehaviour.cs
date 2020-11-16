using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class CutsceneControllerBehaviour : MonoBehaviour
    {
        private readonly Lazy<PlayerControllerBehaviour> _playerControllerBehaviour;

        public CutsceneControllerBehaviour()
        {
            _playerControllerBehaviour = new Lazy<PlayerControllerBehaviour>(() => FindObjectOfType<PlayerControllerBehaviour>());
        }

        void Start()
        {
            StartCoroutine(CutsceneCoroutine());
        }

        IEnumerator CutsceneCoroutine()
        {
            var player = FindObjectOfType<PlayerControllerBehaviour>();
            player.enabled = false;
            var playerPhysics = player.GetComponent<PhysicsObject>();

            var layla = GameObject.Find("Layla");
            yield return WalkToX(playerPhysics, layla.transform.position.x + layla.GetComponent<SpriteRenderer>().sprite.bounds.min.x, player.walkSpeed);
        }

        IEnumerator WalkToX(PhysicsObject obj, float x, float speed)
        {
            while (Mathf.Abs(obj.transform.position.x - x) > 0.01f)
            {
                obj.WalkIntent = Mathf.Clamp(x - obj.transform.position.x,
                    -speed,
                    speed);
                yield return null;
            }
            obj.WalkIntent = 0;
        }
    }
}
