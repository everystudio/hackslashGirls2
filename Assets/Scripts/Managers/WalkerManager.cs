using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using anogame;

public class WalkerManager : StateMachineBase<WalkerManager>
{
    [SerializeField] private bool isQuest;
    private List<CharacterBase> walkers = new List<CharacterBase>();

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject walkerPrefab;

    public UnityEvent OnArrived = new UnityEvent();

    public void StandbyParty()
    {
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
        int index = 0;
        foreach (var walker in walkers)
        {
            walker.transform.position = spawnPoint.position;
            walker.WalkStart(walker.userChara.partyIndex * 0.5f + 0.5f, floorManager);
            index += 1;
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
            }
        }
        private void OnArrived()
        {
            arrivedCount++;
            if (machine.walkers.Count <= arrivedCount)
            {
                ChangeState(new WalkerManager.Arrived(machine));
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
