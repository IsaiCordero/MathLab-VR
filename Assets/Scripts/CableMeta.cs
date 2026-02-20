using UnityEngine;
using Oculus.Interaction;

public class CableMeta : MonoBehaviour
{
    [Header("Components")]
    public LineRenderer lineRenderer;
    public Grabbable grabbableMeta; 
    
    [Header("Configuration")]
    public Transform blockOutPut; 
    public float distConn = 0.1f;
    public string tagIn = "InPut"; 

    [Header("References")]
    public Transform blockOriginal; 
    private Vector3 positionOriginal;
    private Quaternion rotationOriginal;
    private Transform destinyPort = null;

    void Start()
    {
        if (blockOriginal == null)
        {
            blockOriginal = transform.root; 
        }

        positionOriginal = transform.localPosition;
        rotationOriginal = transform.localRotation;

        grabbableMeta.WhenPointerEventRaised += EventsMeta;
    }

    private void OnDestroy()
    {
        if (grabbableMeta != null)
            grabbableMeta.WhenPointerEventRaised -= EventsMeta;

        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if(child.CompareTag(tagIn))
            {
                CableMeta plugConnected = child.GetComponentInChildren<CableMeta>();
                if(plugConnected != null)
                {
                    plugConnected.ResetPosition();
                }
            }
        }

        if(destinyPort != null)
        {
            ResetPosition();
        }
    }

    void Update()
    {
        if (blockOutPut != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, blockOutPut.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    private void EventsMeta(PointerEvent evento)
    {
        if (evento.Type == PointerEventType.Unselect)
        {
            TryConnection();
        }
        else if (evento.Type == PointerEventType.Select)
        {
            transform.SetParent(blockOriginal);
            destinyPort = null;
        }
    }

    void TryConnection()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, distConn);
        Transform InPutFound = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag(tagIn))
            {
                if (hit.transform.IsChildOf(blockOriginal))
                {
                    continue; 
                }

                InPutFound = hit.transform;
                break;
            }
        }

        if (InPutFound != null)
        {
            transform.position = InPutFound.position;
            transform.rotation = InPutFound.rotation;
            transform.SetParent(InPutFound); 
            destinyPort = InPutFound;
        }
        else
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        transform.SetParent(blockOriginal);
        transform.localPosition = positionOriginal;
        transform.localRotation = rotationOriginal;
        destinyPort = null;
    }

}