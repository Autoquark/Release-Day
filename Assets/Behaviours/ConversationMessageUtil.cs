using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Data;
using UnityEngine.UI;

namespace Assets.Behaviours
{
    class ConversationMessageUtil : MonoBehaviour
    {
        private Lazy<Text> _text;
        private Lazy<Text> _name;
        private Lazy<Image> _image;
        private Lazy<GameObject> _id;

        public UnityEngine.Sprite AlexFace;
        public UnityEngine.Sprite ColinFace;
        public UnityEngine.Sprite FrancesFace;
        public UnityEngine.Sprite HenryFace;
        public UnityEngine.Sprite LaylaFace;
        public UnityEngine.Sprite MyraFace;

        public ConversationMessageUtil()
        {
            _text = new Lazy<Text>(() => transform.Find("Text").GetComponent<Text>());
            _name = new Lazy<Text>(() => transform.Find("Id/Name").GetComponent<Text>());
            _image = new Lazy<Image>(() => transform.Find("Id/Image").GetComponent<Image>());
        }

        public void SetMessage(Conversation c)
        {
            _text.Value.text = c.Text;
            _name.Value.text = c.Speaker;
            switch(c.Speaker)
            {
                case "Alex":
                    _image.Value.sprite = AlexFace;
                    break;
                case "Colin":
                    _image.Value.sprite = ColinFace;
                    break;
                case "Frances":
                    _image.Value.sprite = FrancesFace;
                    break;
                case "Henry":
                    _image.Value.sprite = HenryFace;
                    break;
                case "Layla":
                    _image.Value.sprite = LaylaFace;
                    break;
                case "Myra":
                    _image.Value.sprite = MyraFace;
                    break;
            }
        }
    }
}
