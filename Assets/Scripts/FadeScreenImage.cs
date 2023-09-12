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
    [SerializeField] private float fadeTime = 0.25f;
    [SerializeField] private float fadeOutTime = 0.25f;
    [SerializeField] private float fadeInTime = 0.25f;
    [SerializeField] private float waitTime = 0.25f;
    [SerializeField] private float fadeOutWaitTime = 0.25f;
    [SerializeField] private float fadeInWaitTime = 0.25f;

    public UnityEvent OnFadeInCompleted = new UnityEvent();
    public UnityEvent OnFadeOutCompleted = new UnityEvent();

    public void FadeIn(Action OnFinished)
    {
        image.DOFade(0f, fadeInTime).SetEase(Ease.Linear).SetDelay(fadeOutWaitTime).OnComplete(() =>
        {
            OnFinished.Invoke();
            OnFadeInCompleted?.Invoke();
        });
    }

    public void FadeOut(Action OnFinished)
    {
        image.DOFade(1f, fadeOutTime).SetEase(Ease.Linear).SetDelay(fadeOutWaitTime).OnComplete(() =>
        {
            OnFinished.Invoke();
            OnFadeOutCompleted?.Invoke();
        });
    }

    public void Black()
    {
        image.color = Color.black;
    }
}
