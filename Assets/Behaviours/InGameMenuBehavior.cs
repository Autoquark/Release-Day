using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Behaviours
{
    public class InGameMenuBehavior : MonoBehaviour
    {
        protected bool Visible { get; private set; } = false;
        protected bool PauseWhenOpen { get; set; } = true;

        private readonly Lazy<GameObject> _panel;
        private readonly Lazy<LevelControllerBehaviour> _levelController;

        public InGameMenuBehavior()
        {
            _panel = new Lazy<GameObject>(() => transform.Find("Panel").gameObject);
            _levelController = new Lazy<LevelControllerBehaviour>(() => GameObject.FindObjectOfType<LevelControllerBehaviour>());
        }

        protected void ToggleMenu()
        {
            Visible = !Visible;

            if (_panel.Value != null)
            {
                _panel.Value.SetActive(Visible);
            }

            if (PauseWhenOpen)
            {
                _levelController.Value.StopTime(gameObject, Visible);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }
        public void OnContinue()
        {
            ToggleMenu();
        }

        public void OnQuit()
        {
            Application.Quit();
        }
    }
}