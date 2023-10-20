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

    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI luckText;
    [SerializeField] private TextMeshProUGUI spritText;
    [SerializeField] private TextMeshProUGUI heartText;

    [SerializeField] private Slider hpGauge;
    [SerializeField] private Slider strengthGauge;
    [SerializeField] private Slider defenseGauge;
    [SerializeField] private Slider speedGauge;
    [SerializeField] private Slider luckGauge;
    [SerializeField] private Slider spritGauge;
    [SerializeField] private Slider heartGauge;

    [SerializeField] private StarView starView;

    private bool isBlocking = false;
    public UnityEvent<UserChara> OnClick = new UnityEvent<UserChara>();

    public void SetBlockClick()
    {
        if (selectingImage != null)
        {
            selectingImage.gameObject.SetActive(true);
            selectingImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
        isBlocking = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isBlocking)
        {
            return;
        }
        OnClick.Invoke(userChara);
    }
    public void Select(int charaId)
    {
        if (isBlocking)
        {
            return;
        }
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
            levelText.text = "Level." + userChara.level.ToString();
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
            //Debug.Log(userChara.hp + " " + userChara.hp_max);
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
        if (heartGauge != null)
        {
            heartGauge.value = (float)userChara.heart / (float)UserChara.MAX_STATUS_PARAM;
        }

        if (hpText != null)
        {
            hpText.text = userChara.hp.ToString() + "/" + userChara.hp_max.ToString();
        }
        if (strengthText != null)
        {
            strengthText.text = "力：" + userChara.strength.ToString();
        }
        if (defenseText != null)
        {
            defenseText.text = "守：" + userChara.defense.ToString();
        }
        if (speedText != null)
        {
            speedText.text = "速：" + userChara.speed.ToString();
        }
        if (luckText != null)
        {
            luckText.text = "運：" + userChara.luck.ToString();
        }
        if (spritText != null)
        {
            spritText.text = "魂：" + userChara.spirit.ToString();
        }
        if (heartText != null)
        {
            heartText.text = "心：" + userChara.heart.ToString();
        }

        if (starView != null)
        {
            starView.Set(userChara.star);
        }

    }

    public void Set(UserChara userChara)
    {
        //Debug.Log(userChara);
        MasterChara masterChara = ModelManager.Instance.GetMasterChara(userChara.chara_id);
        Set(masterChara, userChara);
    }


}
