using UnityEngine;
using Oculus.Interaction;

public class ButtonSelector : MonoBehaviour
{
    [Header("References")]
    public SelectFunction scriptLogic;

    private Grabbable grabbable;
    
    private Vector3 fixedLocalPos;
    private Quaternion fixedLocalRot;

    void Start()
    {
        fixedLocalPos = transform.localPosition;
        fixedLocalRot = transform.localRotation;
        
        grabbable = GetComponent<Grabbable>();

        if (grabbable != null)
        {
            grabbable.WhenPointerEventRaised += HandlePointerEvent;
        }
    }

    void LateUpdate()
    {
        transform.localPosition = fixedLocalPos;
        transform.localRotation = fixedLocalRot;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select)
        {
            if (scriptLogic != null)
            {
                scriptLogic.ChangeNextFunction();
            }
        }
    }

    private void OnDestroy()
    {
        if (grabbable != null)
            grabbable.WhenPointerEventRaised -= HandlePointerEvent;
    }
}