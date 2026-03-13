using UnityEngine;
using TMPro;
using Oculus.Interaction;

public class NumberBlock : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI valueText;

    [Header("Settings")]
    public int currentValue = 0;
    public int step = 1;

    void Start()
    {
        UpdateVisuals();
    }

    public void Add()
    {
        currentValue += step;
        UpdateVisuals();
    }

    public void Subtract()
    {
        currentValue -= step;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if(valueText != null)
        {
            valueText.text = currentValue.ToString();
        }
    }
}
