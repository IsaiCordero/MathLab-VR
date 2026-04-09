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
    private Renderer joystickRenderer;
    private Rigidbody joystickRb;

    [Header("Settings & Colors")]
    public Color manualColor = Color.white;
    public Color autoColor = Color.blue;
    public float lerpSpeed = 10f; 

    [Header("Cable")]
    public DataCable incomingCable;

    private bool wasConnected = false;

    [Header("Arrow Visual")]
    public Transform arrowRoot;
    public Transform arrowModel;
    public float arrowHeightOffset = 0.15f;
    public float arrowLengthMultiplier = 1f;
    public float minArrowLength = 0.05f;

    private Vector3 arrowInitialScale;
    void Start()
    {
        if (joystickObject != null)
        {
            joystickRb = joystickObject.GetComponent<Rigidbody>();
            joystickRenderer = joystickObject.GetComponent<Renderer>();
            
            Vector3 initialPosition = joystickObject.position;
            Quaternion initialRotation = joystickObject.rotation;
            joystickObject.SetParent(null);
            joystickObject.position = initialPosition;
            joystickObject.rotation = initialRotation;

            var grabbable = joystickObject.GetComponent<Grabbable>();
            if (grabbable != null) grabbable.WhenPointerEventRaised += OnPointerEvent;
        }
        if (arrowModel != null)
        {
            arrowInitialScale = arrowModel.localScale;
        }
        // Configuración del Centro de Referencia
        if (centerReference == null)
        {
            GameObject refGlobal = GameObject.Find("Center"); 
            if (refGlobal != null)
            {
                centerReference = refGlobal.transform;
            }
            else
            {
                GameObject go = new GameObject("LocalCenterRef_" + name);
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
            wasConnected = true;
            
            currentVector = incomingCable.GetVectorFromSource();
            
            if (joystickObject != null && centerReference != null)
            {
                Vector3 targetWorldPos = centerReference.position + currentVector;
                joystickObject.position = Vector3.Lerp(joystickObject.position, targetWorldPos, Time.deltaTime * lerpSpeed);
                
                if (joystickRb != null) joystickRb.isKinematic = true;
            }
            
            if (joystickRenderer != null) joystickRenderer.material.color = autoColor;
        }
        else
        {
            if (wasConnected) ResetBlock();

            if (joystickObject != null && centerReference != null)
            {
                currentVector = joystickObject.position - centerReference.position;
                
                if (joystickRenderer != null) joystickRenderer.material.color = manualColor;
            }
        }

        UpdateVisuals();
        UpdateArrowVisual();
    }

    void ResetBlock()
    {
        if (joystickObject != null && centerReference != null)
        {
            joystickObject.position = centerReference.position;
            if (joystickRb != null) joystickRb.isKinematic = true;
        }
        currentVector = Vector3.zero;
        wasConnected = false;
    }

    private void OnPointerEvent(PointerEvent evt)
    {
        if (joystickRb == null || incomingCable != null) return;

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

    void UpdateArrowVisual()
    {
        if (arrowRoot == null || arrowModel == null || joystickObject == null || centerReference == null) return;

        Vector3 visualVector = joystickObject.position - centerReference.position;
        float magnitude = visualVector.magnitude;

        arrowRoot.position = centerReference.position + Vector3.up * arrowHeightOffset;

        if (magnitude < 0.001f)
        {
            arrowModel.gameObject.SetActive(false);
            return;
        }

        arrowModel.gameObject.SetActive(true);

        Vector3 direction = visualVector.normalized;
        arrowRoot.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        float visualLength = Mathf.Max(minArrowLength, magnitude * arrowLengthMultiplier);

        arrowModel.localScale = new Vector3(
            arrowInitialScale.x,
            arrowInitialScale.y * visualLength,
            arrowInitialScale.z
        );
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