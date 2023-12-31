using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentButton : MonoBehaviour
{
    private MasterItem masterItem;
    private UserItem userItem;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI requireCountText;
    [SerializeField] private Image plusImage;

    public void Initialize(bool equiping, MasterEquip masterEquip, MasterItem masterItem, UserItem userItem)
    {
        this.masterItem = masterItem;
        this.userItem = userItem;
        itemImage.sprite = TextureManager.Instance.GetIconItemSprite(masterItem.filename);
        itemImage.color = equiping ? Color.white : Color.gray;

        if (equiping)
        {
            plusImage.gameObject.SetActive(false);
            requireCountText.gameObject.SetActive(false);
        }
        else
        {
            plusImage.gameObject.SetActive(userItem != null && masterEquip.require_count <= userItem.item_num);
            requireCountText.gameObject.SetActive(true);

            int hasNum = userItem == null ? 0 : userItem.item_num;
            requireCountText.text = $"{hasNum}/{masterEquip.require_count}";
        }
    }
}
