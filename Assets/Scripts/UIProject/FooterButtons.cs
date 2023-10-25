using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FooterButtons : MonoBehaviour
{
    public static UnityEvent<string> OnFooterButtonEvent = new UnityEvent<string>();
    [SerializeField] private Button charaButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button mainButton;
    [SerializeField] private Button gachaButton;
    [SerializeField] private Button settingButton;

    private void Awake()
    {
        charaButton.onClick.AddListener(() => OnFooterButtonEvent.Invoke("Chara"));
        itemButton.onClick.AddListener(() => OnFooterButtonEvent.Invoke("Item"));
        mainButton.onClick.AddListener(() => OnFooterButtonEvent.Invoke("Main"));
        gachaButton.onClick.AddListener(() => OnFooterButtonEvent.Invoke("Gacha"));
        settingButton.onClick.AddListener(() => OnFooterButtonEvent.Invoke("Settings"));
    }


}

