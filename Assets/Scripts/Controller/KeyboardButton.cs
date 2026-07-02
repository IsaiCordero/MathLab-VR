using UnityEngine;
using Oculus.Interaction;

public class KeyboardButton : MonoBehaviour
{
    [Header("References")]
    public VectorKeyboardPanel keyboardPanel;
    public Grabbable grabbableButton;

    [Header("Button Action")]
    public string buttonValue;

    public NumberKeyboardPanel numberKeyboardPanel;

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
        if (evt.Type == PointerEventType.Select)
        {
            if (keyboardPanel != null)
            {
                keyboardPanel.HandleButtonPress(buttonValue);
            }
            else if (numberKeyboardPanel != null)
            {
                numberKeyboardPanel.HandleButtonPress(buttonValue);
            }
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
