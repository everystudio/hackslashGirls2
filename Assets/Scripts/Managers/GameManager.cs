using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class GameManager : SingletonStateMachineBase<GameManager>
{
    private string currentFooterButton = "Main";
    private GameObject currentPanel;

    [SerializeField] private UIClickHandle questView;
    [SerializeField] private UIClickHandle collectView;

    [SerializeField] private List<UICharacterCore> charaPartyList;

    public override void Initialize()
    {
        base.Initialize();
        FooterButtons.OnFooterButtonEvent.AddListener(OnFooterButtonEvent);

        for (int i = 0; i < charaPartyList.Count; i++)
        {
            var partyId = i + 1;
            var userChara = ModelManager.Instance.GetQuestPartyChara(partyId);
            charaPartyList[i].Set(userChara);
        }

        UserChara.OnChanged.AddListener((userChara) =>
        {
            if (0 < userChara.questPartyId)
            {
                charaPartyList[userChara.questPartyId - 1].Set(userChara);
            }
        });

        ChangeState(new GameManager.Idle(this));
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

    private class Idle : StateBase<GameManager>
    {
        public Idle(GameManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();

            machine.questView.OnClicked.AddListener(() =>
            {
                machine.ChangeState(new GameManager.Quest(machine));
            });
            machine.collectView.OnClicked.AddListener(() =>
            {
                machine.ChangeState(new GameManager.Collect(machine));
            });
        }
        public override void OnExitState()
        {
            base.OnExitState();
            machine.questView.OnClicked.RemoveAllListeners();
            machine.collectView.OnClicked.RemoveAllListeners();
        }
    }

    private class Quest : StateBase<GameManager>
    {
        public PanelMainContent panelMainContent;
        public Quest(GameManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            panelMainContent = UIController.Instance.AddPanel("PanelMainContent").GetComponent<PanelMainContent>();
            panelMainContent.BuildQuest();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            UIController.Instance.RemovePanel(panelMainContent.gameObject);
        }
    }

    private class Collect : StateBase<GameManager>
    {
        public Collect(GameManager machine) : base(machine)
        {
        }
    }
}
