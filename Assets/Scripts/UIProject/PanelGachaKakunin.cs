using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PanelGachaKakunin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI ticketText;

    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;

    public UnityEvent<int> OnOK = new UnityEvent<int>();
    public UnityEvent OnCancel = new UnityEvent();

    private int gachaCount;

    private void Awake()
    {
        okButton.onClick.AddListener(() =>
        {
            OnOK.Invoke(gachaCount);
        });
        cancelButton.onClick.AddListener(() =>
        {
            OnCancel.Invoke();
        });
    }

    public void Initialize(int count)
    {
        gachaCount = count;
        messageText.text = $"チケットを使用して" + "\n" + $"{count}回召喚をしますか？";
        ticketText.text = $"{ModelManager.Instance.UserGameData.ticket} → {ModelManager.Instance.UserGameData.ticket - count}";
    }
}
