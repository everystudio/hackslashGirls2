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
    [SerializeField] private MasterChara characterAsset;
    [SerializeField] private UserChara userChara;

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI rankText;

    [SerializeField] private Image selectingImage;

    [SerializeField] private Slider hpGauge;
    [SerializeField] private Slider strengthGauge;
    [SerializeField] private Slider defenseGauge;
    [SerializeField] private Slider speedGauge;
    [SerializeField] private Slider luckGauge;
    [SerializeField] private Slider spritGauge;


    public UnityEvent<UserChara> OnClick = new UnityEvent<UserChara>();

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(userChara);
    }
    public void Select(int charaId)
    {
        if (selectingImage != null)
        {
            selectingImage.gameObject.SetActive(characterAsset.chara_id == charaId);
        }
    }

    public void Set(MasterChara characterAsset, UserChara userChara)
    {
        this.characterAsset = characterAsset;
        this.userChara = userChara;

        if (iconImage != null)
        {
            iconImage.sprite = TextureManager.Instance.GetIconCharaSprite(characterAsset.chara_id);
        }
        if (nameText != null)
        {
            nameText.text = characterAsset.chara_name;
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

        if (hpGauge != null)
        {
            hpGauge.value = (float)userChara.hp / (float)userChara.hp_max;
        }
        if (strengthGauge != null)
        {
            strengthGauge.value = (float)userChara.strength / (float)UserChara.MAX_STATUS_PARAM;
        }
        if (defenseGauge != null)
        {
            defenseGauge.value = (float)userChara.defense / (float)UserChara.MAX_STATUS_PARAM;
        }
        if (speedGauge != null)
        {
            speedGauge.value = (float)userChara.speed / (float)UserChara.MAX_STATUS_PARAM;
        }
        if (luckGauge != null)
        {
            luckGauge.value = (float)userChara.luck / (float)UserChara.MAX_STATUS_PARAM;
        }
        if (spritGauge != null)
        {
            spritGauge.value = (float)userChara.spirit / (float)UserChara.MAX_STATUS_PARAM;
        }




    }

}
