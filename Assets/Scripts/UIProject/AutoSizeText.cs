using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class AutoSizeText : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    private RectTransform rectTransform;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnValidate()
    {
        //var textGenerator = new TextGenerator();
        //var textGenerationSettings = textComponent.GetGenerationSettings(rectTransform.rect.size);

    }
    private void Update()
    {
        var textInfo = textComponent.GetTextInfo(textComponent.text);

        var width = textInfo.textComponent.preferredWidth < 580 ? textInfo.textComponent.preferredWidth : 580;
        var height = textInfo.textComponent.preferredHeight;// textGenerator.GetPreferredHeight(textComponent.text, textGenerationSettings);

        rectTransform.sizeDelta = new Vector2(width, height);

    }
}