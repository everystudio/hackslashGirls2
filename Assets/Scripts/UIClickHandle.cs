using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIClickHandle : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnClicked = new UnityEvent();
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click" + gameObject.name);
        OnClicked.Invoke();
    }

}
