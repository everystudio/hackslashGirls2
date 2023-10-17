using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private Image bossHPGaugeImage;

    private EnemyBase enemyBase;

    public void Initialize(EnemyBase enemyBase)
    {
        this.enemyBase = enemyBase;
        bossNameText.text = enemyBase.masterEnemy.enemy_name;
        bossHPGaugeImage.fillAmount = 1.0f;

        enemyBase.OnChangeHealth.AddListener((health) =>
        {
            bossHPGaugeImage.fillAmount = (float)health / enemyBase.HealthMax;
        });

        enemyBase.OnDie.AddListener(() =>
        {
            gameObject.SetActive(false);
        });


    }



}
