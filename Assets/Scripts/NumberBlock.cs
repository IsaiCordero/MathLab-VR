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
            Debug.Log("resultado =" + result);
            valueText.text = result.ToString("F1");
        }
        else
        {
            if (Time.frameCount % 100 == 0)
            {
                Debug.Log("NO HAY CABLE CONNECTED");
            }
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
}
