using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentFloorNum : MonoBehaviour
{
    [SerializeField] private FloorManager floorManager;
    [SerializeField] private TextMeshProUGUI currentFloorNumText;
    private void Awake()
    {
        floorManager.OnFloorStart.AddListener((currentFloor) =>
        {
            SetFloorText(currentFloor);
        });
    }

    private void SetFloorText(int floorNum)
    {
        currentFloorNumText.text = "フロア：" + floorNum.ToString("D3");
    }

}
