using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using anogame;

public class WalkerManager : StateMachineBase<WalkerManager>
{
    public CharacterBase testingChara;
    private List<CharacterBase> walkers = new List<CharacterBase>();

    [SerializeField] private FadeScreenImage fadeScreenImage;

    private void Start()
    {
        fadeScreenImage.Black();
        walkers.Add(testingChara);

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
            foreach (var walker in machine.walkers)
            {
                walker.transform.localPosition = new Vector3(-6f, walker.transform.localPosition.y, walker.transform.localPosition.z);
                walker.WalkStart();
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
            Debug.Log("Walking");
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
