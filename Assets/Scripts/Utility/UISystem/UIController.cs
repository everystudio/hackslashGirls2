using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UIController>();
                instance.Initialize();
            }
            return instance;
        }
    }

    private static UIController instance;
    private bool initialized = false;
    public enum LAYER
    {
        MAIN_CONTENTS,
        SUB_CONTENTS,
        FLOATER,
        EFFECT,
        DIALOG,
        SYSTEM,
        ADD_SPACE,
        MAX
    }
    private List<RectTransform> layerRectTransforms = new();
    [SerializeField] private bool showDebug = false;
    public bool isStandby = false;

    [SerializeField] private Transform rootTransform;
    [SerializeField] private Transform testingTransform;

    public UnityEvent<string> OnUIEvent = new UnityEvent<string>();

    private Dictionary<string, GameObject> _panelDictionary = new();

    private RectTransform AddLayer(Transform parent, string layerName)
    {
        var ret = new GameObject().AddComponent<RectTransform>();
        ret.name = layerName;
        ret.SetParent(parent);
        ret.anchorMin = Vector2.zero;
        ret.anchorMax = Vector2.one;
        ret.sizeDelta = Vector2.zero;
        ret.offsetMin = Vector2.zero;
        ret.offsetMax = Vector2.zero;
        ret.localScale = Vector3.one;
        return ret;
    }

    private void Awake()
    {
        instance = this;
        Initialize();
    }

    private void Initialize()
    {
        if (initialized == true)
        {
            return;
        }
        for (int i = 0; i < (int)LAYER.MAX; i++)
        {
            layerRectTransforms.Add(AddLayer(rootTransform, ((LAYER)i).ToString()));
        }
        isStandby = true;
        if (testingTransform != null)
        {
            testingTransform.gameObject.SetActive(false);
        }
        initialized = true;
    }

    public GameObject AddPanel(string strName, Transform root)
    {
        GameObject prefab = Resources.Load(strName) as GameObject;
        if (prefab == null)
        {
            Debug.LogError("Not found prefab:" + strName);
            return null;
        }
        prefab.SetActive(true);

        GameObject addPanel = Instantiate(prefab, root);

        if (addPanel.GetComponent<CanvasGroup>() == null)
        {
            addPanel.AddComponent<CanvasGroup>();
        }

        foreach (Button btn in addPanel.GetComponentsInChildren<Button>())
        {
            btn.onClick.AddListener(() =>
            {
                if (showDebug)
                {
                    Debug.Log(btn.name);
                }
                OnUIEvent.Invoke(btn.name);
            });
        }

        UIPanel uiPanel = addPanel.GetComponent<UIPanel>();
        uiPanel?.Initialize();

        Debug.Log(strName);
        _panelDictionary.Add(strName, addPanel);
        return addPanel;
    }


    public GameObject AddPanel(string strName, LAYER layer)
    {
        return AddPanel(strName, layerRectTransforms[(int)layer]);
    }

    public GameObject AddPanel(string strName)
    {
        return AddPanel(strName, LAYER.MAIN_CONTENTS);
    }

    public void RemovePanel(GameObject panelGameObject)
    {
        string removePanelName = "";
        foreach (var kvs in _panelDictionary)
        {
            if (kvs.Value == panelGameObject)
            {
                removePanelName = kvs.Key;
                if (kvs.Value.TryGetComponent(out UIPanel uiPanel))
                {
                    uiPanel?.Shutdown();
                }
                Destroy(kvs.Value);
                break;
            }
        }
        if (0 < removePanelName.Length)
        {
            _panelDictionary.Remove(removePanelName);
        }
    }

    public void ClearPanels()
    {
        foreach (var kvs in _panelDictionary)
        {
            if (kvs.Value.TryGetComponent(out UIPanel uiPanel))
            {
                uiPanel?.Shutdown();
            }
            Destroy(kvs.Value);
        }
        _panelDictionary.Clear();
    }

}
