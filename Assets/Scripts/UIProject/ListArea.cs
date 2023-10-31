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


    public void Init(bool isQuest)
    {
        this.isQuest = isQuest;
        // contentRoot以下の子オブジェクトを全て削除
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        var areas = ModelManager.Instance.MasterArea.List;
        foreach (var area in areas)
        {
            var areaBanner = Instantiate(areaBannerPrefab, contentRoot).GetComponent<BannerArea>();

            areaBanner.Set(area);
            areaBanner.AreaDetailButton.onClick.AddListener(() =>
            {
                BuildAreaDetail(area.area_id);
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
                OnFloorStart.Invoke(floorId, isQuest);
            });
            panelAreaDetail.AreaSelectButton.onClick.AddListener(() =>
            {
                UIController.Instance.RemovePanel(areaDetailGameObject);
                areaDetailGameObject = null;
            });
        }
    }



}
