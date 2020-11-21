using System;
using UnityEngine;
using Assets.Data;
using UnityEngine.UI;
using Assets.Extensions;

namespace Assets.Behaviours
{
    class ConversationMessageUtil : MonoBehaviour
    {
        private readonly Lazy<Text> _text;
        private readonly Lazy<Text> _name;
        private readonly Lazy<Image> _image;
        private readonly Lazy<GameObject> _id;
        private readonly Lazy<GameObject> _choices;
        private int _selectedOption = 0;
        private ConversationController _controller;

        public Sprite AlexFace;
        public Sprite ColinFace;
        public Sprite FrancesFace;
        public Sprite HenryFace;
        public Sprite LaylaFace;
        public Sprite MyraFace;

        public GameObject ChoicePrefab;

        void Update()
        {
            if (_choices.Value)
            {
                ShowSelection();
            }

            if (_controller)
            {
                if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && _selectedOption > 0)
                {
                    _selectedOption--;
                }
                else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && _selectedOption < _choices.Value.transform.childCount - 1)
                {
                    _selectedOption++;
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    _controller.SelectionMade(_selectedOption);
                    _controller = null;
                }
            }
        }

        public ConversationMessageUtil()
        {
            _text = new Lazy<Text>(() => transform.Find("Text").GetComponent<Text>());
            _name = new Lazy<Text>(() => transform.Find("Id/Name").GetComponent<Text>());
            _image = new Lazy<Image>(() => transform.Find("Id/Image").GetComponent<Image>());
            _choices = new Lazy<GameObject>(
                () =>
                {
                    var obj = transform.Find("Choices");
                    return obj ? obj.gameObject : null;
                }
            );
        }

        public void SetMessage(Conversation c, bool isRight)
        {
            _controller = null;

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

        internal void SetOptions(Conversation[] convs, bool isRight, ConversationController controller)
        {
            _controller = controller;

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
            }
        }

        private void ClearChoices()
        {
            _choices.Value.transform.DestroyChildren();

            _selectedOption = 0;
        }

        private void ShowSelection()
        {
            int count = 0;

            foreach(var child in _choices.Value.transform.Children())
            {
                child.transform.Find("Image").GetComponent<Image>().enabled = _selectedOption == count;
                count++;
            }
        }

    }
}
