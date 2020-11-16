using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Behaviours
{
    class DialogueBubbleBehaviour : MonoBehaviour
    {
        private readonly Lazy<Text> _text;

        public DialogueBubbleBehaviour()
        {
            _text = new Lazy<Text>(() => transform.Find("MessageText").GetComponent<Text>());
        }

        private void Start()
        {
            GetComponent<CanvasScaler>().dynamicPixelsPerUnit = 2;
        }

        public void SetText(string text)
        {
            _text.Value.text = text;
            gameObject.SetActive(!string.IsNullOrEmpty(text));
        }
    }
}
