using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GachaProbBanner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI probText;
    [SerializeField] private TextMeshProUGUI nameText;

    public void Set(string title, float probability)
    {
        probText.text = $"{probability:0.00}%";
        nameText.text = title;
    }
}
