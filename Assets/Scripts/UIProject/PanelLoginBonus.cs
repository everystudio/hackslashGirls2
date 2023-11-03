using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using anogame;

public class PanelLoginBonus : Singleton<PanelLoginBonus>
{
    [SerializeField] private Transform root;
    [SerializeField] private Transform bannerParent;
    [SerializeField] private GameObject prizeTextPrefab;

    [SerializeField] private Button closeButton;

    public override void Initialize()
    {
        base.Initialize();
        closeButton.onClick.AddListener(() =>
        {
            root.gameObject.SetActive(false);

        });
    }

    public void Show(List<string> messageList)
    {
        root.gameObject.SetActive(true);

        // bannerParentの子供を全て削除
        foreach (Transform child in bannerParent)
        {
            if (prizeTextPrefab != child)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (var message in messageList)
        {
            var temp = Instantiate(prizeTextPrefab, bannerParent);
            temp.SetActive(true);

            temp.GetComponent<TextMeshProUGUI>().text = message;
        }

    }
}
