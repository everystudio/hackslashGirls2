using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerArea : MonoBehaviour
{
    [SerializeField] private Image bannerImage;
    [SerializeField] private TextMeshProUGUI areaName;
    [SerializeField] private Button bannerButton;

    [SerializeField] private TextMeshProUGUI achievementAreaText;
    [SerializeField] private TextMeshProUGUI achievementCollectText;
    [SerializeField] private TextMeshProUGUI achievementEnemyText;

    [SerializeField] private Button areaDetailButton;
    public Button AreaDetailButton => areaDetailButton;

    public MasterArea masterArea;

    public void Set(MasterArea masterArea)
    {
        this.masterArea = masterArea;
        areaName.text = masterArea.area_name;
    }



}
