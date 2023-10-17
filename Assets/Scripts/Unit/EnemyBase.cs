using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using anogame;

[RequireComponent(typeof(Animator))]
public class EnemyBase : StateMachineBase<EnemyBase>
{
    private float health = 25f;
    private float healthMax = 25f;
    public float HealthMax
    {
        get { return healthMax; }
    }

    public UnityEvent OnDie = new UnityEvent();

    private float attackRange = 3f;
    private FloorManager floorManager;
    private Animator animator;

    public UnityEvent OnAttackHitEvent = new UnityEvent();
    public UnityEvent OnAttackEndEvent = new UnityEvent();

    public MasterEnemy masterEnemy;

    private int attackAssist = 0;
    public UnityEvent<float> OnChangeHealth = new UnityEvent<float>();

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void FloorStart(FloorManager floorManager, MasterEnemy masterEnemy, bool isBoss)
    {
        this.floorManager = floorManager;
        this.masterEnemy = masterEnemy;
        health = masterEnemy.hp;

        if (isBoss)
        {
            health *= 5f;
            attackAssist = masterEnemy.attack;
            transform.localScale *= 2.0f;
        }
        else
        {
            attackAssist = 0;
        }
        spriteRenderer.sprite = TextureManager.Instance.GetEnemySprite(masterEnemy.filename);
        healthMax = health;

        ChangeState(new EnemyBase.Waiting(this));
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
        health = Mathf.Max(health, 0f);
        OnChangeHealth?.Invoke(health);

        if (health <= 0f)
        {
            ChangeState(new EnemyBase.Die(this));
        }
    }

    private void SearchCharacter()
    {
    }

    private class Waiting : StateBase<EnemyBase>
    {
        float delayTime = 0f;
        public Waiting(EnemyBase machine) : base(machine)
        {
        }

        public override void OnUpdateState()
        {
            delayTime += Time.deltaTime;
            base.OnUpdateState();
            if (delayTime <= 1f)
            {
                return;
            }
            var targetCharacter = machine.floorManager.GetNearWalker(machine.transform.position, machine.attackRange + 2f);

            if (targetCharacter != null)
            {
                ChangeState(new Battle(machine));
            }
        }

    }

    private class Battle : StateBase<EnemyBase>
    {
        private float attackInterval = 100f;
        private float elapsedTime = 0f;

        public Battle(EnemyBase machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            elapsedTime = 0f;
        }
        public override void OnUpdateState()
        {
            base.OnUpdateState();
            elapsedTime += Time.deltaTime * machine.masterEnemy.speed;
            if (elapsedTime >= attackInterval)
            {
                var targetCharacter = machine.floorManager.GetNearWalker(machine.transform.position, machine.attackRange);

                if (targetCharacter != null)
                {
                    ChangeState(new Attack(machine, targetCharacter));
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
                //Debug.Log(targetCharacter);
                targetCharacter.TakeDamage(machine.masterEnemy.attack);
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
            machine.animator.SetTrigger("dead");
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
    public void OnDeadEndHandler()
    {
        Destroy(gameObject);
    }


}
