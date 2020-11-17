using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InGameMenuBehavior : MonoBehaviour
{
    protected bool Visible { get; private set; } = false;
    protected bool PauseWhenOpen { get; set; } = true;

    private Lazy<GameObject> _panel;

    public InGameMenuBehavior()
    {
        _panel = new Lazy<GameObject>(() => transform.Find("Panel").gameObject);
    }

    protected void ToggleMenu()
    {
        Visible = !Visible;

        if (_panel.Value != null)
        {
            _panel.Value.SetActive(Visible);
        }

        if (PauseWhenOpen)
        {
            Time.timeScale = Visible ? 0 : 1;
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
