using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Behaviours
{
    class MainMenuBehaviour : MonoBehaviour
    {
        private readonly Lazy<LevelControllerBehaviour> _levelController;
        private readonly Lazy<GameObject> _optionsMenu;
        private readonly Lazy<GameObject> _credits;

        public MainMenuBehaviour()
        {
            _levelController = new Lazy<LevelControllerBehaviour>(FindObjectOfType<LevelControllerBehaviour>);
            _optionsMenu = new Lazy<GameObject>(() => SceneManager.GetActiveScene().GetRootGameObjects().First(x => x.name == "OptionsMenu"));
            //_optionsMenu = new Lazy<GameObject>(() => transform.Find("OptionsMenu").gameObject);
            _credits = new Lazy<GameObject>(() => SceneManager.GetActiveScene().GetRootGameObjects().First(x => x.name == "Credits"));
            //_credits = new Lazy<GameObject>(() => transform.Find("Credits").gameObject);
        }

        public void OnPlay()
        {
            _levelController.Value.GoToNextLevel();
        }

        public void OnOptions()
        {
            _optionsMenu.Value.SetActive(true);
        }

        public void OnCredits()
        {
            _credits.Value.SetActive(true);
        }

        public void OnQuit()
        {
            Application.Quit();
        }
    }
}
