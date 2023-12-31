using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;
using UnityEngine.UI;
using TMPro;

public class UIUserEquipHolder : MonoBehaviour
{
    private MasterChara masterChara;
    private UserChara userChara;
    [SerializeField] private Button rankUpButton;
    [SerializeField] private TextMeshProUGUI rankUpGemText;


    // 原則３つ
    [SerializeField] private EquipmentButton[] equipmentButtons;

    public void Set(MasterChara masterChara, UserChara userChara)
    {
        //Debug.Log(masterChara.chara_name + " 現在のランク:" + userChara.rank);
        this.masterChara = masterChara;
        this.userChara = userChara;

        List<MasterEquip> equipItems = ModelManager.Instance.MasterEquip.List.FindAll(equip => equip.chara_id == masterChara.chara_id && equip.rank == userChara.rank);

        int index = 0;
        int equipingCount = 0;
        foreach (var equipItem in equipItems)
        {
            int item_id = equipItem.item_id;
            bool equiping = userChara.IsEquiping(item_id);
            if (equiping)
            {
                equipingCount += 1;
            }

            MasterItem masterItem = ModelManager.Instance.GetMasterItem(item_id);
            UserItem userItem = ModelManager.Instance.GetUserItem(item_id);
            equipmentButtons[index].Initialize(equiping, equipItem, masterItem, userItem);
            if (equipmentButtons[index].TryGetComponent(out Button button))
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    if (!equiping)
                    {
                        if (ModelManager.Instance.EquipItem(masterChara.chara_id, item_id))
                        {
                            Set(masterChara, userChara);
                        }
                    }
                });
            }
            index += 1;
        }

        int rankUpGem = Defines.GetRankUpGem(userChara.rank + 1);
        rankUpButton.interactable = false;
        if (5 <= userChara.rank)
        {
            rankUpGemText.text = "MAX";
        }
        else if (equipingCount < 3)
        {
            rankUpGemText.text = "装備が足りません";
        }
        else if (rankUpGem > ModelManager.Instance.UserGameData.gem)
        {
            rankUpGemText.text = $"ジェム不足({rankUpGem})";
        }
        else
        {
            rankUpGemText.text = $"ランクアップ({rankUpGem}ジェム)";
            rankUpButton.interactable = true;
            rankUpButton.onClick.RemoveAllListeners();
            rankUpButton.onClick.AddListener(() =>
            {
                ModelManager.Instance.UseGem(rankUpGem);
                userChara.Rankup();
                Set(masterChara, userChara);
            });
        }
    }

}
