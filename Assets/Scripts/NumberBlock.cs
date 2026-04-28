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

    [Header("Cable")]
    public DataCable incomingCable;

    void Start()
    {
        UpdateVisuals();
    }

    void Update()
    {
        if(incomingCable != null)
        {
            float result = incomingCable.GetValueFromSource();
            currentValue = Mathf.RoundToInt(result);
            Debug.Log("resultado =" + result);
            valueText.text = result.ToString("F1");
        }
    }

    public void Add()
    {
        if(incomingCable == null){
            currentValue += step;
            UpdateVisuals();
        }
    }

    public void Subtract()
    {
        if(incomingCable == null){
            currentValue -= step;
            UpdateVisuals();
        }
    }

    void UpdateVisuals()
    {
        if(valueText != null && incomingCable == null)
        {
            valueText.text = currentValue.ToString();
        }
    }

    public void SetValueFromSlider(int value)
    {
        if (incomingCable != null) return;

            currentValue = value;
            UpdateVisuals();
    }

}
