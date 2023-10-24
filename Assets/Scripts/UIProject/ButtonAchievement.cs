using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using anogame;

public class ButtonAchievement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform panelRoot;
    public void OnPointerClick(PointerEventData eventData)
    {
        UIController.Instance.AddPanel("PanelAchievement", panelRoot);
    }
}