using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Behaviours
{
    class LevelControllerBehaviour : MonoBehaviour
    {

        public static int test;
        public List<string> LevelSequence = new List<string>();

        private const float _fadeDuration = 1;

        private float _time_out = 0.0f;
        private HashSet<GameObject> _timeStoppers = new HashSet<GameObject>();
        private Lazy<Image> _blackoutImage;
        private bool _loadingLevel = false;

        public LevelControllerBehaviour()
        {
            _blackoutImage = new Lazy<Image>(() => transform.Find("../MenuRoot/Blackout").GetComponent<Image>());
        }

        private void Start()
        {
            StartCoroutine(LevelStartCoroutine());
        }

        private IEnumerator LevelStartCoroutine()
        {
            foreach (var player in PlayerControllerBehaviour.AllPlayers())
            {
                player.enabled = false;
            }
            yield return StartCoroutine(FadeBetween(Color.black, Color.clear));
            foreach (var player in PlayerControllerBehaviour.AllPlayers())
            {
                player.enabled = true;
            }
        }

        public void GoToNextLevel()
        {
            var current = SceneManager.GetActiveScene().name;
            StartCoroutine(GoToLevelCoroutine(LevelSequence.SkipWhile(x => x != current).Skip(1).First()));
        }

        private IEnumerator GoToLevelCoroutine(string sceneName)
        {
            _loadingLevel = true;
            var operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            foreach(var player in PlayerControllerBehaviour.AllPlayers())
            {
                player.enabled = false;
            }
            yield return StartCoroutine(FadeBetween(Color.clear, Color.black));
            // 0.9 progress indicates that it is finished but being paused because allowSceneActivation is false
            yield return new WaitUntil(() => operation.progress >= 0.9f);
            operation.allowSceneActivation = true;
        }

        private IEnumerator FadeBetween(Color a, Color b)
        {
            var startTime = Time.unscaledTime;
            var endTime = Time.unscaledTime + _fadeDuration;
            while (Time.unscaledTime < endTime)
            {
                _blackoutImage.Value.color = Color.Lerp(a, b, (Time.unscaledTime - startTime) / _fadeDuration);
                yield return null;
            }
            _blackoutImage.Value.color = b;
        }

        private void Update()
        {
            if (!_loadingLevel && _time_out != 0 && Time.time > _time_out)
            {
                StartCoroutine(GoToLevelCoroutine(SceneManager.GetActiveScene().name));
            }

            if (_time_out == 0 && !FindObjectsOfType<PlayerControllerBehaviour>().Any())
            {
                _time_out = Time.time + 1.0f;
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
