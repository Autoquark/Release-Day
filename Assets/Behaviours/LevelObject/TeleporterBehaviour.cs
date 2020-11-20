using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Data;
using Spine.Unity;

namespace Assets.Behaviours
{
    class TeleporterBehaviour : AnimationBase
    {
        public AnimationReferenceAsset activateAnimation;
        public bool HasBug = false;
        public TeleporterBehaviour SendsTo;

        private float _coolDownEnd = 0.0f;
        protected ContactFilter2D Filter;

        public void Start()
        {
            Filter.useTriggers = false;
            Filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        }

        private void OnTriggerStay2D(Collider2D hitBy)
        {
            if (_coolDownEnd > Time.time)
                return;

            PlayerControllerBehaviour player = hitBy.GetComponent<PlayerControllerBehaviour>();

            if (SendsTo != null && player != null)
            {
                CapsuleCollider2D capsule = player.GetComponent<CapsuleCollider2D>();

                if (Physics2D.OverlapCapsule(SendsTo.transform.position, capsule.size, capsule.direction, capsule.transform.eulerAngles.z, Filter, new Collider2D[1]) == 0)
                {
                    if (!HasBug)
                    {
                        player.GetComponent<Rigidbody2D>().position = SendsTo.transform.position;
                    }
                    else
                    {
                        Instantiate(player.gameObject, SendsTo.transform.position, SendsTo.transform.rotation);
                    }

                    _coolDownEnd = SendsTo._coolDownEnd = Time.time + 1.5f;
                    SetAnimationIfDifferent(activateAnimation, false);
                }
            }
        }
    }
}
