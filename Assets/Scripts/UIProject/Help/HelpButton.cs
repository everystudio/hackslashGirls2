using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;

    public void Initialize(string text, UnityEngine.Events.UnityAction onClick)
    {
        this.text.text = text;
        button.onClick.AddListener(onClick);
    }
}
