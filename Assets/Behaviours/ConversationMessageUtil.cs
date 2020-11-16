﻿using System;
using UnityEngine;
using Assets.Data;
using UnityEngine.UI;
using Assets.Extensions;

namespace Assets.Behaviours
{
    class ConversationMessageUtil : MonoBehaviour
    {
        private Lazy<Text> _text;
        private Lazy<Text> _name;
        private Lazy<Image> _image;
        private Lazy<GameObject> _id;
        private Lazy<GameObject> _choices;

        public UnityEngine.Sprite AlexFace;
        public UnityEngine.Sprite ColinFace;
        public UnityEngine.Sprite FrancesFace;
        public UnityEngine.Sprite HenryFace;
        public UnityEngine.Sprite LaylaFace;
        public UnityEngine.Sprite MyraFace;

        public GameObject ChoicePrefab;

        public ConversationMessageUtil()
        {
            _text = new Lazy<Text>(() => transform.Find("Text").GetComponent<Text>());
            _name = new Lazy<Text>(() => transform.Find("Id/Name").GetComponent<Text>());
            _image = new Lazy<Image>(() => transform.Find("Id/Image").GetComponent<Image>());
            _choices = new Lazy<GameObject>(() => transform.Find("Choices").gameObject);
        }

        public void SetMessage(Conversation c, bool isRight)
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

            if (!isRight)
            {
                _image.Value.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        internal void SetOptions(Conversation[] convs, bool isRight)
        {
            ClearChoices();

            _name.Value.text = convs[0].Speaker;
            switch (convs[0].Speaker)
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

            if (!isRight)
            {
                _image.Value.transform.localScale = new Vector3(-1, 1, 1);
            }

            bool first = true;

            foreach(var choice in convs)
            {
                var msg = Instantiate(ChoicePrefab, _choices.Value.transform);
                msg.transform.localScale = new Vector3(1, 1, 1);
                Text text = msg.transform.Find("Text").GetComponent<Text>();
                text.text = choice.Text;
                if (choice.IsHint)
                {
                    text.color = Color.red;
                }
                msg.transform.Find("Image").GetComponent<Image>().color = first ? Color.white : Color.grey;
                first = false;
            }
        }

        private void ClearChoices()
        {
            _choices.Value.transform.DestroyChildren();
        }
    }
}
