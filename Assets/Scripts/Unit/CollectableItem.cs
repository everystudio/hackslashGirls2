using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableItem : MonoBehaviour
{
    MasterItem masterItem;
    public static UnityEvent<int, int> OnCollect = new UnityEvent<int, int>();

    // 誰かに回収されているかどうかのフラグ
    public bool is_collected = false;
    public int amount = 1;

    private Animator animator;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Collect()
    {
        is_collected = true;
        //gameObject.SetActive(false);
        animator.SetTrigger("Collect");
        OnCollect.Invoke(masterItem.item_id, amount);
    }

    public void OnCollectAnimationEnd()
    {
        // ここでOnCollectイベントを呼んでたけど、エフェクト音のなるタイミングがおそくなってしまったので、処理ごと前倒しした
    }

    public void Initialize(MasterItem masterItem, int amount)
    {
        this.masterItem = masterItem;
        this.amount = amount;
        spriteRenderer.sprite = TextureManager.Instance.GetIconItemSprite(masterItem.item_id);
    }
}
