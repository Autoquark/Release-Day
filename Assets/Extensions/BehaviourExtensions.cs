using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Extensions
{
    static class BehaviourExtensions
    {
        public static PlayerControllerBehaviour FirstPlayer(this MonoBehaviour unused)
        {
            return PlayerControllerBehaviour.FirstPlayer();
        }
        public static IEnumerable<PlayerControllerBehaviour> AllPlayers(this MonoBehaviour unused)
        {
            return PlayerControllerBehaviour.AllPlayers();
        }
    }
}
