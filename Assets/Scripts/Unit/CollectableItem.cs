using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableItem : MonoBehaviour
{
    MasterItem masterItem;
    public static UnityEvent<int> OnCollect = new UnityEvent<int>();


    // 誰かに回収されているかどうかのフラグ
    public bool is_collected = false;

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
    }

    public void OnCollectAnimationEnd()
    {
        OnCollect.Invoke(masterItem.item_id);
    }

    public void Initialize(MasterItem masterItem)
    {
        this.masterItem = masterItem;
        spriteRenderer.sprite = TextureManager.Instance.GetIconItemSprite(masterItem.item_id);
    }
}
