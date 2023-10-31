using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using anogame;
using System;

public class WalkerManager : StateMachineBase<WalkerManager>
{
    [SerializeField] private bool isQuest;
    private List<CharacterBase> walkers = new List<CharacterBase>();
    public List<CharacterBase> Walkers => walkers;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject walkerPrefab;

    public UnityEvent OnArrived = new UnityEvent();
    private FloorManager floorManager;

    public void StandbyParty()
    {
        //Debug.Log("StandbyParty");
        // walkersを初期化
        foreach (var walker in walkers)
        {
            Destroy(walker.gameObject);
        }
        walkers.Clear();


        List<UserChara> partyList = null;
        if (isQuest)
        {
            partyList = ModelManager.Instance.userChara.List.FindAll(x => x.questPartyId > 0);
        }
        else
        {
            partyList = ModelManager.Instance.userChara.List.FindAll(x => x.collectPartyId > 0);
        }
        foreach (var userChara in partyList)
        {
            var walker = Instantiate(walkerPrefab, spawnPoint).GetComponent<CharacterBase>();
            walker.SetChara(userChara);
            // walkder.transform以下のLayerをspawnPointと同じにする
            foreach (Transform t in walker.transform)
            {
                t.gameObject.layer = spawnPoint.gameObject.layer;
            }
            walkers.Add(walker);
        }
    }

    public void WalkStart(FloorManager floorManager)
    {
        this.floorManager = floorManager;

        ResetWalkerIndex();

        int index = 1;
        foreach (var walker in walkers)
        {
            walker.transform.position = spawnPoint.position;
            if (walker.IsAlive())
            {
                walker.WalkStart(index * 0.5f + 0.5f, floorManager);
                index += 1;
            }
            else
            {
                walker.gameObject.SetActive(false);
            }
        }
        ChangeState(new WalkerManager.Walking(this));
    }


    private void Start()
    {
        ChangeState(new WalkerManager.Waiting(this));
    }

    private class Waiting : StateBase<WalkerManager>
    {
        public Waiting(WalkerManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
        }
    }


    private class Walking : StateBase<WalkerManager>
    {
        private int arrivedCount = 0;
        private int walkingNum = 0;
        public Walking(WalkerManager machine) : base(machine)
        {
        }

        public override void OnEnterState()
        {
            //Debug.Log("Walking");
            foreach (var walker in machine.walkers)
            {
                walker.OnArrived.RemoveAllListeners();
                walker.OnArrived.AddListener(OnArrived);
                walker.OnDied.RemoveAllListeners();
                walker.OnDied.AddListener(OnDied);

                if (walker.IsAlive())
                {
                    walkingNum += 1;
                }
            }
        }

        private void OnDied()
        {
            machine.ResetWalkerIndex();
        }

        private void OnArrived()
        {
            arrivedCount++;
            if (walkingNum <= arrivedCount)
            {
                ChangeState(new WalkerManager.Arrived(machine));
            }
        }
    }

    private void ResetWalkerIndex()
    {
        int index = 1;
        foreach (var walker in walkers)
        {
            if (walker.IsAlive())
            {
                walker.SetIndex(index);
                index += 1;
            }
        }

    }

    private class Arrived : StateBase<WalkerManager>
    {
        public Arrived(WalkerManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            machine.OnArrived.Invoke();
        }
    }




}
