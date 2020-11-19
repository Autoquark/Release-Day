using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Behaviours;
using System.Linq;

public class InGameMenuBehavior : MonoBehaviour
{
    protected bool Visible { get; private set; } = false;
    protected bool PauseWhenOpen { get; set; } = true;

    private readonly Lazy<GameObject> _panel;
    private readonly Lazy<GameObject> _optionsMenu;

    public InGameMenuBehavior()
    {
        _panel = new Lazy<GameObject>(() => transform.Find("Panel").gameObject);
        _optionsMenu = new Lazy<GameObject>(() => transform.Find("../OptionsMenu").gameObject);
    }

    public void ToggleMenu()
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

    public void OnOptions()
    {
        _optionsMenu.Value.SetActive(true);
        ToggleMenu();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
