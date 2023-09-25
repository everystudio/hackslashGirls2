using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class TestManager : SingletonStateMachineBase<TestManager>
{
    private void Start()
    {
        ChangeState(new TestManager.Waiting(this));
    }

    private class Waiting : StateBase<TestManager>
    {
        public Waiting(TestManager machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            Debug.Log("Waiting");
        }
    }
}
