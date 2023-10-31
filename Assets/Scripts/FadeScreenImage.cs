using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class FadeScreenImage : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float fadeOutTime = 0.25f;
    [SerializeField] private float fadeInTime = 0.25f;
    [SerializeField] private float fadeOutWaitTime = 0.25f;
    [SerializeField] private float fadeInWaitTime = 0.25f;

    public UnityEvent OnFadeInCompleted = new UnityEvent();
    public UnityEvent OnFadeOutCompleted = new UnityEvent();

    private Tweener tweener;

    public void FadeIn(Action OnFinished)
    {

        tweener = image.DOFade(0f, fadeInTime).SetEase(Ease.Linear).SetDelay(fadeInWaitTime).OnComplete(() =>
        {
            OnFinished.Invoke();
            OnFadeInCompleted?.Invoke();
            image.raycastTarget = false;
            tweener = null;
        });
    }

    public void FadeOut(Action OnFinished)
    {
        Debug.Log("start fadeout");
        image.raycastTarget = true;
        tweener = image.DOFade(1f, fadeOutTime).SetEase(Ease.Linear).SetDelay(fadeOutWaitTime).OnComplete(() =>
        {
            Debug.Log("end fadeout");
            OnFinished.Invoke();
            OnFadeOutCompleted?.Invoke();
            tweener = null;
        });
    }

    public void Black()
    {
        if (tweener != null)
        {
            tweener.Kill();
        }
        image.color = Color.black;
        image.raycastTarget = true;
    }
}
