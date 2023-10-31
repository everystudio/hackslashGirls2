using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using anogame;
using System;

public class FloorManager : StateMachineBase<FloorManager>
{
    [SerializeField] private bool isQuest;
    [SerializeField] private FadeScreenImage fadeScreenImage;
    [SerializeField] private WalkerManager walkerManager;
    public WalkerManager WalkerManager => walkerManager;

    [SerializeField] private Camera usingCamera;
    public Camera UsingCamera => usingCamera;

    [SerializeField] private GameObject testEnemyPrefab;
    private List<EnemyBase> enemyList = new List<EnemyBase>();

    [SerializeField] private GameObject collectableItemPrefab;
    private List<CollectableItem> collectableItems = new List<CollectableItem>();

    private int currentFloor = 1;
    public int CurrentFloor => currentFloor;
    [HideInInspector] public UnityEvent<int> OnFloorStart = new UnityEvent<int>();

    [SerializeField] private Transform enemyRoot;
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

    // 最高到達フロアの更新をしたイベント
    public static UnityEvent<int> OnUpdateMaxFloor = new UnityEvent<int>();

    [SerializeField] private BossInfo bossInfo;

    private void Start()
    {
        if (bossInfo != null)
        {
            bossInfo.gameObject.SetActive(false);
        }
        ChangeState(new FloorManager.Standby(this));
    }

    public CharacterBase GetNearWalker(Vector3 position, float range)
    {
        CharacterBase nearestWalker = null;
        float nearestDistance = 100f;
        foreach (var walker in walkerManager.Walkers)
        {
            if (walker.IsAlive() == false)
            {
                continue;
            }
            float distance = Vector3.Distance(position, walker.transform.position);

            if (distance < nearestDistance && distance < range)
            {
                nearestDistance = distance;
                nearestWalker = walker;
            }
        }
        return nearestWalker;
    }

    private void StartNewFloor(int currentFloor)
    {
        MasterFloor currentFloorModel = ModelManager.Instance.MasterFloor.List.Find(x =>
            x.floor_start <= currentFloor && currentFloor <= x.floor_end);

        if (backgroundSpriteRenderer == null)
        {
            backgroundSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        if (currentFloorModel != null)
        {
            backgroundSpriteRenderer.sprite = TextureManager.Instance.GetBackgroundSprite(currentFloorModel.background);
        }

        walkerManager.StandbyParty();
        if (isQuest)
        {
            if (testEnemyPrefab != null)
            {
                SpawnEnemy(currentFloor);
            }
        }
        else
        {
            if (collectableItemPrefab != null)
            {
                SpawnCollectableItem(currentFloor);
            }
        }


        OnFloorStart.Invoke(currentFloor);
    }


    private void SpawnEnemy(int floor)
    {
        // enemiesをクリア
        foreach (var enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }
        enemyList.Clear();


        for (int i = 0; i < Defines.MaxEnemyNum; i++)
        {
            EnemyBase enemyBase = Instantiate(testEnemyPrefab, transform).GetComponent<EnemyBase>();
            enemyList.Add(enemyBase);
            //Debug.Log(enemyRoot.transform.localPosition.y);
            enemyBase.transform.localPosition = new Vector3(i * 0.75f + 1.5f, enemyRoot.transform.localPosition.y, 0);
            enemyBase.OnDie.AddListener((userEnemy) =>
            {
                enemyList.Remove(enemyBase);
            });

            // 現在のフロアからFloorModelを検索
            MasterFloor floorModel = ModelManager.Instance.MasterFloor.List.Find(x =>
                x.floor_start <= floor && floor <= x.floor_end);

            bool isBoss = false;
            MasterEnemy enemyModel = null;

            bool forceBossAppearFlag = false;

            if ((forceBossAppearFlag || floor == floorModel.floor_end) && i == Defines.MaxEnemyNum - 1)
            {
                // 最後の敵
                enemyModel = ModelManager.Instance.GetMasterEnemy(floorModel.enemy_id_boss);
                isBoss = true;
            }
            else
            {
                // FloorModelからEnemyModelを検索
                enemyModel = ModelManager.Instance.GetMasterEnemy(floorModel.GetRandomEnemyID());
            }

            int rarity = enemyModel.rarity;
            if (1 < rarity)
            {
                // 0.1%の確率でレア敵
                if (UnityEngine.Random.Range(0, 2) < 1)
                {
                    rarity = enemyModel.rarity;
                }
                else
                {
                    rarity = 1;
                }
            }

            enemyBase.FloorStart(this, enemyModel, isBoss, rarity);
            if (isBoss)
            {
                bossInfo.gameObject.SetActive(true);
                bossInfo.Initialize(enemyBase);
            }

        }
    }

    private void SpawnCollectableItem(int floor)
    {
        // collectableItemsをクリア
        foreach (var collectableItem in collectableItems)
        {
            Destroy(collectableItem.gameObject);
        }
        collectableItems.Clear();

        // 現在のマスターフロアを取得
        MasterFloor floorModel = ModelManager.Instance.MasterFloor.List.Find(x =>
            x.floor_start <= floor && floor <= x.floor_end);
        MasterArea areaModel = ModelManager.Instance.GetMasterArea(floorModel.area_id);

        // 現在のマスターアイテムを取得
        List<MasterItem> masterItems = ModelManager.Instance.MasterItem.List.FindAll(
            item => item.area_id == areaModel.area_id && item.floor_start <= floor);

        //Debug.Log(masterItems.Count);
        int[] probArray = new int[masterItems.Count];
        for (int i = 0; i < masterItems.Count; i++)
        {
            probArray[i] = masterItems[i].prob;
        }

        for (int i = 0; i < 3; i++)
        {
            // MasterItem.probを元にランダムにアイテムを生成
            int itemIndex = UtilRand.GetIndex(probArray);
            //Debug.Log(itemIndex);
            CollectableItem collectableItem = Instantiate(collectableItemPrefab, transform).GetComponent<CollectableItem>();
            collectableItem.Initialize(masterItems[itemIndex]);
            collectableItems.Add(collectableItem);
            collectableItem.transform.localPosition = new Vector3(i * 0.75f + 1.5f, 0, 0);
        }
    }


    public EnemyBase GetNearestEnemy(CharacterBase walker)
    {
        EnemyBase nearestEnemy = null;
        float nearestDistance = 100f;
        foreach (var enemy in enemyList)
        {
            float distance = Vector3.Distance(walker.transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    public CollectableItem GetNearestCollectableItem(CharacterBase walker)
    {
        CollectableItem nearestCollectableItem = null;
        float nearestDistance = 100f;
        foreach (var collectableItem in collectableItems.FindAll(x => x.is_collected == false))
        {
            float distance = Vector3.Distance(walker.transform.position, collectableItem.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCollectableItem = collectableItem;
            }
        }
        return nearestCollectableItem;
    }

    public void RequestStart(int floorId)
    {
        fadeScreenImage.Black();
        currentFloor = floorId;
        ChangeState(new FloorManager.FloorStart(this, floorId));
    }


    private class Standby : StateBase<FloorManager>
    {
        public Standby(FloorManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            machine.fadeScreenImage.Black();
            //machine.walkerManager.StandbyParty();

            machine.currentFloor = ModelManager.Instance.UserGameData.last_quest_floor_id;


            ChangeState(new FloorManager.FloorStart(machine, machine.currentFloor));
        }
    }

    private class FloorStart : StateBase<FloorManager>
    {
        private int currentFloor;
        public FloorStart(FloorManager machine, int currentFloor) : base(machine)
        {
            this.currentFloor = currentFloor;
        }
        public override void OnEnterState()
        {
            machine.StartNewFloor(currentFloor);
            machine.fadeScreenImage.FadeIn(() =>
            {
                ChangeState(new FloorManager.Walking(machine));
            });
        }
    }

    private class Walking : StateBase<FloorManager>
    {
        public Walking(FloorManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            machine.walkerManager.OnArrived.RemoveAllListeners();
            machine.walkerManager.OnArrived.AddListener(OnArrived);
            machine.walkerManager.WalkStart(machine);



        }

        private void OnArrived()
        {
            machine.walkerManager.OnArrived.RemoveAllListeners();
            ChangeState(new FloorManager.Arrived(machine));
        }
    }

    private class Arrived : StateBase<FloorManager>
    {
        public Arrived(FloorManager machine) : base(machine)
        {
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            machine.fadeScreenImage.FadeOut(() =>
            {
                if (machine.isQuest)
                {
                    if (ModelManager.Instance.UserGameData.max_floor_id < machine.currentFloor)
                    {
                        OnUpdateMaxFloor.Invoke(machine.currentFloor);
                    }
                }

                //Debug.Log("currentFloor:" + machine.currentFloor);


                // 生存者の確認
                bool isAlive = false;
                foreach (var walker in machine.walkerManager.Walkers)
                {
                    if (walker.IsAlive())
                    {
                        isAlive = true;
                        break;
                    }
                }

                if (isAlive)
                {
                    machine.currentFloor++;
                    ChangeState(new FloorManager.FloorStart(machine, machine.currentFloor));
                }
                else
                {
                    ChangeState(new FloorManager.Restart(machine));
                }
            });
        }
    }

    private class Restart : StateBase<FloorManager>
    {
        public Restart(FloorManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {




            ChangeState(new FloorManager.FloorStart(machine, 1));

        }



    }
}
