using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class PanelAchievement : UIPanel
{
    [SerializeField] private GameObject bannerPrefab;
    [SerializeField] private Transform bannerParent;

    // Start is called before the first frame update
    protected override void initialize()
    {
        base.initialize();

        // bannerParentの子供を全て削除
        foreach (Transform child in bannerParent)
        {
            Destroy(child.gameObject);
        }


        foreach (var master in ModelManager.Instance.MasterAchievement.List)
        {
            MissionBanner banner = Instantiate(bannerPrefab, bannerParent).GetComponent<MissionBanner>();
            banner.Set(master);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
