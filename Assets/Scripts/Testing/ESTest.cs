using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESTest : MonoBehaviour
{
    [SerializeField] private GameObject testPrefab;


    void Update()
    {
        // クリックするとマウスの位置にプレファブを生成
        if (Input.GetMouseButtonDown(0))
        {
            var obj = Instantiate(testPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
        }

    }


}
