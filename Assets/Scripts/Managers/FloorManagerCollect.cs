using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class FloorManagerCollect : FloorManager
{
    protected override void SaveRequestStart(int floorId)
    {
        ModelManager.Instance.UserGameData.last_collect_area_id = floorId;
    }

    protected override void StartNewFloor(int area_id)
    {
        //Debug.Log("Collect AreaID:" + area_id);
        List<MasterFloor> masterFloors = ModelManager.Instance.MasterFloor.List.FindAll(x => x.area_id == area_id);
        int minFloorId = 999999;
        int maxFloorId = 0;
        foreach (var floor in masterFloors)
        {
            minFloorId = Mathf.Min(minFloorId, floor.floor_start);
            maxFloorId = Mathf.Max(maxFloorId, floor.floor_end);
        }

        //Debug.Log("minFloorId:" + minFloorId);
        //Debug.Log("maxFloorId:" + maxFloorId);
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

        //Debug.Log(floor);
        foreach (var collectableItem in collectableItems)
        {
            Destroy(collectableItem.gameObject);
        }
        collectableItems.Clear();

        // 現在のマスターアイテムを取得
        List<MasterItem> masterItems = ModelManager.Instance.MasterItem.List.FindAll(
            item => item.area_id == area_id && item.floor_start <= floor);

        //Debug.Log(masterItems.Count);
        int[] probArray = new int[masterItems.Count];
        for (int i = 0; i < masterItems.Count; i++)
        {
            probArray[i] = masterItems[i].prob;
        }

        int totalCollectPartyLuck = ModelManager.Instance.GetTotalCollectPartyLuck();
        float luckRate = Mathf.Min(1.0f, 0.001f * totalCollectPartyLuck);

        for (int i = 0; i < 4; i++)
        {

            float dropRate = Mathf.Lerp(30, 70, luckRate);

            if (UnityEngine.Random.Range(0, 100) < dropRate)
            {
                continue;
            }

            float itemDropRate = Mathf.Lerp(90, 70, luckRate);

            CollectableItem collectableItem = Instantiate(collectableItemPrefab, transform).GetComponent<CollectableItem>();
            // 80%の確率でコインを獲得する
            if (UnityEngine.Random.Range(0, 100) < itemDropRate)
            {
                int amount = area_id * 100;
                collectableItem.Initialize(ModelManager.Instance.GetMasterItem(Defines.CoinItemID), amount);
            }
            else
            {
                // MasterItem.probを元にランダムにアイテムを生成
                int itemIndex = UtilRand.GetIndex(probArray);
                //Debug.Log(itemIndex);
                collectableItem.Initialize(masterItems[itemIndex], 1);
            }
            collectableItems.Add(collectableItem);
            collectableItem.transform.localPosition = new Vector3(i * 0.75f + 1.5f, 0, 0);
        }
    }


}
