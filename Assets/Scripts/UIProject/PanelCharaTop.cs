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


    protected override void initialize()
    {
        base.initialize();
        Debug.Log(selectingCharaId);

        ModelManager.Instance.OnUserCharaChanged.AddListener((userChara) =>
        {
            if (userChara.chara_id == selectingCharaId)
            {
                ShowSelectingChara(selectingCharaId);
            }
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
            Debug.Log(charaId);

            GameObject charaButton = Instantiate(charaButtonPrefab, charaListParent);
            UICharacterCore charaButtonCore = charaButton.GetComponent<UICharacterCore>();
            charaButtonCore.Set(masterChara, userChara);
            charaButtonCore.OnClick.AddListener((chara) =>
            {
                selectingCharaId = chara.chara_id;
                ShowSelectingChara(selectingCharaId);
            });
        }
    }

    private void ShowSelectingChara(int selectingCharaId)
    {
        MasterChara masterChara = ModelManager.Instance.GetMasterChara(selectingCharaId);
        UserChara userChara = ModelManager.Instance.GetUserChara(selectingCharaId);
        characterCore.Set(masterChara, userChara);

        userEquipHolder.Set(masterChara, userChara);
    }
}
