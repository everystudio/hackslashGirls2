using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using anogame;

public class WalkerManager : StateMachineBase<WalkerManager>
{
    [SerializeField] private bool isQuest;
    private List<CharacterBase> walkers = new List<CharacterBase>();

    [SerializeField] private FadeScreenImage fadeScreenImage;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject walkerPrefab;

    private void Start()
    {
        fadeScreenImage.Black();

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
        ChangeState(new WalkerManager.FadeIn(this));
    }

    private class FadeIn : StateBase<WalkerManager>
    {
        public FadeIn(WalkerManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            int index = 0;
            foreach (var walker in machine.walkers)
            {
                walker.transform.position = machine.spawnPoint.position;
                walker.WalkStart(walker.userChara.partyIndex * 0.5f + 0.5f);
                index += 1;
            }

            machine.fadeScreenImage.FadeIn(() =>
            {
                ChangeState(new WalkerManager.Walking(machine));
            });
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
                ChangeState(new WalkerManager.FadeOut(machine));
            }
        }
    }

    private class FadeOut : StateBase<WalkerManager>
    {
        public FadeOut(WalkerManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            Debug.Log("FadeOut");
            machine.fadeScreenImage.FadeOut(() =>
            {
                ChangeState(new WalkerManager.End(machine));
            });
        }
    }

    private class End : StateBase<WalkerManager>
    {
        public End(WalkerManager machine) : base(machine)
        {
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            Debug.Log("End");
            // 終了処理とかをする予定
            ChangeState(new WalkerManager.FadeIn(machine));
        }
    }
}
