using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using anogame;
using TMPro;

public class PanelAreaDetail : UIPanel
{
    [SerializeField] private TextMeshProUGUI areaNameText;
    [SerializeField] private TextMeshProUGUI areaProgressText;

    [SerializeField] private GameObject itemIconPrefab;
    [SerializeField] private Transform itemIconParent;

    [SerializeField] private GameObject enemyIconPrefab;
    [SerializeField] private Transform enemyIconParent;

    [SerializeField] private GameObject floorStartButtonPrefab;
    [SerializeField] private Transform floorStartButtonParent;

    [SerializeField] private Button areaSelectButton;

    [SerializeField] private Button closeButton;
    public Button CloseButton => closeButton;

    [SerializeField] private GameObject rootFloorSelect;
    [SerializeField] private GameObject rootAreaSelect;

    [HideInInspector] public UnityEvent<int, bool> OnFloorStart = new UnityEvent<int, bool>();
    [HideInInspector] public UnityEvent<int> OnAreaSelect = new UnityEvent<int>();

    private bool isQuest;

    public void Init(bool isQuest, MasterArea masterArea)
    {
        //Debug.Log("PanelAreaDetail Init");
        rootFloorSelect.SetActive(isQuest);
        rootAreaSelect.SetActive(!isQuest);

        List<MasterFloor> masterFloors = ModelManager.Instance.MasterFloor.List.FindAll(floor => floor.area_id == masterArea.area_id);

        int minFloorId = 0;
        int maxFloorId = 0;
        foreach (var floor in masterFloors)
        {
            minFloorId = Mathf.Min(minFloorId, floor.floor_start);
            maxFloorId = Mathf.Max(maxFloorId, floor.floor_end);
        }

        areaNameText.text = masterArea.area_name;

        this.isQuest = isQuest;

        int floorDiff = maxFloorId - minFloorId;
        int floorProgress = ModelManager.Instance.UserGameData.max_floor_id - minFloorId;
        //Debug.Log(floorDiff);
        areaProgressText.text = "進捗：" + $"{floorProgress}/{floorDiff}";

        // itemIconParentの子要素を全削除
        foreach (Transform child in itemIconParent)
        {
            Destroy(child.gameObject);
        }
        List<MasterItem> masterItems = ModelManager.Instance.MasterItem.List.FindAll(item => item.area_id == masterArea.area_id);
        foreach (var item in masterItems)
        {
            var itemIcon = Instantiate(itemIconPrefab, itemIconParent).GetComponent<ItemIcon>();
            var userItem = ModelManager.Instance.GetUserItem(item.item_id);
            itemIcon.Initialize(item, userItem != null);
        }

        // enemyIconParentの子要素を全削除
        foreach (Transform child in enemyIconParent)
        {
            Destroy(child.gameObject);
        }
        List<MasterEnemy> masterEnemies = ModelManager.Instance.MasterEnemy.List.FindAll(enemy => enemy.area_id == masterArea.area_id);
        List<MasterEnemy> masterRareEnemies = ModelManager.Instance.MasterEnemy.List.FindAll(enemy => enemy.area_id == masterArea.area_id && enemy.rarity == 2);
        foreach (var enemy in masterEnemies)
        {
            int rarity = 1;
            var enemyIcon = Instantiate(enemyIconPrefab, enemyIconParent).GetComponent<EnemyIcon>();

            UserEnemy userEnemy = ModelManager.Instance.GetUserEnemy(enemy.enemy_id, rarity);
            enemyIcon.Set(enemy, rarity, userEnemy != null);
        }
        foreach (var enemy in masterRareEnemies)
        {
            var enemyIcon = Instantiate(enemyIconPrefab, enemyIconParent).GetComponent<EnemyIcon>();

            UserEnemy userEnemy = ModelManager.Instance.GetUserEnemy(enemy.enemy_id, enemy.rarity);
            enemyIcon.Set(enemy, enemy.rarity, userEnemy != null);
        }

        // floorStartButtonParentの子要素を全削除
        foreach (Transform child in floorStartButtonParent)
        {
            Destroy(child.gameObject);
        }

        // フロアのスタートボタンを10個生成
        bool isFirst = true;
        foreach (var floor in masterFloors)
        {
            FloorStartButton floorStartButton = Instantiate(floorStartButtonPrefab, floorStartButtonParent).GetComponent<FloorStartButton>();
            floorStartButton.Set(floor.floor_start);
            floorStartButton.Button.onClick.AddListener(() =>
            {
                Debug.Log(floor.floor_start + " Click!");
                OnFloorStart.Invoke(floor.floor_start, isQuest);
            });
            // ここはよくデバッグで解除する
            floorStartButton.Button.interactable = floor.floor_start <= ModelManager.Instance.UserGameData.max_floor_id;
            if (isFirst)
            {
                floorStartButton.Button.interactable = true;
                isFirst = false;
            }
        }

        areaSelectButton.onClick.AddListener(() =>
        {
            OnAreaSelect.Invoke(masterArea.area_id);
        });



    }
}
