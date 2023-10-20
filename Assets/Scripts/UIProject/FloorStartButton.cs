using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloorStartButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI floorNumberText;
    [SerializeField] private Button floorStartButton;
    public Button Button => floorStartButton;

    public void Set(int floorNumber)
    {
        floorNumberText.text = floorNumber.ToString();
    }
}
