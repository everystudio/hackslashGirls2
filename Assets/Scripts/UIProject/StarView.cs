using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarView : MonoBehaviour
{
    [SerializeField] private Sprite starIconSprite;
    [SerializeField] private Sprite starIconSpriteGray;
    [SerializeField] private Image[] starIcons;

    public void Set(int star)
    {
        for (int i = 0; i < starIcons.Length; i++)
        {
            if (starIcons[i] != null)
            {
                starIcons[i].sprite = (i < star) ? starIconSprite : starIconSpriteGray;
            }
        }
    }
}
