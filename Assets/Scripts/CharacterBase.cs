using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using anogame;

public class CharacterBase : StateMachineBase<CharacterBase>
{
    private bool isDead = false;
    private const float EDGE_X = 13f;
    [SerializeField] private float speed = 2.5f;
    public UnityEvent OnArrived = new UnityEvent();

    private Animator animator;

    [SerializeField] private CharacterAsset characterAsset;
    [SerializeField] private OverrideSprite overrideSprite;

    public UserChara userChara;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetChara(UserChara userChara)
    {
        this.userChara = userChara;
        characterAsset = ModelManager.Instance.GetCharacterAsset(userChara.id);
        overrideSprite.OverrideTexture = characterAsset.miniTexture;
    }

    public void WalkStart(float delay)
    {
        ChangeState(new CharacterBase.WalkDelay(this, delay));
    }
    private class WalkDelay : StateBase<CharacterBase>
    {
        private float delay;
        public WalkDelay(CharacterBase machine, float delay) : base(machine)
        {
            this.delay = delay;
        }
        public override void OnUpdateState()
        {
            delay -= Time.deltaTime;
            if (delay <= 0f)
            {
                ChangeState(new CharacterBase.Walking(machine));
            }
        }

    }


    private class Walking : StateBase<CharacterBase>
    {
        private bool isArrived = false;
        public Walking(CharacterBase machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            isArrived = false;
            machine.animator.SetFloat("speed", 1f);
        }
        public override void OnUpdateState()
        {
            if (isArrived)
            {
                return;
            }
            else if (EDGE_X <= machine.transform.localPosition.x)
            {
                isArrived = true;
                machine.OnArrived.Invoke();
            }
            machine.transform.Translate(Vector2.right * machine.speed * Time.deltaTime);
        }

        public override void OnExitState()
        {
            machine.animator.SetFloat("speed", 0f);
        }

    }
}
