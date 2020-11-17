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

        private Lazy<BuggyInGameMenuBehaviour> _inGameMenu;

        public MenuOpenTrigger()
        {
            _inGameMenu = new Lazy<BuggyInGameMenuBehaviour>(() => FindObjectOfType<BuggyInGameMenuBehaviour>());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!collision.GetComponent<PlayerControllerBehaviour>())
            {
                return;
            }

            _inGameMenu.Value.SetMenuVisible(_open);
        }
    }
}
