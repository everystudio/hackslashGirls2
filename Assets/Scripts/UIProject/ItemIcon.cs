using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemIcon : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Image icon;
    [SerializeField] private TextMeshProUGUI count;

    public void Initialize(UserItem userItem, MasterItem masterItem)
    {
        icon.sprite = TextureManager.Instance.GetIconItemSprite(masterItem.item_id);
        count.text = "x" + userItem.item_num.ToString();
    }

}
