using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Behaviours;
using System.Linq;

namespace Assets.Behaviours
{
    public class InGameMenuBehavior : MonoBehaviour
    {
        protected bool PauseWhenOpen { get; set; } = true;

        private readonly Lazy<GameObject> _panel;
        private readonly Lazy<LevelControllerBehaviour> _levelController;
        private readonly Lazy<MenuRootBehaviour> _menuRoot;
        private readonly Lazy<GameObject> _confirmQuitRoot;

        public InGameMenuBehavior()
        {
            _panel = new Lazy<GameObject>(() => transform.Find("Panel").gameObject);
            _menuRoot = new Lazy<MenuRootBehaviour>(() => GetComponentInParent<MenuRootBehaviour>());
            _levelController = new Lazy<LevelControllerBehaviour>(() => FindObjectOfType<LevelControllerBehaviour>());
            _confirmQuitRoot = new Lazy<GameObject>(() => transform.Find("Mask").gameObject);
        }

        public void OnEnable()
        {
            if (PauseWhenOpen)
            {
                _levelController.Value.StopTime(gameObject, true);
            }
        }

        private void OnDisable()
        {
            _levelController.Value.StopTime(gameObject, false);
        }

        private void OnDestroy()
        {
            _levelController.Value.StopTime(gameObject, false);
        }

        public void OnContinue()
        {
            gameObject.SetActive(false);
        }

        public void OnOptions()
        {
            _menuRoot.Value.ShowOptionsMenu();
        }

        public void OnRestart()
        {
            _levelController.Value.RestartLevel();
        }

        public void OnQuit()
        {
            _confirmQuitRoot.Value.SetActive(true);
        }

        public void OnConfirmQuit()
        {
            Application.Quit();
        }

        public void OnCancelQuit()
        {
            _confirmQuitRoot.Value.SetActive(false);
        }
    }
}