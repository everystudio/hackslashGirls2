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
    [SerializeField] private TextMeshProUGUI hpSingleText;
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

    [SerializeField] private Image strengthImage100;
    [SerializeField] private Image strengthImage200;
    [SerializeField] private Image strengthImage300;

    [SerializeField] private Image defenseImage100;
    [SerializeField] private Image defenseImage200;
    [SerializeField] private Image defenseImage300;

    [SerializeField] private Image speedImage100;
    [SerializeField] private Image speedImage200;
    [SerializeField] private Image speedImage300;

    [SerializeField] private Image luckImage100;
    [SerializeField] private Image luckImage200;
    [SerializeField] private Image luckImage300;

    [SerializeField] private Image spritImage100;
    [SerializeField] private Image spritImage200;
    [SerializeField] private Image spritImage300;

    [SerializeField] private Image heartImage100;
    [SerializeField] private Image heartImage200;
    [SerializeField] private Image heartImage300;





    [SerializeField] private GameObject downRoot;

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

    public void Clear()
    {
        if (iconImage != null)
        {
            iconImage.sprite = null;
        }
        if (nameText != null)
        {
            nameText.text = "";
        }
        if (levelText != null)
        {
            levelText.text = "";
        }
        if (rankText != null)
        {
            rankText.text = "";
        }
        if (selectingImage != null)
        {
            selectingImage.gameObject.SetActive(false);
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

        int param_interval = 300;

        if (strengthImage100 != null)
        {
            strengthImage100.fillAmount = (float)userChara.strength / (float)param_interval;
            strengthImage200.fillAmount = (float)(userChara.strength - param_interval * 1) / (float)param_interval;
            strengthImage300.fillAmount = (float)(userChara.strength - param_interval * 2) / (float)param_interval;
        }

        if (defenseImage100 != null)
        {
            defenseImage100.fillAmount = (float)userChara.defense / (float)param_interval;
            defenseImage200.fillAmount = (float)(userChara.defense - param_interval * 1) / (float)param_interval;
            defenseImage300.fillAmount = (float)(userChara.defense - param_interval * 2) / (float)param_interval;
        }

        if (speedImage100 != null)
        {
            speedImage100.fillAmount = (float)userChara.speed / (float)param_interval;
            speedImage200.fillAmount = (float)(userChara.speed - param_interval * 1) / (float)param_interval;
            speedImage300.fillAmount = (float)(userChara.speed - param_interval * 2) / (float)param_interval;
        }

        if (luckImage100 != null)
        {
            luckImage100.fillAmount = (float)userChara.luck / (float)param_interval;
            luckImage200.fillAmount = (float)(userChara.luck - param_interval * 1) / (float)param_interval;
            luckImage300.fillAmount = (float)(userChara.luck - param_interval * 2) / (float)param_interval;
        }

        if (spritImage100 != null)
        {
            spritImage100.fillAmount = (float)userChara.spirit / (float)param_interval;
            spritImage200.fillAmount = (float)(userChara.spirit - param_interval * 1) / (float)param_interval;
            spritImage300.fillAmount = (float)(userChara.spirit - param_interval * 2) / (float)param_interval;
        }

        if (heartImage100 != null)
        {
            heartImage100.fillAmount = (float)userChara.heart / (float)param_interval;
            heartImage200.fillAmount = (float)(userChara.heart - param_interval * 1) / (float)param_interval;
            heartImage300.fillAmount = (float)(userChara.heart - param_interval * 2) / (float)param_interval;
        }

        if (defenseGauge != null)
        {
            defenseGauge.value = (float)userChara.defense / (float)UserChara.MAX_STATUS_PARAM;
            defenseGauge.value = 0f;
        }
        if (speedGauge != null)
        {
            speedGauge.value = (float)userChara.speed / (float)UserChara.MAX_STATUS_PARAM;
            speedGauge.value = 0f;
        }
        if (luckGauge != null)
        {
            luckGauge.value = (float)userChara.luck / (float)UserChara.MAX_STATUS_PARAM;
            luckGauge.value = 0f;
        }
        if (spritGauge != null)
        {
            spritGauge.value = (float)userChara.spirit / (float)UserChara.MAX_STATUS_PARAM;
            spritGauge.value = 0f;
        }
        if (heartGauge != null)
        {
            heartGauge.value = (float)userChara.heart / (float)UserChara.MAX_STATUS_PARAM;
            heartGauge.value = 0f;
        }

        if (hpText != null)
        {
            hpText.text = userChara.hp.ToString() + "/" + userChara.hp_max.ToString();
        }
        if (hpSingleText != null)
        {
            hpSingleText.text = userChara.hp.ToString();
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

        if (downRoot != null)
        {
            downRoot.SetActive(!(0 < userChara.hp));
        }

    }

    public void Set(UserChara userChara)
    {
        //Debug.Log(userChara.chara_id);
        MasterChara masterChara = ModelManager.Instance.GetMasterChara(userChara.chara_id);
        Set(masterChara, userChara);
    }


}
