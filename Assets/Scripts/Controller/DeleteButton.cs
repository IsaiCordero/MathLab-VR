using UnityEngine;
using Oculus.Interaction;

public class DeleteButton : MonoBehaviour
{
    [Header("References")]
    public CableMeta plugConnection; 
    public Grabbable grabbableButton;      

    private Vector3 fixedPosition;
    private Quaternion fixedRotation;

    void Start()
    {
        fixedPosition = transform.localPosition;
        fixedRotation = transform.localRotation;

        if (grabbableButton != null)
        {
            grabbableButton.WhenPointerEventRaised += EventsButton;
        }
    }

    private void OnDestroy()
    {
        if (grabbableButton != null)
            grabbableButton.WhenPointerEventRaised -= EventsButton;
    }

    private void EventsButton(PointerEvent evento)
    {
        if (evento.Type == PointerEventType.Select)
        {
            DeleteSecurity();
        }
    }

    void Update()
    {
        transform.localPosition = fixedPosition;
        transform.localRotation = fixedRotation;
    }

    void DeleteSecurity()
    {
        if (plugConnection == null) return;

        Transform rootBlock = plugConnection.blockOriginal;

        CableMeta[] allCables = FindObjectsOfType<CableMeta>();
        foreach (CableMeta c in allCables)
        {
            bool cableBelongsToDeletedBlock = c.transform.IsChildOf(rootBlock);
            bool cableConnectedToDeletedBlock = c.DestinyPort != null && c.DestinyPort.IsChildOf(rootBlock);

            if (cableBelongsToDeletedBlock || cableConnectedToDeletedBlock)
            {
                c.ResetPosition();
            }
        }

        plugConnection.ResetPosition();

        Destroy(rootBlock.gameObject);
    }
}
