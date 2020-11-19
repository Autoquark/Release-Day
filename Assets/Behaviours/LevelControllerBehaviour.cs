﻿using System;
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
        private HashSet<GameObject> _timeStoppers = new HashSet<GameObject>();

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

        public void StopTime(GameObject obj, bool stop)
        {
            if (stop)
            {
                _timeStoppers.Add(obj);

                Time.timeScale = 0;
            }
            else
            {
                _timeStoppers.Remove(obj);

                if (!IsTimeStopped)
                {
                    Time.timeScale = 1;
                }
            }
        }

        public bool IsTimeStopped => _timeStoppers.Count > 0;
    }
}
