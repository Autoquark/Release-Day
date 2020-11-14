using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InGameMenuBehavior : MonoBehaviour
{
    bool _visible = false;

    private Lazy<GameObject> _panel;

    public InGameMenuBehavior()
    {
        _panel = new Lazy<GameObject>(() => transform.Find("Panel").gameObject);
    }

    private void ToggleMenu()
    {
        _visible = !_visible;

        if (_panel.Value != null)
        {
            _panel.Value.SetActive(_visible);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }
    public void OnContinue()
    {
        ToggleMenu();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
