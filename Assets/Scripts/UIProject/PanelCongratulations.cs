using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCongratulations : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

    }

    private void OnEnable()
    {
        closeButton.gameObject.SetActive(false);
        StartCoroutine(AppearCloseButton(5f));
    }

    IEnumerator AppearCloseButton(float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        closeButton.gameObject.SetActive(true);

    }
}
