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
    private FloorManager floorManager;
    private readonly float PARTY_OFFSET = 1f;

    private UnityEvent OnAttackHit = new UnityEvent();
    private UnityEvent OnAttackEnd = new UnityEvent();

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

    public void WalkStart(float delay, FloorManager floorManager)
    {
        this.floorManager = floorManager;
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
            // 一番近い敵が攻撃範囲内にいるかどうか
            EnemyBase nearestEnemy = machine.floorManager.GetNearestEnemy(machine);
            if (nearestEnemy != null)
            {
                float distance = Vector3.Distance(machine.transform.position, nearestEnemy.transform.position);
                if (distance <= machine.characterAsset.attackRange + machine.userChara.partyIndex * machine.PARTY_OFFSET)
                {
                    ChangeState(new CharacterBase.Fighting(machine, nearestEnemy));
                    return;
                }
            }


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

    private class Fighting : StateBase<CharacterBase>
    {
        private EnemyBase nearestEnemy;
        private float interval = 1f;

        public Fighting(CharacterBase machine, EnemyBase nearestEnemy) : base(machine)
        {
            this.machine = machine;
            this.nearestEnemy = nearestEnemy;
        }
        public override void OnEnterState()
        {
            // 向きをあわせるために
            machine.animator.SetFloat("speed", 1f);

            nearestEnemy.OnDie.AddListener(TargetEnemyDie);
        }

        private void TargetEnemyDie()
        {
            ChangeState(new CharacterBase.Walking(machine));
        }

        public override void OnUpdateState()
        {
            interval -= Time.deltaTime * machine.characterAsset.attackSpeed;
            if (interval <= 0f)
            {
                ChangeState(new CharacterBase.Attack(machine, nearestEnemy));
            }
        }
        public override void OnExitState()
        {
            nearestEnemy.OnDie.RemoveListener(TargetEnemyDie);
        }
    }

    private class Attack : StateBase<CharacterBase>
    {
        private EnemyBase nearestEnemy;

        public Attack(CharacterBase machine, EnemyBase nearestEnemy) : base(machine)
        {
            this.machine = machine;
            this.nearestEnemy = nearestEnemy;
        }
        public override void OnEnterState()
        {
            machine.OnAttackHit.AddListener(() =>
            {
                nearestEnemy.TakeDamage(machine.characterAsset.attack);
            });
            machine.OnAttackEnd.AddListener(() =>
            {
                ChangeState(new CharacterBase.Walking(machine));
            });
            machine.animator.SetTrigger("attack");
        }
        public override void OnExitState()
        {
            base.OnExitState();
            machine.OnAttackHit.RemoveAllListeners();
            machine.OnAttackEnd.RemoveAllListeners();
        }
    }

    public void AnimationAttackHit()
    {
        OnAttackHit?.Invoke();
    }
    public void AnimationAttackEnd()
    {
        OnAttackEnd?.Invoke();
    }
}