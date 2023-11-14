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
    [SerializeField] protected WalkerManager walkerManager;
    public WalkerManager WalkerManager => walkerManager;

    [SerializeField] private Camera usingCamera;
    public Camera UsingCamera => usingCamera;

    [SerializeField] private GameObject testEnemyPrefab;
    private List<EnemyBase> enemyList = new List<EnemyBase>();

    [SerializeField] protected GameObject collectableItemPrefab;
    protected List<CollectableItem> collectableItems = new List<CollectableItem>();

    private int currentFloor = 1;
    public int CurrentFloor => currentFloor;
    [HideInInspector] public UnityEvent<int> OnFloorStart = new UnityEvent<int>();

    [SerializeField] private Transform enemyRoot;
    [SerializeField] protected SpriteRenderer backgroundSpriteRenderer;

    // 最高到達フロアの更新をしたイベント
    public static UnityEvent<int> OnUpdateMaxFloor = new UnityEvent<int>();

    [SerializeField] private BossInfo bossInfo;

    public AdsInterstitial adsInterstitial;
    private int interstitialCount = 0;
    private const int InterstitialInterval = 5;

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

    public CharacterBase GetTargetRandomWalker()
    {
        List<CharacterBase> targetList = new List<CharacterBase>();

        List<int> probList = new List<int>();
        int baseProb = 1000;

        foreach (var walker in walkerManager.Walkers)
        {
            if (walker.IsAlive() == false)
            {
                continue;
            }
            targetList.Add(walker);
            probList.Add(baseProb);
            baseProb /= 2;
        }
        if (targetList.Count == 0)
        {
            return null;
        }
        int[] probArray = probList.ToArray();
        int index = UtilRand.GetIndex(probArray);
        return targetList[index];
    }

    protected virtual void StartNewFloor(int currentFloor)
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
        if (testEnemyPrefab != null)
        {
            SpawnEnemy(currentFloor);
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
            //Debug.Log(floor);

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
                //if (UnityEngine.Random.Range(0, 2) < 1)
                if (UnityEngine.Random.Range(0, 1000) < 1)
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
            else
            {
                bossInfo.gameObject.SetActive(false);
            }

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

    protected virtual void SaveRequestStart(int floorId)
    {
        ModelManager.Instance.UserGameData.last_quest_floor_id = floorId;
        ModelManager.Instance.UserGameData.restart_quest_floor_id = Mathf.Max(floorId, 1);
    }

    public void RequestStart(int floorId)
    {
        fadeScreenImage.Black();
        currentFloor = floorId;
        //Debug.Log($"RequestStart:{floorId}");
        SaveRequestStart(floorId);
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
            if (machine.isQuest)
            {
                machine.currentFloor = Mathf.Max(1, ModelManager.Instance.UserGameData.last_quest_floor_id);
            }
            else
            {
                machine.currentFloor = ModelManager.Instance.UserGameData.last_collect_area_id;
            }

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
            machine.currentFloor = currentFloor;
            machine.StartNewFloor(currentFloor);

            // ここ、本当は不要になります
            bool isAlive = false;
            foreach (var walker in machine.walkerManager.Walkers)
            {
                if (walker.IsAlive())
                {
                    isAlive = true;
                    break;
                }
            }
            if (isAlive == false)
            {
                foreach (var walker in machine.walkerManager.Walkers)
                {
                    walker.Revive();
                }
            }


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
                    if (machine.isQuest)
                    {
                        if (ModelManager.Instance.UserGameData.max_floor_id < machine.currentFloor)
                        {
                            OnUpdateMaxFloor.Invoke(machine.currentFloor);
                        }
                        machine.currentFloor++;
                        ModelManager.Instance.UserGameData.last_quest_floor_id = machine.currentFloor;
                    }

                    foreach (CharacterBase walker in machine.walkerManager.Walkers)
                    {
                        //Debug.Log("HealRate");
                        if (walker.IsAlive())
                        {
                            walker.HealRate(0.1f);
                        }
                    }


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
            // 生き返るタイミングで広告を表示
            machine.interstitialCount -= 1;
            if (machine.interstitialCount < 0 && machine.adsInterstitial.CanShowAd())
            {
                machine.adsInterstitial.OnContentClosed.AddListener(() =>
                {
                    AudioManager.Instance.Mute(false);
                    // ゲームスピードを戻す処理が必要かも
                    machine.StartCoroutine(machine.ResumeGameSpeedContinu());
                    machine.adsInterstitial.OnContentClosed.RemoveAllListeners();
                });
                AudioManager.Instance.Mute(true);
                machine.adsInterstitial.ShowInterstitialAd();
                machine.interstitialCount = InterstitialInterval;
            }

            foreach (var walker in machine.walkerManager.Walkers)
            {
                walker.Revive();
            }

            Debug.Log($"Restart:{ModelManager.Instance.UserGameData.restart_quest_floor_id}");

            ModelManager.Instance.UserGameData.restart_quest_floor_id = Mathf.Max(1, ModelManager.Instance.UserGameData.restart_quest_floor_id);

            ModelManager.Instance.UserGameData.last_quest_floor_id = ModelManager.Instance.UserGameData.restart_quest_floor_id;
            ChangeState(new FloorManager.FloorStart(machine, ModelManager.Instance.UserGameData.last_quest_floor_id));

        }
    }


    IEnumerator ResumeGameSpeedContinu()
    {
        int count = 0;
        while (count < 60)
        {
            yield return null;
            GameManager.Instance.GameSpeedResume();
            count += 1;
        }

        if (Time.timeScale < GameManager.Instance.GameSpeed)
        {
            StartCoroutine(ResumeGameSpeedContinu());
        }

    }
}
