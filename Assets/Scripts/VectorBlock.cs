using UnityEngine;
using TMPro;
using System.Globalization;
using Oculus.Interaction; 

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

    private Rigidbody joystickRb;

    void Start()
    {
        if (joystickObject != null)
        {
            Vector3 initialPosition = joystickObject.position;
            Quaternion initialRotation = joystickObject.rotation;

            joystickObject.SetParent(null);
            joystickObject.position = initialPosition;
            joystickObject.rotation = initialRotation;

            joystickRb = joystickObject.GetComponent<Rigidbody>();
            
            var grabbable = joystickObject.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                grabbable.WhenPointerEventRaised += OnPointerEvent;
            }
        }

        if (centerReference == null)
        {
            GameObject refGlobal = GameObject.Find("Center"); 
            if (refGlobal != null)
            {
                centerReference = refGlobal.transform;
            }
            else
            {
                GameObject go = new GameObject("CenterRef");
                centerReference = go.transform;
                centerReference.SetParent(this.transform);
                centerReference.localPosition = Vector3.zero; 
            }
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
            currentVector = joystickObject.position - centerReference.position;
            
            UpdateVisuals();
        }
    }

    private void OnPointerEvent(PointerEvent evt)
    {
        if (joystickRb == null) return;

        if (evt.Type == PointerEventType.Unselect)
        {
            joystickRb.isKinematic = true;
            joystickRb.linearVelocity = Vector3.zero;
            joystickRb.angularVelocity = Vector3.zero;
        }
        else if (evt.Type == PointerEventType.Select)
        {
            joystickRb.isKinematic = false;
        }
    }

    void UpdateVisuals()
    {
        if (valueText != null)
        {   
            string x = currentVector.x.ToString("F1", CultureInfo.InvariantCulture);
            string y = currentVector.y.ToString("F1", CultureInfo.InvariantCulture);
            string z = currentVector.z.ToString("F1", CultureInfo.InvariantCulture);

            valueText.text = $"({x}, {y}, {z})";
        }
    }

    private void OnDestroy()
    {
        if (joystickObject != null)
        {
            var grabbable = joystickObject.GetComponent<Grabbable>();
            if (grabbable != null) grabbable.WhenPointerEventRaised -= OnPointerEvent;
        }
    }
}