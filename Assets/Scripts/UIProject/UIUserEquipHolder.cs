using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;
using UnityEngine.UI;

public class UIUserEquipHolder : MonoBehaviour
{
    private MasterChara masterChara;
    private UserChara userChara;

    // 原則３つ
    [SerializeField] private EquipmentButton[] equipmentButtons;

    public void Set(MasterChara masterChara, UserChara userChara)
    {
        this.masterChara = masterChara;
        this.userChara = userChara;

        List<MasterEquip> equipItems = ModelManager.Instance.MasterEquip.List.FindAll(equip => equip.chara_id == masterChara.chara_id && equip.rank == userChara.rank);

        int index = 0;
        foreach (var equipItem in equipItems)
        {
            int item_id = equipItem.item_id;
            bool equiping = userChara.IsEquiping(item_id);

            MasterItem masterItem = ModelManager.Instance.GetMasterItem(item_id);
            UserItem userItem = ModelManager.Instance.GetUserItem(item_id);
            equipmentButtons[index].Initialize(equiping, equipItem, masterItem, userItem);
            index += 1;
        }
    }

}
