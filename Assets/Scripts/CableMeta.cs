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
    public string FirstInPut = "First InPut";
    public string SecondInPut = "Second InPut";
    public string InPut = "Input";

    [Header("Colors")]
    public Color colorDisconnected = Color.red;
    public Color colorConnected = Color.green;

    [Header("References")]
    public Transform blockOriginal;
    private Vector3 positionOriginal;
    private Quaternion rotationOriginal;
    private Transform destinyPort = null;

    public Transform DestinyPort => destinyPort;

    public int curveResolution = 10;
    private bool isConnected = false;

    private Vector3 lastStartPoint;
    private Vector3 lastEndPoint;
    private bool curveInitialized = false;

    void Start()
    {
        if (blockOriginal == null)
        {
            blockOriginal = transform.root;
        }

        positionOriginal = transform.localPosition;
        rotationOriginal = transform.localRotation;

        grabbableMeta.WhenPointerEventRaised += EventsMeta;

        UpdateCableColor(colorDisconnected);

        if (blockOutPut != null)
        {
            lastStartPoint = blockOutPut.position;
        }
        lastEndPoint = transform.position;
        curveInitialized = false;
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
            grabbableMeta.WhenPointerEventRaised -= EventsMeta;

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
        if (blockOutPut != null)
        {
            Vector3 currentStartPoint = blockOutPut.position;
            Vector3 currentEndPoint = transform.position;

            if (!curveInitialized || currentStartPoint != lastStartPoint || currentEndPoint != lastEndPoint)
            {
                DrawCurveBezier();
                lastStartPoint = currentStartPoint;
                lastEndPoint = currentEndPoint;
                curveInitialized = true;
            }
        }

        if (isConnected && destinyPort != null)
        {
            transform.position = destinyPort.position;

            Vector3 directionToTarget = destinyPort.parent.position - transform.position;

            if (directionToTarget.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(directionToTarget);
            }
        }


        if (grabbableMeta.SelectingPointsCount > 0 && !isConnected)
        {
            UpdateCableColor(colorDisconnected);
        }
    }

    void UpdateCableColor(Color targetColor)
    {
        lineRenderer.startColor = targetColor;
        lineRenderer.endColor = targetColor;

        if (lineRenderer.material.HasProperty("_EmissionColor"))
        {
            lineRenderer.material.SetColor("_EmissionColor", targetColor * 2.0f);
        }
    }

    void DrawCurveBezier()
    {
        lineRenderer.positionCount = curveResolution;

        Vector3 startPoint = blockOutPut.position;
        Vector3 endPoint = transform.position;

        Vector3 middlePoint = (startPoint + endPoint) / 2f;
        float distance = Vector3.Distance(startPoint, endPoint);
        middlePoint += blockOutPut.right * (distance * 0.3f);
        middlePoint.y -= (distance * 0.2f);

        for (int i = 0; i < curveResolution; i++)
        {
            float y = i / (float)(curveResolution - 1);
            Vector3 curvePosition = CalculateBezierPoint(y, startPoint, middlePoint, endPoint);
            lineRenderer.SetPosition(i, curvePosition);
        }
    }

    Vector3 CalculateBezierPoint(float y, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - y;
        float yy = y * y;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * y * p1;
        p += yy * p2;

        return p;
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
            UpdateCableColor(colorDisconnected);
            curveInitialized = false;
        }
    }

    void TryConnection()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, distConn);
        Transform InPutFound = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag(FirstInPut) || hit.CompareTag(SecondInPut) || hit.CompareTag(InPut))
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
            Quaternion previousWorldRotation = transform.rotation;

            transform.SetParent(null, true);
            transform.position = InPutFound.position;
            transform.rotation = previousWorldRotation;

            DataCable dc = GetComponent<DataCable>();
            bool connectedSuccessfully = false;
            if (dc != null)
            {
                 connectedSuccessfully = dc.ConnectToPort(InPutFound);
            }
            if (connectedSuccessfully)
            {
                destinyPort = InPutFound;
                isConnected = true;
                UpdateCableColor(colorConnected);
                curveInitialized = false;
            }
            else
            {
                ResetPosition();
            }
        }
        else
        {
            ResetPosition();
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
        UpdateCableColor(colorDisconnected);
        curveInitialized = false;
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
