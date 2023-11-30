using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;

    private MasterHelp masterHelp;

    public void Initialize(MasterHelp masterHelp, UnityEngine.Events.UnityAction<MasterHelp> onClick)
    {
        this.masterHelp = masterHelp;
        this.text.text = masterHelp.title;
        button.onClick.AddListener(() =>
        {
            onClick(this.masterHelp);
        });
    }
}
