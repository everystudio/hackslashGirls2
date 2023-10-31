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

    public UnityEvent<int, bool> OnFloorStart = new UnityEvent<int, bool>();
    public UnityEvent<int> OnAreaStartCollect = new UnityEvent<int>();


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

    private void BuildArea(bool isQuest)
    {
        lastViewContentIsParty = false;
        if (currentContent != null)
        {
            UIController.Instance.RemovePanel(currentContent);
        }

        currentContent = UIController.Instance.AddPanel(areaPrefab, contentRoot);
        if (currentContent.TryGetComponent(out ListArea listArea))
        {
            listArea.Init(isQuest);
            listArea.OnFloorStart.AddListener((floorId, isQuest) =>
            {
                OnFloorStart.Invoke(floorId, isQuest);
            });
            listArea.OnAreaStartCollect.AddListener((areaId) =>
            {
                OnAreaStartCollect.Invoke(areaId);
            });
        }
    }

    private void BuildParty(bool isQuest)
    {
        lastViewContentIsParty = true;

        if (currentContent != null)
        {
            UIController.Instance.RemovePanel(currentContent);
        }

        currentContent = UIController.Instance.AddPanel(partyPrefab, contentRoot);
        if (currentContent.TryGetComponent(out ListParty listParty))
        {
            listParty.isQuest = isQuest;
            listParty.Initialize();
        }
    }

    public void Build(bool isQuest)
    {
        if (lastViewContentIsParty == false)
        {
            BuildArea(isQuest);
        }
        else
        {
            BuildParty(isQuest);
        }

        if (tabArea != null)
        {
            tabArea.onClick.AddListener(() =>
            {
                BuildArea(isQuest);
            });
        }
        if (tabParty != null)
        {
            tabParty.onClick.AddListener(() =>
            {
                BuildParty(isQuest);
            });
        }
    }
}
