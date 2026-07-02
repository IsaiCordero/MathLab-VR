using UnityEngine;
using TMPro;

public class NumberBlockView : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI valueText;

    public void UpdateValue(float value)
    {
        if (valueText == null) return;

        valueText.text = value.ToString("F2");
    }
}