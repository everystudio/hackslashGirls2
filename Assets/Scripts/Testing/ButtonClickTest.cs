using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickTest : MonoBehaviour
{
    void Start()
    {
        //ボタンがクリックされた時の処理
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Button click!" + this.gameObject.name);
        });

    }

}
