using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;
using UnityEngine.UI;
using TMPro;

public class UIHeader : Singleton<UIHeader>
{
    [SerializeField] TextMeshProUGUI coinText;

    public override void Initialize()
    {
        base.Initialize();

        UpdateCoin(ModelManager.Instance.UserGameData.coin);
        ModelManager.Instance.OnChangeUserGameData.AddListener((userGameData) =>
        {
            UpdateCoin(userGameData.coin);
        });
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = Defines.GetNumericString(coin);
    }

    private void OnDestroy()
    {



    }

}
