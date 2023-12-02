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

    [SerializeField] private Button openProvideButton;
    [SerializeField] private Button closeProvideButton;
    [SerializeField] private GameObject providePanel;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private GachaProbBanner gachaProbBannerPrefab;

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

        openProvideButton.onClick.AddListener(() =>
        {
            providePanel.SetActive(true);
        });
        closeProvideButton.onClick.AddListener(() =>
        {
            providePanel.SetActive(false);
        });
        providePanel.SetActive(false);

        int gacha_id = 1;

        int totalProb = 0;
        foreach (var masterGacha in ModelManager.Instance.MasterGacha.List.FindAll(p => p.gacha_id == gacha_id))
        {
            totalProb += masterGacha.prob;
        }

        foreach (var masterGacha in ModelManager.Instance.MasterGacha.List)
        {
            var gachaProbBanner = Instantiate(gachaProbBannerPrefab, contentRoot);

            MasterChara masterChara = ModelManager.Instance.GetMasterChara(masterGacha.chara_id);
            float prob = (float)masterGacha.prob / totalProb * 100f;
            gachaProbBanner.Set($"{masterChara.chara_name}(☆{masterChara.initial_star})", prob);
        }


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
                // ガチャの種類増えたら対応する
                gachaResultList.Add(ModelManager.Instance.GetGachaResult(1));
            }
            ModelManager.Instance.UseTicket(count);

            foreach (var get_chara_id in gachaResultList)
            {
                var getUserChara = ModelManager.Instance.AddChara(get_chara_id, out bool isNew);
                // すでに持っていた場合は追加失敗でnullが返ってくる
                if (!isNew)
                {
                    ModelManager.Instance.AddStar(get_chara_id, 1);
                }
            }

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
