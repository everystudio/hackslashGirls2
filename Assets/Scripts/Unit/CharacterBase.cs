using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using anogame;

public class CharacterBase : StateMachineBase<CharacterBase>
{
    private const float EDGE_X = 13f;
    [SerializeField] private float speed = 2.5f;
    public UnityEvent OnArrived = new UnityEvent();
    public UnityEvent OnDied = new UnityEvent();

    private Animator animator;

    [SerializeField] private MasterChara characterAsset;
    [SerializeField] private OverrideSprite overrideSprite;

    public UserChara userChara;
    private FloorManager floorManager;
    private readonly float PARTY_OFFSET = 1f;

    private UnityEvent OnAttackHit = new UnityEvent();
    private UnityEvent OnAttackEnd = new UnityEvent();

    // 編成用のやつではなく、現在冒険中のインデックス
    private int partyIndex = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool IsAlive()
    {
        return 0 < userChara.hp;
    }

    public void TakeDamage(int damage)
    {
        if (IsAlive() == false)
        {
            return;
        }

        userChara.hp -= damage;
        if (userChara.hp <= 0f)
        {
            ChangeState(new CharacterBase.Die(this));
        }
        UserChara.OnChanged.Invoke(userChara);
    }

    public void SetChara(UserChara userChara)
    {
        this.userChara = userChara;
        characterAsset = ModelManager.Instance.GetMasterChara(userChara.chara_id);
        overrideSprite.OverrideTexture = TextureManager.Instance.GetMiniCharaTexture(characterAsset.chara_id);
    }

    public void SetIndex(int index)
    {
        partyIndex = index;
    }

    public void WalkStart(float delay, FloorManager floorManager)
    {
        this.floorManager = floorManager;
        if (IsAlive())
        {
            ChangeState(new CharacterBase.WalkDelay(this, delay));
        }
        else
        {
            ChangeState(new CharacterBase.Close(this));
        }
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
                if (distance <= machine.characterAsset.attack_range + machine.partyIndex * machine.PARTY_OFFSET)
                {
                    ChangeState(new CharacterBase.Fighting(machine, nearestEnemy));
                    return;
                }
            }

            // 一番近いアイテムが攻撃範囲内にいるかどうか
            CollectableItem nearestCollectableItem = machine.floorManager.GetNearestCollectableItem(machine);
            if (nearestCollectableItem != null)
            {
                float distance = Vector3.Distance(machine.transform.position, nearestCollectableItem.transform.position);
                if (distance <= machine.characterAsset.attack_range)
                {
                    ChangeState(new CharacterBase.Collecting(machine, nearestCollectableItem));
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

        private void TargetEnemyDie(UserEnemy userEnemy)
        {
            ChangeState(new CharacterBase.Walking(machine));
        }

        public override void OnUpdateState()
        {
            interval -= Time.deltaTime * machine.characterAsset.attack_speed;
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
                if (nearestEnemy != null)
                {
                    var obj = Instantiate(Resources.Load("FlyingText"), machine.transform.parent) as GameObject;

                    Vector3 randomOffsetXY = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0f);

                    obj.transform.position = nearestEnemy.transform.position + new Vector3(0, 0, -1f) + randomOffsetXY;


                    var flyingText = obj.GetComponent<FlyingText>();
                    flyingText.Initialize(machine.userChara.strength);
                    Destroy(obj, 5f);

                    nearestEnemy.TakeDamage(machine.userChara.strength);
                }
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

    private class Collecting : StateBase<CharacterBase>
    {
        private float waittime = 1.5f;
        private CollectableItem nearestCollectableItem;

        public Collecting(CharacterBase machine, CollectableItem nearestCollectableItem) : base(machine)
        {
            this.machine = machine;
            this.nearestCollectableItem = nearestCollectableItem;
        }
        public override void OnEnterState()
        {
            // キャラクターのアニメーションを変更する
            machine.animator.SetTrigger("win");
            nearestCollectableItem.Collect();
        }

        public override void OnUpdateState()
        {
            base.OnUpdateState();
            waittime -= Time.deltaTime;
            if (waittime <= 0f)
            {
                ChangeState(new CharacterBase.Walking(machine));
            }
        }
        public override void OnExitState()
        {
            machine.animator.Play("Idle");
        }
    }

    private class Die : StateBase<CharacterBase>
    {
        public Die(CharacterBase machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            machine.OnDied.Invoke();
            machine.OnArrived.Invoke();
            machine.animator.SetTrigger("die");
        }
    }

    private class Close : StateBase<CharacterBase>
    {
        public Close(CharacterBase machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
        }
    }

}