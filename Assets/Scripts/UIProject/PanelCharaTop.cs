using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;
using TMPro;
using UnityEngine.UI;
using System;

public class PanelCharaTop : UIPanel
{
    // ページ切り替えたりしても保存しておく
    public static int selectingCharaId = 1;

    [SerializeField] private UICharacterCore characterCore;
    [SerializeField] private UIUserEquipHolder userEquipHolder;

    [SerializeField] private Transform charaListParent;
    [SerializeField] private GameObject charaButtonPrefab;

    [SerializeField] private Button levelupButton;
    [SerializeField] private TextMeshProUGUI requireCoinText;

    [SerializeField] private Button starupButton;
    [SerializeField] private PanelStarUp panelStarUp;


    protected override void initialize()
    {
        base.initialize();
        //Debug.Log(selectingCharaId);

        panelStarUp.gameObject.SetActive(false);

        ModelManager.Instance.OnUserCharaChanged.AddListener((userChara) =>
        {
            if (userChara.chara_id == selectingCharaId)
            {
                ShowSelectingChara(selectingCharaId);
            }
        });

        ModelManager.Instance.OnChangeUserGameData.AddListener((gameData) =>
        {
            RefreshLevelupButton(selectingCharaId);
        });

        ShowSelectingChara(selectingCharaId);

        // charaListParentの子要素を全て削除
        foreach (Transform child in charaListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var userChara in ModelManager.Instance.UserChara.List)
        {
            var masterChara = ModelManager.Instance.GetMasterChara(userChara.chara_id);
            int charaId = masterChara.chara_id;
            //Debug.Log(charaId);

            GameObject charaButton = Instantiate(charaButtonPrefab, charaListParent);
            UICharacterCore charaButtonCore = charaButton.GetComponent<UICharacterCore>();
            charaButtonCore.Set(masterChara, userChara);
            charaButtonCore.OnClick.AddListener((chara) =>
            {
                selectingCharaId = chara.chara_id;
                ShowSelectingChara(selectingCharaId);
            });
        }

        levelupButton.onClick.AddListener(() =>
        {
            UserChara userChara = ModelManager.Instance.GetUserChara(selectingCharaId);

            int requireCoint = GetRequireCoin(userChara.level + 1, userChara.exp);
            if (ModelManager.Instance.UseCoin(requireCoint))
            {
                if (userChara.Levelup())
                {
                    ModelManager.Instance.OnUserCharaChanged.Invoke(userChara);
                }
            }
        });

        starupButton.onClick.AddListener(() =>
        {
            panelStarUp.gameObject.SetActive(true);
            panelStarUp.Redraw(ModelManager.Instance.GetUserChara(selectingCharaId), ModelManager.Instance.GetMasterChara(selectingCharaId));
        });

        panelStarUp.OnStarUp.AddListener(() =>
        {
            ShowSelectingChara(selectingCharaId);
        });

        ModelManager.Instance.OnUserItemChanged.AddListener(() =>
        {
            ShowSelectingChara(selectingCharaId);
        });
    }

    private int GetRequireCoin(int level, int exp)
    {
        int requireCoin = Defines.CalculateRequiredLevelupCoin(level);
        requireCoin = Mathf.Max(0, requireCoin - exp);
        return requireCoin;
    }

    private void ShowSelectingChara(int selectingCharaId)
    {
        MasterChara masterChara = ModelManager.Instance.GetMasterChara(selectingCharaId);
        UserChara userChara = ModelManager.Instance.GetUserChara(selectingCharaId);

        characterCore.Set(masterChara, userChara);
        userEquipHolder.Set(masterChara, userChara);


        starupButton.gameObject.SetActive(userChara.star < 5);
    }

    private void RefreshLevelupButton(int selectingCharaId)
    {
        UserChara userChara = ModelManager.Instance.GetUserChara(selectingCharaId);
        int requireCoin = GetRequireCoin(userChara.level + 1, userChara.exp);

        requireCoinText.text = Defines.GetNumericString(requireCoin);
        levelupButton.interactable = requireCoin <= ModelManager.Instance.UserGameData.coin;

    }



}
