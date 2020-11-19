using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class OptionsMenuBehaviour : MonoBehaviour
    {
        private readonly Lazy<InGameMenuBehavior> _inGameMenu;

        public OptionsMenuBehaviour()
        {
            _inGameMenu = new Lazy<InGameMenuBehavior>(() => transform.Find("../In Game Menu").GetComponent<InGameMenuBehavior>());
        }

        public void OnBack()
        {
            gameObject.SetActive(false);
            _inGameMenu.Value.ToggleMenu();
        }
    }
}
