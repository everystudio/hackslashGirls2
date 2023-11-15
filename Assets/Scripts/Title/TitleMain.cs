using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using anogame;
using PlayFab;

using TMPro;

public class TitleMain : SingletonStateMachineBase<TitleMain>
{
    [SerializeField] private TextMeshProUGUI infoText;
    public override void Initialize()
    {
        base.Initialize();
        ChangeState(new TitleMain.Login(this));
    }

    private class Login : StateBase<TitleMain>
    {
        public Login(TitleMain machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            PlayFabAuthService.OnLoginSuccess += OnLoginSuccess;
            PlayFabAuthService.OnPlayFabError += OnPlayFabError;
            PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
        }

        private void OnLoginSuccess(LoginResult success)
        {
            ChangeState(new TitleMain.Wait(machine));
        }
        private void OnPlayFabError(PlayFabError error)
        {
            ChangeState(new TitleMain.Error(machine));
        }

        public override void OnExitState()
        {
            base.OnExitState();
            PlayFabAuthService.OnLoginSuccess -= OnLoginSuccess;
            PlayFabAuthService.OnPlayFabError -= OnPlayFabError;
        }

    }

    private class Error : StateBase<TitleMain>
    {
        public Error(TitleMain machine) : base(machine)
        {
        }
    }

    private class Wait : StateBase<TitleMain>
    {
        public Wait(TitleMain machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            machine.infoText.text = "PAP SCREEN";
        }

        public override void OnUpdateState()
        {
            base.OnUpdateState();
            if (Input.GetMouseButtonDown(0))
            {
                ChangeState(new TitleMain.GameStart(machine));
            }
        }
    }

    private class GameStart : StateBase<TitleMain>
    {
        public GameStart(TitleMain machine) : base(machine)
        {
        }

        public override void OnEnterState()
        {
            Application.targetFrameRate = 60;
            base.OnEnterState();
            SceneManager.LoadScene("MainScene");
        }
    }
}
