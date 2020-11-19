using Assets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class MenuRootBehaviour : MonoBehaviour
    {
        private readonly Lazy<GameObject> _inGameMenu;
        private readonly Lazy<GameObject> _optionsMenu;

        public MenuRootBehaviour()
        {
            _inGameMenu = new Lazy<GameObject>(() => transform.Find("In Game Menu").gameObject);
            _optionsMenu = new Lazy<GameObject>(() => transform.Find("OptionsMenu").gameObject);
        }

        public void SetActiveChild(GameObject gameObject)
        {
            if(gameObject != null && gameObject.transform.parent != transform)
            {
                throw new Exception();
            }

            foreach(var child in transform.Children())
            {
                child.gameObject.SetActive(false);
            }

            gameObject?.SetActive(true);
        }

        public void ShowInGameMenu() => SetActiveChild(_inGameMenu.Value);
        public void ShowOptionsMenu() => SetActiveChild(_optionsMenu.Value);

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(_inGameMenu.Value.activeSelf)
                {
                    SetActiveChild(null);
                }
                else if(_optionsMenu.Value.activeSelf)
                {
                    ShowInGameMenu();
                }
                else
                {
                    ShowInGameMenu();
                }
            }
        }
    }
}
