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

    [SerializeField] private GameObject testEnemyPrefab;
    private List<EnemyBase> enemyList = new List<EnemyBase>();

    [SerializeField] private GameObject collectableItemPrefab;
    private List<CollectableItem> collectableItems = new List<CollectableItem>();

    private int currentFloor = 1;
    public int CurrentFloor => currentFloor;
    [HideInInspector] public UnityEvent<int> OnFloorStart = new UnityEvent<int>();

    private void Start()
    {
        ChangeState(new FloorManager.Standby(this));
    }

    public CharacterBase GetNearWalker(Vector3 position, float range)
    {
        CharacterBase nearestWalker = null;
        float nearestDistance = 100f;
        foreach (var walker in walkerManager.Walkers)
        {
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
        if (isQuest)
        {
            if (testEnemyPrefab != null)
            {
                SpawnTestEnemy();
            }
        }
        else
        {
            if (collectableItemPrefab != null)
            {
                SpawnCollectableItem();
            }
        }


        OnFloorStart.Invoke(currentFloor);
    }


    private void SpawnTestEnemy()
    {
        // enemiesをクリア
        foreach (var enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }
        enemyList.Clear();

        for (int i = 0; i < 5; i++)
        {
            EnemyBase enemyBase = Instantiate(testEnemyPrefab, transform).GetComponent<EnemyBase>();
            enemyList.Add(enemyBase);
            enemyBase.transform.localPosition = new Vector3(i * 0.75f + 1.5f, 0, 0);
            enemyBase.OnDie.AddListener(() =>
            {
                enemyList.Remove(enemyBase);
            });
        }
    }

    private void SpawnCollectableItem()
    {
        // collectableItemsをクリア
        foreach (var collectableItem in collectableItems)
        {
            Destroy(collectableItem.gameObject);
        }
        collectableItems.Clear();

        for (int i = 0; i < 3; i++)
        {
            CollectableItem collectableItem = Instantiate(collectableItemPrefab, transform).GetComponent<CollectableItem>();
            collectableItems.Add(collectableItem);
            collectableItem.transform.localPosition = new Vector3(i * 0.75f + 1.5f, 0, 0);
            /*
            collectableItem.OnCollect.AddListener(() =>
            {
                collectableItems.Remove(collectableItem);
            });
            */
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


    private class Standby : StateBase<FloorManager>
    {
        public Standby(FloorManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            machine.fadeScreenImage.Black();
            machine.walkerManager.StandbyParty();
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

            foreach (var enemy in machine.enemyList)
            {
                enemy.FloorStart(machine);
            }
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
                machine.currentFloor++;
                ChangeState(new FloorManager.FloorStart(machine, machine.currentFloor));
            });
        }
    }


}
