using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class ConversationController : MonoBehaviour
    {
        bool _visible = false;
        private Lazy<Canvas> _canvas;

        private void Start()
        {
            _canvas.Value.enabled = false;
        }

        public ConversationController()
        {
            _canvas = new Lazy<Canvas>(GetComponent<Canvas>);
        }

        public void ToggleVisibility()
        {
            _visible = !_visible;

            _canvas.Value.enabled = _visible;
        }
    }
}
