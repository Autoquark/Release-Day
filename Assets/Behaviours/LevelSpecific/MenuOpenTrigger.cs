using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours.LevelSpecific
{
    class MenuOpenTrigger : MonoBehaviour
    {
        public bool _open = true;

        private Lazy<MenuRootBehaviour> _menuRoot;

        public MenuOpenTrigger()
        {
            _menuRoot = new Lazy<MenuRootBehaviour>(() => FindObjectOfType<MenuRootBehaviour>());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!collision.GetComponent<PlayerControllerBehaviour>())
            {
                return;
            }

            _menuRoot.Value.ShowInGameMenu();
            Destroy(gameObject);
        }
    }
}
