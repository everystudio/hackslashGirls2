using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlyingText : MonoBehaviour
{
    [SerializeField] private TextMeshPro damageText;

    public void Initialize(int damage)
    {
        damageText.text = damage.ToString();
    }
    public void Initialize(string msg)
    {
        damageText.text = msg;
    }

    public static FlyingText Create(Transform parent, string msg, Vector3 targetPosition, float offsetRange = 0.5f)
    {
        var obj = Instantiate(Resources.Load<FlyingText>("FlyingText"), parent);

        Vector3 randomOffsetXY = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-offsetRange, offsetRange), 0f);
        obj.transform.position = targetPosition + new Vector3(0, 0, -1f) + randomOffsetXY;

        obj.Initialize(msg);
        Destroy(obj.gameObject, 5f);
        return obj;
    }
}
