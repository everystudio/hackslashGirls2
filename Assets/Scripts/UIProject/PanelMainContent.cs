using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using anogame;

public class PanelMainContent : UIPanel
{
    [SerializeField] private Transform contentRoot;
    [SerializeField] private GameObject areaPrefab;
    [SerializeField] private GameObject partyPrefab;

    [SerializeField] private Button tabArea;
    [SerializeField] private Button tabParty;

    [SerializeField] private Button closeButton;

    private GameObject currentContent;
    public UnityEvent OnBackButtonClicked = new UnityEvent();

    private static bool lastViewContentIsParty = false;

    protected override void initialize()
    {
        closeButton.onClick.AddListener(() =>
        {
            OnBackButtonClicked.Invoke();
            //UIController.Instance.RemovePanel(this.gameObject);
        });
    }

    protected override void shutdown()
    {
        base.shutdown();
        UIController.Instance.RemovePanel(currentContent);
    }

    public void Build(bool isQuest)
    {
        if (lastViewContentIsParty == false)
        {
            currentContent = UIController.Instance.AddPanel(areaPrefab, contentRoot);
        }
        else
        {
            currentContent = UIController.Instance.AddPanel(partyPrefab, contentRoot);
            if (currentContent.TryGetComponent(out ListParty listParty))
            {
                listParty.isQuest = isQuest;
                listParty.Initialize();
            }
        }

        if (tabArea != null)
        {
            tabArea.onClick.AddListener(() =>
            {
                lastViewContentIsParty = false;
                UIController.Instance.RemovePanel(currentContent);
                currentContent = UIController.Instance.AddPanel(areaPrefab, contentRoot);
            });
        }
        if (tabParty != null)
        {
            tabParty.onClick.AddListener(() =>
            {
                lastViewContentIsParty = true;

                UIController.Instance.RemovePanel(currentContent);
                currentContent = UIController.Instance.AddPanel(partyPrefab, contentRoot);
                if (currentContent.TryGetComponent(out ListParty listParty))
                {
                    listParty.isQuest = isQuest;
                    listParty.Initialize();
                }
            });
        }
    }
}
