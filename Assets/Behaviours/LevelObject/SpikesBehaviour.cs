using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Data;

namespace Assets.Behaviours
{
    class SpikesBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D hitBy)
        {
            var player = hitBy.GetComponent<PlayerControllerBehaviour>();

            if (!player)
                return;

            player.KillPlayer();
        }
    }
}
