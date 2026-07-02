using UnityEngine;
using Oculus.Interaction;

public class CableMeta : MonoBehaviour
{
    [Header("Components")]
    public Grabbable grabbableMeta;
    public CableVisual cableVisual;

    [Header("Configuration")]
    public Transform blockOutPut;
    public float distConn = 0.1f;
    public string FirstInPut = "First InPut";
    public string SecondInPut = "Second InPut";
    public string InPut = "Input";

    [Header("References")]
    public Transform blockOriginal;
    private Vector3 positionOriginal;
    private Quaternion rotationOriginal;
    private Transform destinyPort = null;

    [Header("Audio Feedback")]
    public AudioSource audioSource;
    public AudioClip connectSound;
    public AudioClip failSound;

    public Transform DestinyPort => destinyPort;

    private bool isConnected = false;

    void Start()
    {
        if (blockOriginal == null)
        {
            blockOriginal = transform.root;
        }

        if (cableVisual == null)
        {
            cableVisual = GetComponent<CableVisual>();
        }

        positionOriginal = transform.localPosition;
        rotationOriginal = transform.localRotation;

        if (grabbableMeta != null)
        {
            grabbableMeta.WhenPointerEventRaised += EventsMeta;
        }

        if (cableVisual != null)
        {
            cableVisual.Initialize(blockOutPut);
        }
    }

    void LateUpdate()
    {
        if (!isConnected)
        {
            transform.localRotation = rotationOriginal;
        }
    }

    private void OnDestroy()
    {
        if (grabbableMeta != null)
        {
            grabbableMeta.WhenPointerEventRaised -= EventsMeta;
        }

        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag(FirstInPut) || child.CompareTag(SecondInPut) || child.CompareTag(InPut))
            {
                CableMeta plugConnected = child.GetComponentInChildren<CableMeta>();
                if (plugConnected != null)
                {
                    plugConnected.ResetPosition();
                }
            }
        }

        if (destinyPort != null)
        {
            ResetPosition();
        }
    }

    void Update()
    {
        if (isConnected && destinyPort != null)
        {
            transform.position = destinyPort.position;

            Vector3 directionToTarget = destinyPort.parent.position - transform.position;

            if (directionToTarget.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(directionToTarget);
            }
        }

        if (cableVisual != null)
        {
            cableVisual.RefreshCurve(blockOutPut, destinyPort, blockOriginal, isConnected);
        }

        if (grabbableMeta.SelectingPointsCount > 0 && !isConnected)
        {
            if (cableVisual != null)
            {
                cableVisual.SetDragging();
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
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
            DataCable dc = GetComponent<DataCable>();
            if (dc != null)
            {
                dc.DisconnectFromPort();
            }

            transform.SetParent(blockOriginal);
            destinyPort = null;
            isConnected = false;

            if (cableVisual != null)
            {
                cableVisual.SetDisconnected();
            }
        }
    }

    void TryConnection()
    {
        Transform inputFound = FindClosestInputPort();

        if (inputFound != null)
        {
            Quaternion previousWorldRotation = transform.rotation;

            transform.SetParent(null, true);
            transform.position = inputFound.position;
            transform.rotation = previousWorldRotation;

            DataCable dc = GetComponent<DataCable>();
            bool connectedSuccessfully = false;

            if (dc != null)
            {
                connectedSuccessfully = dc.ConnectToPort(inputFound);
            }

            if (connectedSuccessfully)
            {
                destinyPort = inputFound;
                isConnected = true;

                if (cableVisual != null)
                {
                    cableVisual.SetConnected();
                }

                PlaySound(connectSound);
            }
            else
            {
                PlaySound(failSound);
                ResetPosition();
            }
        }
        else
        {
            ResetPosition();
        }
    }

    Transform FindClosestInputPort()
    {
        Transform closestPort = null;
        float closestDistance = distConn * distConn;

        FindClosestInputPortByTag(FirstInPut, ref closestPort, ref closestDistance);
        FindClosestInputPortByTag(SecondInPut, ref closestPort, ref closestDistance);
        FindClosestInputPortByTag(InPut, ref closestPort, ref closestDistance);

        return closestPort;
    }

    void FindClosestInputPortByTag(string tagName, ref Transform closestPort, ref float closestDistance)
    {
        if (string.IsNullOrEmpty(tagName)) return;

        GameObject[] ports = GameObject.FindGameObjectsWithTag(tagName);

        foreach (GameObject portObject in ports)
        {
            Transform port = portObject.transform;

            if (port.IsChildOf(blockOriginal))
            {
                continue;
            }

            float distance = (port.position - transform.position).sqrMagnitude;

            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestPort = port;
            }
        }
    }

    public void ResetPosition()
    {
        DataCable dc = GetComponent<DataCable>();

        if (dc != null)
        {
            dc.DisconnectFromPort();
        }

        transform.SetParent(blockOriginal);
        transform.localPosition = positionOriginal;
        transform.localRotation = rotationOriginal;

        destinyPort = null;
        isConnected = false;

        if (cableVisual != null)
        {
            cableVisual.SetDisconnected();
        }
    }

    public void Disconnect()
    {
        DataCable dc = GetComponent<DataCable>();

        if (dc != null)
        {
            dc.DisconnectFromPort();
            dc.sourceObject = null;
        }

        ResetPosition();
    }
}