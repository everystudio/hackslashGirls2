using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIcon : MonoBehaviour
{
    [SerializeField] private Image enemyIconImage;
    [SerializeField] private Image background;

    public void Set(MasterEnemy masterEnemy, int rarity, bool isFind)
    {
        enemyIconImage.sprite = TextureManager.Instance.GetEnemySprite(masterEnemy.filename);

        enemyIconImage.color = isFind ? Color.white : Color.black;

        background.color = GetRarityColor(rarity);
    }

    private Color GetRarityColor(int rarity)
    {
        if (rarity == 2)
        {
            return Color.red;
        }
        return Color.white;
    }
}
