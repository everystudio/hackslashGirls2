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

    [SerializeField] private TextMeshProUGUI totalPercentText;
    [SerializeField] private Image completeImage;

    public MasterArea masterArea;

    public void Set(MasterArea masterArea)
    {
        this.masterArea = masterArea;
        areaName.text = masterArea.area_name;

        UserArea userArea = ModelManager.Instance.UserArea.List.Find(x => x.area_id == masterArea.area_id);

        achievementAreaText.text = userArea.PercentFloor().ToString() + "%";
        achievementCollectText.text = userArea.PercentItem().ToString() + "%";
        achievementEnemyText.text = userArea.PercentEnemy().ToString() + "%";


        if (userArea.TotalPercent() >= 100)
        {
            completeImage.gameObject.SetActive(true);
        }
        else
        {
            completeImage.gameObject.SetActive(false);
        }
        totalPercentText.text = userArea.TotalPercent().ToString() + "%";

    }



}
