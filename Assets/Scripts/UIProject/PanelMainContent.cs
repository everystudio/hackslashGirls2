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

    private GameObject currentContent;

    public void BuildQuest()
    {
        currentContent = UIController.Instance.AddPanel(areaPrefab, contentRoot);

        if (tabArea != null)
        {
            tabArea.onClick.AddListener(() =>
            {
                UIController.Instance.RemovePanel(currentContent);
                currentContent = UIController.Instance.AddPanel(areaPrefab, contentRoot);
            });
        }
        if (tabParty != null)
        {
            tabParty.onClick.AddListener(() =>
            {
                Debug.Log("Party");
                UIController.Instance.RemovePanel(currentContent);
                currentContent = UIController.Instance.AddPanel(partyPrefab, contentRoot);
                if (currentContent.TryGetComponent(out ListParty listParty))
                {
                    listParty.isQuest = true;
                    listParty.Initialize();
                }
            });
        }
    }
}
