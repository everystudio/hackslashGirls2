using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterBase : MonoBehaviour
{
    private bool isDead = false;
    private bool isArrived = false;
    private const float EDGE_X = 6f;
    [SerializeField] private float speed = 2.5f;
    public UnityEvent OnArrived = new UnityEvent();

    void Update()
    {
        if (isArrived)
        {
            return;
        }
        else if (EDGE_X <= transform.localPosition.x)
        {
            isArrived = true;
            OnArrived.Invoke();
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void WalkStart()
    {
        isArrived = false;
    }
}
