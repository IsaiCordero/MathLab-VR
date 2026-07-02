using UnityEngine;
using Oculus.Interaction;

public abstract class FixedGrabbableButton : MonoBehaviour
{
    [Header("Button")]
    public Grabbable grabbableButton;

    private Vector3 fixedLocalPosition;
    private Quaternion fixedLocalRotation;

    protected virtual void Start()
    {
        fixedLocalPosition = transform.localPosition;
        fixedLocalRotation = transform.localRotation;

        if (grabbableButton != null)
        {
            grabbableButton.WhenPointerEventRaised += HandlePointerEvent;
        }
    }

    protected virtual void LateUpdate()
    {
        transform.localPosition = fixedLocalPosition;
        transform.localRotation = fixedLocalRotation;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select)
        {
            OnButtonPressed();
        }
    }

    protected abstract void OnButtonPressed();

    protected virtual void OnDestroy()
    {
        if (grabbableButton != null)
        {
            grabbableButton.WhenPointerEventRaised -= HandlePointerEvent;
        }
    }
}