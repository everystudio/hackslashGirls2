using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using anogame;
using UnityEngine.EventSystems;

public class UICharacterCore : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CharacterAsset characterAsset;
    [SerializeField] private UserChara userChara;

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI rankText;

    [SerializeField] private Image selectingImage;

    public UnityEvent<UserChara> OnClick = new UnityEvent<UserChara>();

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(userChara);
    }
    public void Select(int charaId)
    {
        if (selectingImage != null)
        {
            selectingImage.gameObject.SetActive(characterAsset.id == charaId);
        }
    }

    public void Set(CharacterAsset characterAsset, UserChara userChara)
    {
        this.characterAsset = characterAsset;
        this.userChara = userChara;

        if (iconImage != null)
        {
            iconImage.sprite = characterAsset.icon;
        }
        if (nameText != null)
        {
            nameText.text = characterAsset.name;
        }
        if (levelText != null)
        {
            levelText.text = userChara.level.ToString();
        }
        if (rankText != null)
        {
            rankText.text = userChara.rank.ToString();
        }
        if (selectingImage != null)
        {
            selectingImage.gameObject.SetActive(false);
        }
    }

}
