using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using anogame;

public class PanelAchievement : UIPanel
{
    [SerializeField] private GameObject bannerPrefab;
    [SerializeField] private Transform bannerParent;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button allCollectButton;

    private List<UserAchievement> recievableList = new List<UserAchievement>();

    private void Redraw()
    {
        // bannerParentの子供を全て削除
        foreach (Transform child in bannerParent)
        {
            Destroy(child.gameObject);
        }

        // まだ受け取っていないものを前側にソートする
        List<UserAchievement> showList = ModelManager.Instance.UserAchievement.List;
        showList.Sort((a, b) =>
        {
            if (a.is_received && !b.is_received)
            {
                return 1;
            }
            else if (!a.is_received && b.is_received)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });

        recievableList.Clear();
        foreach (var userAchievement in showList)
        {

            if (userAchievement.is_completed && !userAchievement.is_received)
            {
                recievableList.Add(userAchievement);
            }

            MasterAchievement master = ModelManager.Instance.GetMasterAchievement(userAchievement.achievement_id);

            MissionBanner banner = Instantiate(bannerPrefab, bannerParent).GetComponent<MissionBanner>();

            banner.Set(master, userAchievement);
        }

        closeButton.onClick.AddListener(() =>
        {
            UIController.Instance.RemovePanel(this.gameObject);
        });

        if (0 < recievableList.Count)
        {
            allCollectButton.gameObject.SetActive(true);
        }
        else
        {
            allCollectButton.gameObject.SetActive(false);
        }

        allCollectButton.onClick.RemoveAllListeners();
        allCollectButton.onClick.AddListener(() =>
        {
            foreach (var userAchievement in recievableList)
            {
                var master = ModelManager.Instance.GetMasterAchievement(userAchievement.achievement_id);

                // ここが呼ぶのはなんか変な気がするけど
                MissionBanner.OnRecieve.Invoke(master, userAchievement);
            }
            Redraw();
        });
    }


    // Start is called before the first frame update
    protected override void initialize()
    {
        base.initialize();
        Redraw();
    }





}
