using UnityEngine;
using TMPro;

public class VectorBlock : MonoBehaviour
{
    [Header("Text UI")]
    public TextMeshProUGUI valueText;

    [Header("Vector Data")]
    public Vector3 currentVector; 

    [Header("Joystick Reference")]
    public Transform joystickObject;
    public Transform centerReference; 

    [Header("Cable")]
    public DataCable incomingCable;

    void Start()
    {
        if (centerReference == null && joystickObject != null)
        {
            centerReference = new GameObject("CenterRef").transform;
            centerReference.SetParent(this.transform);
            centerReference.position = joystickObject.position;
        }
    }

    void Update()
    {
        if (incomingCable != null)
        {
            float result = incomingCable.GetValueFromSource();
            valueText.text = result.ToString("F1");
        }
        else if (joystickObject != null && centerReference != null)
        {
            currentVector = joystickObject.localPosition - centerReference.localPosition;
            
            UpdateVisuals();
        }
    }

    void UpdateVisuals()
    {
        if (valueText != null)
        {
            valueText.text = $"X: {currentVector.x:F1}\nY: {currentVector.y:F1}\nZ: {currentVector.z:F1}";
        }
    }
}