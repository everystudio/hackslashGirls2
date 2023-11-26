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

    public UnityEvent<UserEnemy> OnDie = new UnityEvent<UserEnemy>();
    public static UnityEvent<UserEnemy, MasterEnemy, Vector3> OnAnyDie = new UnityEvent<UserEnemy, MasterEnemy, Vector3>();

    private float attackRange = 3f;
    private FloorManager floorManager;
    private Animator animator;

    public UnityEvent OnAttackHitEvent = new UnityEvent();
    public UnityEvent OnAttackEndEvent = new UnityEvent();

    public MasterEnemy masterEnemy;

    private int attackAssist = 0;
    public UnityEvent<float> OnChangeHealth = new UnityEvent<float>();

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Material rareMaterial;

    UserEnemy userEnemy;



    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void FloorStart(FloorManager floorManager, MasterEnemy masterEnemy, bool isBoss, int rarity = 1)
    {
        this.floorManager = floorManager;
        this.masterEnemy = masterEnemy;
        health = masterEnemy.hp;

        userEnemy = new UserEnemy();
        userEnemy.enemy_id = masterEnemy.enemy_id;
        userEnemy.area_id = masterEnemy.area_id;
        userEnemy.rarity = rarity;
        userEnemy.count = 1;


        if (isBoss)
        {
            health *= 5f;
            attackAssist = masterEnemy.attack;
            transform.localScale *= 2.0f;
        }
        else if (1 < rarity)
        {
            health *= 10f;
            attackAssist = masterEnemy.attack / 2;
            spriteRenderer.material = rareMaterial;
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
                //var targetCharacter = machine.floorManager.GetNearWalker(machine.transform.position, machine.attackRange);
                //var targetCharacter = machine.floorManager.GetNearWalker(machine.transform.position, Mathf.Infinity);
                var targetCharacter = machine.floorManager.GetTargetRandomWalker();

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
                AudioManager.Instance.PlayRandomAttackEnemySFX();

                int speed = targetCharacter.userChara.speed;
                speed += targetCharacter.userChara.luck / 2;
                for (int i = 0; i < speed / Defines.DODGE_DIVIDE; i++)
                {
                    // 1%の確率で回避
                    if (Random.Range(0, 100) < 1)
                    {
                        FlyingText.Create(targetCharacter.transform, "回避！", targetCharacter.transform.position + new Vector3(0, 0.5f, 0f), 0f);
                        return;
                    }
                }
                FlyingText.Create(targetCharacter.transform, $"<color=red>{machine.masterEnemy.attack.ToString()}</color>", targetCharacter.transform.position + new Vector3(0, 0.25f, 0f), 0.2f);
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
            machine.OnDie?.Invoke(machine.userEnemy);
            OnAnyDie?.Invoke(machine.userEnemy, machine.masterEnemy, machine.transform.localPosition);
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
