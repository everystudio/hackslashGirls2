using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using anogame;

public class ListArea : StateMachineBase<ListArea>
{
    [SerializeField] private GameObject areaBannerPrefab;
    [SerializeField] private Transform contentRoot;

    [SerializeField] private GameObject areaDetailPrefab;
    private GameObject areaDetailGameObject;
    private bool isQuest;

    public UnityEvent<int, bool> OnFloorStart = new UnityEvent<int, bool>();
    public UnityEvent<int> OnAreaStartCollect = new UnityEvent<int>();


    public void Init(bool isQuest)
    {
        this.isQuest = isQuest;
        // contentRoot以下の子オブジェクトを全て削除
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        var userAreaList = ModelManager.Instance.UserArea.List.FindAll(x => 0 <= x.floor_count);

        foreach (UserArea userArea in userAreaList)
        {
            var masterArea = ModelManager.Instance.GetMasterArea(userArea.area_id);

            var areaBanner = Instantiate(areaBannerPrefab, contentRoot).GetComponent<BannerArea>();

            areaBanner.Set(masterArea);
            areaBanner.AreaDetailButton.onClick.AddListener(() =>
            {
                BuildAreaDetail(masterArea.area_id);
            });

        }

    }
    private void OnDestroy()
    {
        if (areaDetailGameObject != null)
        {
            UIController.Instance.RemovePanel(areaDetailGameObject);
        }
    }

    private void BuildAreaDetail(int areaId)
    {
        if (areaDetailGameObject != null)
        {
            UIController.Instance.RemovePanel(areaDetailGameObject);
        }
        areaDetailGameObject = UIController.Instance.AddPanel(areaDetailPrefab, UIController.LAYER.DIALOG);

        if (areaDetailGameObject.TryGetComponent(out PanelAreaDetail panelAreaDetail))
        {
            panelAreaDetail.Init(isQuest, ModelManager.Instance.GetMasterArea(areaId));
            panelAreaDetail.CloseButton.onClick.AddListener(() =>
            {
                UIController.Instance.RemovePanel(areaDetailGameObject);
                areaDetailGameObject = null;
            });
            panelAreaDetail.OnFloorStart.AddListener((floorId, isQuest) =>
            {
                ModelManager.Instance.UserGameData.restart_quest_floor_id = floorId;
                //Debug.LogError("OnFloorStart:" + ModelManager.Instance.UserGameData.restart_quest_floor_id);
                OnFloorStart.Invoke(floorId, isQuest);
            });
            panelAreaDetail.OnAreaSelect.AddListener((area_id) =>
            {
                ModelManager.Instance.UserGameData.last_collect_area_id = area_id;
                OnAreaStartCollect.Invoke(area_id);
            });
        }
    }



}
