using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableItem : MonoBehaviour
{
    public int item_id;

    public UnityEvent<int> OnCollect = new UnityEvent<int>();


    // 誰かに回収されているかどうかのフラグ
    public bool is_collected = false;

    private Animator animator;

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
        OnCollect.Invoke(item_id);
    }


}
