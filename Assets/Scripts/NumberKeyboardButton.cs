using UnityEngine;
using Oculus.Interaction;

public class NumberKeyboardButton : MonoBehaviour
{
    [Header("References")]
    public NumberKeyboardPanel numberKeyboardPanel;
    public Grabbable grabbableButton;

    [Header("Button Action")]
    public string buttonValue;

    private Vector3 fixedLocalPosition;
    private Quaternion fixedLocalRotation;

    void Start()
    {
        fixedLocalPosition = transform.localPosition;
        fixedLocalRotation = transform.localRotation;

        if (grabbableButton != null)
        {
            grabbableButton.WhenPointerEventRaised += HandlePointerEvent;
        }
    }

    void LateUpdate()
    {
        transform.localPosition = fixedLocalPosition;
        transform.localRotation = fixedLocalRotation;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select && numberKeyboardPanel != null)
        {
            numberKeyboardPanel.HandleButtonPress(buttonValue);
        }
    }

    private void OnDestroy()
    {
        if (grabbableButton != null)
        {
            grabbableButton.WhenPointerEventRaised -= HandlePointerEvent;
        }
    }
}