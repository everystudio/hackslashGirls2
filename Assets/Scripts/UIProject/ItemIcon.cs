using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;

public class ItemIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEngine.UI.Image icon;
    [SerializeField] private TextMeshProUGUI count;

    public static UnityEvent<UserItem> OnClick = new UnityEvent<UserItem>();
    private UserItem userItem;

    public void Initialize(UserItem userItem, MasterItem masterItem)
    {
        icon.sprite = TextureManager.Instance.GetIconItemSprite(masterItem.item_id);
        icon.enabled = true;
        count.text = "x" + userItem.item_num.ToString();

        this.userItem = userItem;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(userItem);
    }
}
