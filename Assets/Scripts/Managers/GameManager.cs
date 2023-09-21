using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class GameManager : Singleton<GameManager>
{
    private string currentFooterButton = "Main";
    private GameObject currentPanel;

    public override void Initialize()
    {
        base.Initialize();
        FooterButtons.OnFooterButtonEvent.AddListener(OnFooterButtonEvent);
    }

    private void OnFooterButtonEvent(string arg0)
    {
        if (currentFooterButton == arg0)
        {
            return;
        }
        currentFooterButton = arg0;
        if (currentPanel != null)
        {
            UIController.Instance.RemovePanel(currentPanel);
        }
        currentPanel = UIController.Instance.AddPanel("Panel" + arg0 + "Top");
    }
}
