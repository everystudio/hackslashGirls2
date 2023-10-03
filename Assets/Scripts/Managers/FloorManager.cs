using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;
using System;

public class FloorManager : StateMachineBase<FloorManager>
{
    [SerializeField] private bool isQuest;
    [SerializeField] private FadeScreenImage fadeScreenImage;
    [SerializeField] private WalkerManager walkerManager;


    [SerializeField] private GameObject testEnemyPrefab;
    private List<EnemyBase> enemies = new List<EnemyBase>();

    private void Start()
    {
        ChangeState(new FloorManager.Standby(this));
    }

    private void StartNewFloor()
    {
        if (testEnemyPrefab != null)
        {
            SpawnTestEnemy();
        }
    }

    private void SpawnTestEnemy()
    {
        // enemiesをクリア
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();

        for (int i = 0; i < 5; i++)
        {
            EnemyBase enemyBase = Instantiate(testEnemyPrefab, transform).GetComponent<EnemyBase>();
            enemies.Add(enemyBase);
            enemyBase.transform.localPosition = new Vector3(i * 0.75f + 1.5f, 0, 0);
            enemyBase.OnDie.AddListener(() =>
            {
                enemies.Remove(enemyBase);
            });
        }
    }

    public EnemyBase GetNearestEnemy(CharacterBase walker)
    {
        EnemyBase nearestEnemy = null;
        float nearestDistance = 100f;
        foreach (var enemy in enemies)
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
            ChangeState(new FloorManager.FloorStart(machine));
        }
    }

    private class FloorStart : StateBase<FloorManager>
    {
        public FloorStart(FloorManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            machine.StartNewFloor();
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
                ChangeState(new FloorManager.FloorStart(machine));
            });
        }
    }


}
