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

    [SerializeField] private List<GameObject> collectRootList;

    [SerializeField] private float gameSpeed = 1f;
    private float savedGameSpeed = 1f;
    public float GameSpeed
    {
        get { return gameSpeed; }
        set
        {
            gameSpeed = value;
            Time.timeScale = gameSpeed;
        }
    }
    private void GamePause()
    {
        savedGameSpeed = GameSpeed;
        Time.timeScale = 0f;
    }
    private void GameResume()
    {
        Time.timeScale = savedGameSpeed;
    }

    public override void Initialize()
    {
        /*
        int total = 0;
        for (int i = 1; i < 100; i++)
        {
            total += Defines.CalculateRequiredLevelupCoin(i);
            Debug.Log("Level:" + i + " RequiredExperience:" + Defines.CalculateRequiredLevelupCoin(i));
        }
        Debug.Log("Total:" + total);
        */

        FloorManager.OnUpdateMaxFloor.AddListener((maxFloor) =>
        {
            // 採取メンバー解禁処理
            if (maxFloor == Defines.CollectStartFloorID)
            {
                for (int i = 0; i < 4 + 2; i++)
                {
                    UserChara addChara = ModelManager.Instance.AddChara(i + 5);
                    if (i < 4)
                    {
                        // 入れ替え用に2人メンバーを追加
                        addChara.collectPartyId = i + 1;
                    }
                }
                foreach (GameObject root in collectRootList)
                {
                    root.SetActive(true);
                }
            }
            // 実際の更新はここ
            ModelManager.Instance.UserGameData.max_floor_id = maxFloor;
        });

        foreach (GameObject root in collectRootList)
        {
            root.SetActive(2 <= ModelManager.Instance.UserGameData.max_floor_id);
        }


        //Debug.Log("GameManager Initialize");
        base.Initialize();
        FooterButtons.OnFooterButtonEvent.AddListener(OnFooterButtonEvent);

        GameSpeed = 3f;

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
    private class MainContentsView : StateBase<GameManager>
    {
        public PanelMainContent panelMainContent;
        public MainContentsView(GameManager machine) : base(machine)
        {
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            panelMainContent = UIController.Instance.AddPanel("PanelMainContent").GetComponent<PanelMainContent>();
            FooterButtons.OnFooterButtonEvent.AddListener(OnFooterButtonEventQuest);

            panelMainContent.OnBackButtonClicked.AddListener(() =>
            {
                ReturnIdle();
            });
        }
        private void ReturnIdle()
        {
            machine.ChangeState(new GameManager.Idle(machine));
        }

        private void OnFooterButtonEventQuest(string arg0)
        {
            ReturnIdle();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            UIController.Instance.RemovePanel(panelMainContent.gameObject);
        }

    }



    private class Quest : MainContentsView
    {
        public Quest(GameManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            panelMainContent.Build(true);

        }

    }

    private class Collect : MainContentsView
    {
        public Collect(GameManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            panelMainContent.Build(false);
        }
    }
}
