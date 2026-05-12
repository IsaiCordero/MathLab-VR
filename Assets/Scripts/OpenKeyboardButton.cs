using UnityEngine;
using Oculus.Interaction;

public class OpenKeyboardButton : MonoBehaviour
{
    [Header("References")]
    public VectorBlock targetVectorBlock;
    public Grabbable grabbableButton;

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
        if (evt.Type == PointerEventType.Select && targetVectorBlock != null)
        {
            targetVectorBlock.OpenKeyboard();
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
