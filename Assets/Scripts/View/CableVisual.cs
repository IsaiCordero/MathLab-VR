using UnityEngine;

public class CableVisual : MonoBehaviour
{
    [Header("Components")]
    public LineRenderer lineRenderer;
    public GameObject outputArrowVisual;

    [Header("Colors")]
    public Color colorDisconnected = Color.red;
    public Color colorConnected = Color.green;
    public float emissionIntensity = 4f;

    [Header("Curve")]
    public int curveResolution = 20;
    public float controlPointDistance = 0.4f;

    private Vector3 lastStartPoint;
    private Vector3 lastEndPoint;
    private bool curveInitialized = false;

    public void Initialize(Transform blockOutput)
    {
        SetDisconnected();

        if (blockOutput != null)
        {
            lastStartPoint = GetAttachPoint(blockOutput).position;
        }

        lastEndPoint = transform.position;
        curveInitialized = false;
    }

    public void RefreshCurve(Transform blockOutput, Transform destinyPort, Transform blockOriginal, bool isConnected)
    {
        if (lineRenderer == null || blockOutput == null) return;

        Vector3 currentStartPoint = GetAttachPoint(blockOutput).position;
        Vector3 currentEndPoint = transform.position;

        if (!curveInitialized || currentStartPoint != lastStartPoint || currentEndPoint != lastEndPoint)
        {
            DrawCurveBezier(blockOutput, destinyPort, blockOriginal, isConnected);

            lastStartPoint = currentStartPoint;
            lastEndPoint = currentEndPoint;
            curveInitialized = true;
        }
    }

    public void SetConnected()
    {
        SetCableColor(colorConnected);
        SetOutputArrowVisible(false);
        curveInitialized = false;
    }

    public void SetDisconnected()
    {
        SetCableColor(colorDisconnected);
        SetOutputArrowVisible(true);
        curveInitialized = false;
    }

    public void SetDragging()
    {
        SetCableColor(colorDisconnected);
    }

    void SetCableColor(Color targetColor)
    {
        if (lineRenderer == null) return;

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

    void SetOutputArrowVisible(bool visible)
    {
        if (outputArrowVisual != null)
        {
            outputArrowVisual.SetActive(visible);
        }
    }

    void DrawCurveBezier(Transform blockOutput, Transform destinyPort, Transform blockOriginal, bool isConnected)
    {
        int resolution = Mathf.Max(2, curveResolution);
        lineRenderer.positionCount = resolution;

        Transform startAttach = GetAttachPoint(blockOutput);

        Vector3 p0 = startAttach.position;
        Vector3 p3 = transform.position;

        float distance = Vector3.Distance(p0, p3);
        float controlDistance = Mathf.Max(0.05f, distance * controlPointDistance);

        Vector3 startDir = GetOutwardDirection(blockOutput, blockOutput, blockOriginal);
        Vector3 endDir = startDir * -1f;

        if (isConnected && destinyPort != null)
        {
            endDir = GetOutwardDirection(destinyPort, blockOutput, blockOriginal);
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

    Transform GetNodeRoot(Transform port, Transform blockOutput, Transform blockOriginal)
    {
        if (port == null) return null;

        if (port == blockOutput && blockOriginal != null)
        {
            return blockOriginal;
        }

        INodeInput nodeInput = port.GetComponentInParent<INodeInput>();

        if (nodeInput is MonoBehaviour inputBehaviour)
        {
            return inputBehaviour.transform;
        }

        return port.parent;
    }

    Vector3 GetOutwardDirection(Transform port, Transform blockOutput, Transform blockOriginal)
    {
        Transform root = GetNodeRoot(port, blockOutput, blockOriginal);

        if (root == null)
        {
            return port.right;
        }

        Vector3 localPortPosition = root.InverseTransformPoint(port.position);
        float side = localPortPosition.x >= 0f ? 1f : -1f;

        return root.TransformDirection(Vector3.right * side).normalized;
    }
}