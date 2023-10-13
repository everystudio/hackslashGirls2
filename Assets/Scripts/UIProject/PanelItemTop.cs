using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;
using TMPro;

public class PanelItemTop : UIPanel
{
    [SerializeField] private GameObject itemIconPrefab;
    [SerializeField] private Transform itemIconParent;

    [SerializeField] private ItemIcon selectingItemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemAbility;

    protected override void initialize()
    {
        base.initialize();

        // itemIconParentの子供を全て削除
        foreach (Transform child in itemIconParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var userItem in ModelManager.Instance.UserItem.List)
        {
            var itemIcon = Instantiate(itemIconPrefab, itemIconParent).GetComponent<ItemIcon>();

            var masterItem = ModelManager.Instance.GetMasterItem(userItem.item_id);
            itemIcon.Initialize(userItem, masterItem);
        }

        ItemIcon.OnClick.AddListener((userItem) =>
        {
            MasterItem masterItem = ModelManager.Instance.GetMasterItem(userItem.item_id);

            selectingItemIcon.Initialize(userItem, masterItem);

            itemName.text = masterItem.item_name;
            itemDescription.text = masterItem.description;

            string ability = "";
            if (masterItem.hp_max > 0)
            {
                ability += "最大HP+" + masterItem.hp_max + " ";
            }
            if (masterItem.strength > 0)
            {
                ability += "力+" + masterItem.strength + " ";
            }
            if (masterItem.defense > 0)
            {
                ability += "守+" + masterItem.defense + " ";
            }
            if (masterItem.speed > 0)
            {
                ability += "速+" + masterItem.speed + " ";
            }
            if (masterItem.luck > 0)
            {
                ability += "運+" + masterItem.luck + " ";
            }
            if (masterItem.spirit > 0)
            {
                ability += "魂+" + masterItem.spirit + " ";
            }
            if (masterItem.heart > 0)
            {
                ability += "心+" + masterItem.heart + " ";
            }
            itemAbility.text = ability;
        });




    }
}
