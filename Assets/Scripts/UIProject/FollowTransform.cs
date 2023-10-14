using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] private Transform target; // 追従する対象
    [SerializeField] private Vector3 offset; // オフセット（World Spaceのオフセット）
    private RectTransform rectTransform;
    private Camera useCamera;

    public void SetTarget(Transform target, Vector3 offset, Camera useCamera)
    {
        this.useCamera = useCamera;
        this.target = target;
        this.offset = offset;
        rectTransform = GetComponent<RectTransform>();
        RefreshPosition();
    }
    public void SetTarget(Transform target, Camera useCamera)
    {
        SetTarget(target, Vector3.zero, useCamera);
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        //RefreshPosition();
    }

    private void RefreshPosition()
    {
        if (target)
        {
            // World PositionをScreen Positionに変換
            //Debug.Log(useCamera);

            //Debug.Log("target.position + offset" + (target.position + offset));
            //Debug.Log("useCamera.WorldToScreenPoint(target.position + offset)" + useCamera.WorldToScreenPoint(target.position + offset));
            Vector2 screenPos = useCamera.WorldToScreenPoint(target.position + offset);
            rectTransform.position = screenPos;
        }
    }
}
