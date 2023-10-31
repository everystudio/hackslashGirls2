using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class FloorManagerCollect : FloorManager
{
    protected override void StartNewFloor(int area_id)
    {
        Debug.Log("Collect AreaID:" + area_id);
        List<MasterFloor> masterFloors = ModelManager.Instance.MasterFloor.List.FindAll(x => x.area_id == area_id);
        int minFloorId = 999999;
        int maxFloorId = 0;
        foreach (var floor in masterFloors)
        {
            minFloorId = Mathf.Min(minFloorId, floor.floor_start);
            maxFloorId = Mathf.Max(maxFloorId, floor.floor_end);
        }

        Debug.Log("minFloorId:" + minFloorId);
        Debug.Log("maxFloorId:" + maxFloorId);
        maxFloorId = Mathf.Min(maxFloorId, ModelManager.Instance.UserGameData.max_floor_id);
        maxFloorId = Mathf.Max(minFloorId + 1, maxFloorId);


        if (backgroundSpriteRenderer == null)
        {
            backgroundSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        if (masterFloors[0] != null)
        {
            backgroundSpriteRenderer.sprite = TextureManager.Instance.GetBackgroundSprite(masterFloors[0].background);
        }

        walkerManager.StandbyParty();

        if (collectableItemPrefab != null)
        {
            SpawnCollectableItem(area_id, maxFloorId);
        }


        OnFloorStart.Invoke(area_id);
    }

    protected void SpawnCollectableItem(int area_id, int floor)
    {
        // collectableItemsをクリア

        Debug.Log(floor);
        foreach (var collectableItem in collectableItems)
        {
            Destroy(collectableItem.gameObject);
        }
        collectableItems.Clear();

        // 現在のマスターアイテムを取得
        List<MasterItem> masterItems = ModelManager.Instance.MasterItem.List.FindAll(
            item => item.area_id == area_id && item.floor_start <= floor);

        Debug.Log(masterItems.Count);
        int[] probArray = new int[masterItems.Count];
        for (int i = 0; i < masterItems.Count; i++)
        {
            probArray[i] = masterItems[i].prob;
        }

        for (int i = 0; i < 3; i++)
        {
            CollectableItem collectableItem = Instantiate(collectableItemPrefab, transform).GetComponent<CollectableItem>();
            // 90%の確率でコインを獲得する
            if (UnityEngine.Random.Range(0, 10) < 9)
            {
                collectableItem.Initialize(ModelManager.Instance.GetMasterItem(Defines.CoinItemID));
            }
            else
            {
                // MasterItem.probを元にランダムにアイテムを生成
                int itemIndex = UtilRand.GetIndex(probArray);
                //Debug.Log(itemIndex);
                collectableItem.Initialize(masterItems[itemIndex]);
            }
            collectableItems.Add(collectableItem);
            collectableItem.transform.localPosition = new Vector3(i * 0.75f + 1.5f, 0, 0);
        }
    }


}
