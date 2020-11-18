using Assets.Behaviours.Cutscene;
using Assets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class RemarkTriggerBehaviour : MonoBehaviour
    {
        public string _text;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!collision.HasComponent<PlayerControllerBehaviour>())
            {
                return;
            }

            collision.GetComponent<TalkBehaviour>().Remark(_text);
            Destroy(gameObject);
        }
    }
}
