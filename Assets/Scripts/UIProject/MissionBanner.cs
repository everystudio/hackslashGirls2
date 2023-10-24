using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField] private Button recieveButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    MasterAchievement masterAchievement;
    UserAchievement userAchievement;

    public static UnityEvent<MasterAchievement, UserAchievement> OnRecieve = new UnityEvent<MasterAchievement, UserAchievement>();

    public void Set(MasterAchievement masterAchievement, UserAchievement userAchievement)
    {
        this.masterAchievement = masterAchievement;
        this.userAchievement = userAchievement;

        prizeText.text = masterAchievement.GetPrizeText();

        if (userAchievement.is_completed)
        {
            buttonText.text = userAchievement.is_received ? "受け取り済み" : "受け取る";
            recieveButton.interactable = !userAchievement.is_received && userAchievement.is_completed;
        }
        else
        {
            buttonText.text = "進行中";
            recieveButton.interactable = false;
        }

        recieveButton.onClick.AddListener(() =>
        {
            buttonText.text = "受け取り済み";
            recieveButton.interactable = false;
            OnRecieve.Invoke(this.masterAchievement, this.userAchievement);
            transform.SetAsLastSibling();
        });

        if (masterAchievement.prize_type == 1)
        {
            prizeImage.sprite = spriteCoin;
        }
        else if (masterAchievement.prize_type == 2)
        {
            prizeImage.sprite = spriteGem;
        }
        else if (masterAchievement.prize_type == 3)
        {
            prizeImage.sprite = spriteTicket;
        }
        else
        {
            prizeImage.sprite = TextureManager.Instance.GetIconItemSprite(masterAchievement.prize_id);
        }
        missionText.text = masterAchievement.description;
    }
}
