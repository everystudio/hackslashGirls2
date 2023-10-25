using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGachaTop : MonoBehaviour
{
    public List<UICharacterCore> uiCharacterCoreList = new List<UICharacterCore>();

    private void Start()
    {
        int index = 0;
        foreach (var uiCharacterCore in uiCharacterCoreList)
        {
            MasterChara masterChara = ModelManager.Instance.GetMasterChara(index + 1);

            UserChara userChara = new UserChara(masterChara);

            uiCharacterCore.Set(masterChara, userChara);
            index += 1;
        }



    }

}
