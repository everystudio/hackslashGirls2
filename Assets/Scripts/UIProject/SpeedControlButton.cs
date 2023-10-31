using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Button))]
public class SpeedControlButton : MonoBehaviour
{
    public static UnityEvent OnClicedHandler = new UnityEvent();
    [SerializeField] private TextMeshProUGUI sppedText;

    private void Awake()
    {
        int currentSpeed = Defines.GetCurrentGameSpeed(ModelManager.Instance.UserGameData.game_speed_index);
        UpdateSpeed(currentSpeed);

        GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClicedHandler.Invoke();
        });

        ModelManager.Instance.OnChangeUserGameData.AddListener((value) =>
        {
            int currentSpeed = Defines.GetCurrentGameSpeed(ModelManager.Instance.UserGameData.game_speed_index);
            UpdateSpeed(currentSpeed);
        });

    }

    public void UpdateSpeed(int speed)
    {
        sppedText.text = $"Speedx{speed}";

    }



}
