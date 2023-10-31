using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PanelStarUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI starUpText;
    [SerializeField] private TextMeshProUGUI shojiStarText;

    [SerializeField] private UICharacterCore preCharaCore;
    [SerializeField] private UICharacterCore afterCharaCore;

    [SerializeField] private Button starUpButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI starUpButtonText;

    [HideInInspector] public UnityEvent OnStarUp = new UnityEvent();

    public void Redraw(UserChara user, MasterChara master)
    {
        preCharaCore.Set(master, user);

        var nextUserChara = new UserChara(master);
        nextUserChara.star = Mathf.Min(user.star + 1, 5);
        afterCharaCore.Set(master, nextUserChara);

        var userStar = ModelManager.Instance.GetUserStar(user.chara_id);
        int currentStarNum = userStar == null ? 0 : userStar.num;

        int nextStarNum = Defines.GetStarUp(user.star + 1);
        bool canStarUp = nextStarNum <= currentStarNum;
        if (5 <= nextUserChara.star)
        {
            canStarUp = false;
        }

        starUpText.text = $"{currentStarNum} → {currentStarNum - Defines.GetStarUp(user.star + 1)}";

        if (canStarUp)
        {
            starUpButtonText.text = "開放！";
        }
        else
        {
            if (5 <= nextUserChara.star)
            {
                starUpButtonText.text = "上限です";
            }
            else
            {
                starUpButtonText.text = "Star不足";
            }
        }

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        starUpButton.interactable = canStarUp;
        starUpButton.onClick.RemoveAllListeners();
        starUpButton.onClick.AddListener(() =>
        {
            user.star = nextUserChara.star;
            OnStarUp.Invoke();

            Redraw(user, master);
        });
    }

}
