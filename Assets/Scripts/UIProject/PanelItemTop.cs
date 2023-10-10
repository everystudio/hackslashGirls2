using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class PanelItemTop : UIPanel
{
    [SerializeField] private GameObject itemIconPrefab;
    [SerializeField] private Transform itemIconParent;

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


    }
}
