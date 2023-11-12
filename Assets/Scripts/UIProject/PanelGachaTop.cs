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
                // ガチャの種類増えたら対応する
                gachaResultList.Add(ModelManager.Instance.GetGachaResult(1));
            }
            ModelManager.Instance.UseTicket(count);

            foreach (var get_chara_id in gachaResultList)
            {
                var getUserChara = ModelManager.Instance.AddChara(get_chara_id);
                // すでに持っていた場合は追加失敗でnullが返ってくる
                if (getUserChara == null)
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
