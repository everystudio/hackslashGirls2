using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionBanner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI prizeText;
    [SerializeField] private Image prizeImage;
    [SerializeField] private TextMeshProUGUI missionText;

    [SerializeField] private Sprite spriteCoin;
    [SerializeField] private Sprite spriteGem;
    [SerializeField] private Sprite spriteTicket;

    MasterAchievement masterAchievement;

    public void Set(MasterAchievement achievement)
    {
        masterAchievement = achievement;

        prizeText.text = achievement.GetPrizeText();

        if (achievement.prize_type == 1)
        {
            prizeImage.sprite = spriteCoin;
        }
        else if (achievement.prize_type == 2)
        {
            prizeImage.sprite = spriteGem;
        }
        else if (achievement.prize_type == 3)
        {
            prizeImage.sprite = spriteTicket;
        }
        else
        {
            prizeImage.sprite = TextureManager.Instance.GetIconItemSprite(achievement.prize_id);
        }
        missionText.text = achievement.description;
    }
}
