using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpDetail : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button buttonClose;

    private void Awake()
    {
        buttonClose.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void Initialize(MasterHelp masterHelp)
    {
        gameObject.SetActive(true);
        title.text = masterHelp.title;
        description.text = masterHelp.description;

    }

}
