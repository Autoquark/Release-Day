using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Behaviours
{
    class CreditsBehaviour : MonoBehaviour
    {
        private readonly Lazy<LevelControllerBehaviour> _levelController;

        public CreditsBehaviour()
        {
            _levelController = new Lazy<LevelControllerBehaviour>(FindObjectOfType<LevelControllerBehaviour>);
        }

        public void OnBack()
        {
            _levelController.Value.GoToLevel("MainMenu");
        }
    }
}
