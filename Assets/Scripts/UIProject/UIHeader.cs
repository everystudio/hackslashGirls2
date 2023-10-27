using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;
using UnityEngine.UI;
using TMPro;

public class UIHeader : Singleton<UIHeader>
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI gemText;
    [SerializeField] TextMeshProUGUI ticketText;

    public override void Initialize()
    {
        base.Initialize();

        UpdateCoin(ModelManager.Instance.UserGameData.coin);
        UpdateGem(ModelManager.Instance.UserGameData.gem);
        UpdateTicket(ModelManager.Instance.UserGameData.ticket);
        ModelManager.Instance.OnChangeUserGameData.AddListener((userGameData) =>
        {
            UpdateCoin(userGameData.coin);
            UpdateGem(userGameData.gem);
            UpdateTicket(userGameData.ticket);
        });
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = Defines.GetNumericString(coin);
    }

    public void UpdateGem(int gem)
    {
        gemText.text = Defines.GetNumericString(gem);
    }

    public void UpdateTicket(int ticket)
    {
        ticketText.text = Defines.GetNumericString(ticket);
    }

    private void OnDestroy()
    {



    }

}
