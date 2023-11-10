using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaxFloorText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI maxFloorText;
    [SerializeField] private TextMeshProUGUI mezaseText;

    private void Start()
    {
        UpdateText(ModelManager.Instance.UserGameData.max_floor_id);
        FloorManager.OnUpdateMaxFloor.AddListener(UpdateText);
    }

    private void UpdateText(int floor)
    {
        maxFloorText.text = floor.ToString();
        if (500 <= floor)
        {
            mezaseText.text = "おめでとう";
        }
    }

}
