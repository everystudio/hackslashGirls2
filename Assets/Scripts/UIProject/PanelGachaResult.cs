using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelGachaResult : MonoBehaviour
{
    public List<UICharacterCore> uiCharacterCoreList = new List<UICharacterCore>();

    [SerializeField] private Button closeButton;

    public UnityEvent OnClose = new UnityEvent();

    private List<int> gachaResultList;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            OnClose.Invoke();
        });
    }

    public void Initialize(List<int> gachaResultList)
    {
        this.gachaResultList = gachaResultList;
        closeButton.interactable = false;
        for (int i = 0; i < uiCharacterCoreList.Count; i++)
        {
            uiCharacterCoreList[i].gameObject.SetActive(i < gachaResultList.Count);
            uiCharacterCoreList[i].Clear();
        }

        StartCoroutine(Open(gachaResultList));

        /*
        foreach (var uiCharacterCore in uiCharacterCoreList)
        {
            MasterChara masterChara = ModelManager.Instance.GetMasterChara(index + 1);

            UserChara userChara = new UserChara(masterChara);

            uiCharacterCore.Set(masterChara, userChara);
            index += 1;
        }
        */
    }

    private IEnumerator Open(List<int> gachaResultList)
    {
        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < gachaResultList.Count; i++)
        {
            int charaId = gachaResultList[i];
            uiCharacterCoreList[i].Set(ModelManager.Instance.GetMasterChara(charaId), new UserChara(ModelManager.Instance.GetMasterChara(charaId)));
            yield return new WaitForSeconds(0.5f);
        }

        closeButton.interactable = true;

    }

}
