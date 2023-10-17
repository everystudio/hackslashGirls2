using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlyingText : MonoBehaviour
{
    [SerializeField] private TextMeshPro damageText;

    public void Initialize(int damage)
    {
        damageText.text = damage.ToString();
    }
}
