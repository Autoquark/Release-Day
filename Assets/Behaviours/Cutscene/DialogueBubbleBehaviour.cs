using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Behaviours.Cutscene
{
    class DialogueBubbleBehaviour : MonoBehaviour
    {
        private const float _remarkDisplayTime = 1;
        private const float _remarkFadeTime = 1;
        private const int _paddingWithPrompt = 15;
        private const int _paddingWithoutPrompt = 7;

        private readonly Lazy<Text> _text;
        private readonly Lazy<GameObject> _prompt;
        private readonly Lazy<CanvasGroup> _canvasGroup;
        private readonly Lazy<VerticalLayoutGroup> _wrapperLayout;
        private float _remarkShownAt = -999;
        private bool _showingRemark;

        public DialogueBubbleBehaviour()
        {
            _wrapperLayout = new Lazy<VerticalLayoutGroup>(() => transform.Find("Wrapper").GetComponent<VerticalLayoutGroup>());
            _text = new Lazy<Text>(() => transform.Find("Wrapper/MessageText").GetComponent<Text>());
            _prompt = new Lazy<GameObject>(() => transform.Find("Wrapper/Prompt").gameObject);
            _canvasGroup = new Lazy<CanvasGroup>(GetComponent<CanvasGroup>);
        }

        private void Start()
        {
            GetComponent<CanvasScaler>().dynamicPixelsPerUnit = 2;
        }

        public void ShowCutsceneDialogue(string text)
        {
            _showingRemark = false;
            _canvasGroup.Value.alpha = 1;

            _prompt.Value.SetActive(true);
            _wrapperLayout.Value.padding.right = _paddingWithPrompt;

            _text.Value.text = text;
            gameObject.SetActive(!string.IsNullOrEmpty(text));
        }

        public void ShowRemark(string text)
        {
            _showingRemark = true;
            _canvasGroup.Value.alpha = 1;

            _prompt.Value.SetActive(false);
            _wrapperLayout.Value.padding.right = _paddingWithoutPrompt;

            _text.Value.text = text;
            gameObject.SetActive(true);
            _remarkShownAt = Time.time;
        }

        private void Update()
        {
            if(!_showingRemark)
            {
                return;
            }

            if(Time.time > _remarkShownAt + _remarkDisplayTime + _remarkFadeTime)
            {
                gameObject.SetActive(false);
            }
            else if(Time.time > _remarkShownAt + _remarkDisplayTime)
            {
                _canvasGroup.Value.alpha = 1 - ((Time.time - (_remarkShownAt + _remarkDisplayTime)) / _remarkFadeTime);
            }
        }
    }
}
