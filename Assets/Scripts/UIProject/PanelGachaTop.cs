using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelGachaTop : MonoBehaviour
{
    [SerializeField] private Button singleButton;
    [SerializeField] private Button multiButton;

    [SerializeField] private TextMeshProUGUI multiButtonText;

    [SerializeField] private PanelGachaKakunin panelKakunin;
    [SerializeField] private PanelGachaResult panelGachaResult;

    private void Start()
    {
        panelKakunin.gameObject.SetActive(false);
        panelGachaResult.gameObject.SetActive(false);
        /*
        int index = 0;
        foreach (var uiCharacterCore in uiCharacterCoreList)
        {
            MasterChara masterChara = ModelManager.Instance.GetMasterChara(index + 1);

            UserChara userChara = new UserChara(masterChara);

            uiCharacterCore.Set(masterChara, userChara);
            index += 1;
        }
        */


        Redraw();

        panelKakunin.OnCancel.AddListener(() =>
        {
            panelKakunin.gameObject.SetActive(false);
        });

        panelKakunin.OnOK.AddListener((count) =>
        {
            panelKakunin.gameObject.SetActive(false);

            Debug.Log($"count:{count}");

            panelGachaResult.gameObject.SetActive(true);

            List<int> gachaResultList = new List<int>();
            for (int i = 0; i < count; i++)
            {
                gachaResultList.Add(Random.Range(1, 5));
            }
            ModelManager.Instance.UseTicket(count);

            panelGachaResult.Initialize(gachaResultList);


        });

        panelGachaResult.OnClose.AddListener(() =>
        {
            panelGachaResult.gameObject.SetActive(false);
            Redraw();
        });


    }

    public void Redraw()
    {
        singleButton.interactable = 0 < ModelManager.Instance.UserGameData.ticket;

        int multiCount = Mathf.Min(10, ModelManager.Instance.UserGameData.ticket);

        multiButtonText.text = $"{multiCount}回連";
        multiButton.interactable = 0 < multiCount;
        multiButton.gameObject.SetActive(2 <= ModelManager.Instance.UserGameData.ticket);

        singleButton.onClick.RemoveAllListeners();
        multiButton.onClick.RemoveAllListeners();

        singleButton.onClick.AddListener(() =>
        {
            if (ModelManager.Instance.UserGameData.ticket < 1)
            {
                return;
            }
            panelKakunin.gameObject.SetActive(true);
            panelKakunin.Initialize(1);
        });

        multiButton.onClick.AddListener(() =>
        {
            if (ModelManager.Instance.UserGameData.ticket < 2)
            {
                return;
            }
            panelKakunin.gameObject.SetActive(true);
            panelKakunin.Initialize(multiCount);
        });

    }

}
