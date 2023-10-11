using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using anogame;

[RequireComponent(typeof(Animator))]
public class EnemyBase : StateMachineBase<EnemyBase>
{
    private float health = 25f;
    public UnityEvent OnDie = new UnityEvent();

    private float attackRange = 1f;
    private FloorManager floorManager;
    private Animator animator;

    public UnityEvent OnAttackHitEvent = new UnityEvent();
    public UnityEvent OnAttackEndEvent = new UnityEvent();

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void FloorStart(FloorManager floorManager)
    {
        this.floorManager = floorManager;
        ChangeState(new EnemyBase.Battle(this));
    }

    public float Health
    {
        get { return health; }
    }

    public void TakeDamage(float damage)
    {
        if (health <= 0f)
        {
            return;
        }
        health -= damage;
        if (health <= 0f)
        {
            ChangeState(new EnemyBase.Die(this));
        }
    }

    private void SearchCharacter()
    {
    }

    private class Battle : StateBase<EnemyBase>
    {
        private float attackInterval = 2f;
        private float elapsedTime = 0f;
        private float range = 1f;

        public Battle(EnemyBase machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            elapsedTime = 0f;
            range = machine.attackRange;
        }
        public override void OnUpdateState()
        {
            base.OnUpdateState();
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= attackInterval)
            {
                var targetCharacter = machine.floorManager.GetNearWalker(machine.transform.position, range);

                if (targetCharacter != null)
                {
                    targetCharacter.TakeDamage(1);
                    //ChangeState(new Attack(machine, targetCharacter));
                }
                else
                {
                    elapsedTime = 0f;
                }
            }
        }
    }

    private class Attack : StateBase<EnemyBase>
    {
        private CharacterBase targetCharacter;
        public Attack(EnemyBase machine, CharacterBase targetCharacter) : base(machine)
        {
            this.targetCharacter = targetCharacter;
        }
        public override void OnEnterState()
        {
            base.OnEnterState();

            machine.OnAttackHitEvent.AddListener(() =>
            {
            });
            machine.OnAttackEndEvent.AddListener(() =>
            {
                ChangeState(new Battle(machine));
            });
            machine.animator.SetTrigger("attack");
        }

        public override void OnExitState()
        {
            base.OnExitState();
            machine.OnAttackHitEvent.RemoveAllListeners();
            machine.OnAttackEndEvent.RemoveAllListeners();
        }
    }

    private class Die : StateBase<EnemyBase>
    {
        public Die(EnemyBase machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            machine.OnDie?.Invoke();
            Destroy(machine.gameObject);
        }
    }

    public void OnAttackHitHandler()
    {
        OnAttackHitEvent?.Invoke();
    }
    public void OnAttackEndHandler()
    {
        OnAttackEndEvent?.Invoke();
    }
}
