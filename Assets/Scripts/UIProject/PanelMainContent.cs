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
        currentContent = Instantiate(areaPrefab, contentRoot);

        if (tabArea != null)
        {
            tabArea.onClick.AddListener(() =>
            {
                Destroy(currentContent);
                currentContent = Instantiate(areaPrefab, contentRoot);
            });
        }
        if (tabParty != null)
        {
            tabParty.onClick.AddListener(() =>
            {
                Destroy(currentContent);
                currentContent = Instantiate(partyPrefab, contentRoot);
            });
        }
    }
}
