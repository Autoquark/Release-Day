using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Behaviours
{
    class LevelControllerBehaviour : MonoBehaviour
    {
        private float _time_out = 0.0f;

        private void Update()
        {
            if (_time_out != 0 && Time.time > _time_out)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (_time_out == 0 && !FindObjectsOfType<PlayerControllerBehaviour>().Any())
            {
                _time_out = Time.time + 2.0f;
            }
        }
    }
}
