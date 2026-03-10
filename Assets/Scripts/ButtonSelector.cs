using UnityEngine;
using Oculus.Interaction;

public class ButtonSelector: MonoBehaviour
{
    [Header("References")]
    public SelectFunction scriptLogic;

    private Grabbable grabbable;

    void Start()
    {
        grabbable = GetComponent<Grabbable>();

        if(grabbable != null)
        {
            grabbable.WhenPointerEventRaised += HandlePointerEvent;
        }
    }

    private void HandlePointerEvent(PointerEvent evt){
        Debug.Log("Evento detectado: " + evt.Type);
        if(evt.Type == PointerEventType.Select)
        {
            if(scriptLogic != null)
            {
                scriptLogic.ChangeNextFunction();
            }
        }
    }

    private void OnDestroy()
    {
        if(grabbable != null)
        {
            grabbable.WhenPointerEventRaised -= HandlePointerEvent;
        }
    }
}
