using UnityEngine;
using Oculus.Interaction;

public class CableMeta : MonoBehaviour
{
    [Header("Components")]
    public LineRenderer lineRenderer;
    public Grabbable grabbableMeta;
    public GameObject outputArrowVisual;

    [Header("Configuration")]
    public Transform blockOutPut;
    public float distConn = 0.1f;
    public string FirstInPut = "First InPut";
    public string SecondInPut = "Second InPut";
    public string InPut = "Input";

    [Header("Colors")]
    public Color colorDisconnected = Color.red;
    public Color colorConnected = Color.green;
    public float emissionIntensity = 4f;

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

    public int curveResolution = 20;

    public float curveTangentStrength = 0.35f;
    public float curveVerticalDrop = 0.15f;
    public float controlPointDistance = 0.4f;

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
        if (isConnected && destinyPort != null)
        {
            transform.position = destinyPort.position;

            Vector3 directionToTarget = destinyPort.parent.position - transform.position;

            if (directionToTarget.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(directionToTarget);
            }
        }
        if (blockOutPut != null)
        {
            Vector3 currentStartPoint = GetAttachPoint(blockOutPut).position;
            Vector3 currentEndPoint = transform.position;

            if (!curveInitialized || currentStartPoint != lastStartPoint || currentEndPoint != lastEndPoint)
            {
                DrawCurveBezier();
                lastStartPoint = currentStartPoint;
                lastEndPoint = currentEndPoint;
                curveInitialized = true;
            }
        }



        if (grabbableMeta.SelectingPointsCount > 0 && !isConnected)
        {
            UpdateCableColor(colorDisconnected);
        }
    }

    void UpdateCableColor(Color targetColor)
    {
        lineRenderer.startColor = targetColor * 1.5f;
        lineRenderer.endColor = targetColor * 1.5f;

        Material mat = lineRenderer.material;

        if (mat.HasProperty("_BaseColor"))
        {
            mat.SetColor("_BaseColor", targetColor);
        }
        if (mat.HasProperty("_Color"))
        {
            mat.SetColor("_Color", targetColor);
        }
        if (mat.HasProperty("_EmissionColor"))
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", targetColor * emissionIntensity);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void SetOutputArrowVisible(bool visible)
    {
        if (outputArrowVisual != null)
        {
            outputArrowVisual.SetActive(visible);
        }
    }

void DrawCurveBezier()
{
    int resolution = Mathf.Max(2, curveResolution);
    lineRenderer.positionCount = resolution;

    Transform startAttach = GetAttachPoint(blockOutPut);

    Vector3 p0 = startAttach.position;
    Vector3 p3 = transform.position;

    float distance = Vector3.Distance(p0, p3);
    float controlDistance = Mathf.Max(0.05f, distance * controlPointDistance);

    Vector3 startDir = GetOutwardDirection(blockOutPut);
    Vector3 endDir = startDir * -1f;

    if (isConnected && destinyPort != null)
    {
        endDir = GetOutwardDirection(destinyPort);
    }

    Vector3 p1 = p0 + startDir * controlDistance;
    Vector3 p2 = p3 + endDir * controlDistance;

    for (int i = 0; i < resolution; i++)
    {
        float t = i / (float)(resolution - 1);
        Vector3 curvePosition = CalculateCubicBezierPoint(t, p0, p1, p2, p3);
        lineRenderer.SetPosition(i, curvePosition);
    }
}

Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
{
    float u = 1f - t;
    float uu = u * u;
    float uuu = uu * u;
    float tt = t * t;
    float ttt = tt * t;

    Vector3 p = uuu * p0;
    p += 3f * uu * t * p1;
    p += 3f * u * tt * p2;
    p += ttt * p3;

    return p;
}

Transform GetAttachPoint(Transform port)
{
    if (port == null) return null;

    Transform attachPoint = port.Find("CableAttachPoint");

    if (attachPoint != null)
    {
        return attachPoint;
    }

    return port;
}

Transform GetNodeRoot(Transform port)
{
    if (port == null) return null;

    if (port == blockOutPut && blockOriginal != null)
    {
        return blockOriginal;
    }

    NumberBlock numberBlock = port.GetComponentInParent<NumberBlock>();
    if (numberBlock != null) return numberBlock.transform;

    VectorBlock vectorBlock = port.GetComponentInParent<VectorBlock>();
    if (vectorBlock != null) return vectorBlock.transform;

    TwoInputFunction twoInputFunction = port.GetComponentInParent<TwoInputFunction>();
    if (twoInputFunction != null) return twoInputFunction.transform;

    FunctionOneInput oneInputFunction = port.GetComponentInParent<FunctionOneInput>();
    if (oneInputFunction != null) return oneInputFunction.transform;

    OneInputNumberFunction oneInputNumberFunction = port.GetComponentInParent<OneInputNumberFunction>();
    if (oneInputNumberFunction != null) return oneInputNumberFunction.transform;

    return port.parent;
}

Vector3 GetOutwardDirection(Transform port)
{
    Transform root = GetNodeRoot(port);

    if (root == null)
    {
        return port.right;
    }

    Vector3 localPortPosition = root.InverseTransformPoint(port.position);
    float side = localPortPosition.x >= 0f ? 1f : -1f;

    return root.TransformDirection(Vector3.right * side).normalized;
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
            SetOutputArrowVisible(true);
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
                SetOutputArrowVisible(false);
                PlaySound(connectSound);
                curveInitialized = false;
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
        SetOutputArrowVisible(true);
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
