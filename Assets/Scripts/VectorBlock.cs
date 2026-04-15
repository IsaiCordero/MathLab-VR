using UnityEngine;
using TMPro;

public class VectorBlock : MonoBehaviour
{
    [Header("Text UI")]
    public TextMeshProUGUI valueText;

    [Header("Vector Data")]
    public Vector3 currentVector;

    [Header("Vector Area")]
    public Transform vectorCube;
    public Transform centerReference;

    [Header("Dynamic Arrow")]
    public Transform dynamicArrowRoot;
    public Transform dynamicArrowBody;
    public Transform dynamicArrowHead;

    [Header("Settings")]
    public float lerpSpeed = 10f;
    public float vectorScaleFactor = 10f;
    public float minArrowLength = 0.02f;
    public float arrowLengthMultiplier = 2f;

    [Header("Cable")]
    public DataCable incomingCable;

    private bool wasConnected = false;
    private Vector3 dynamicArrowBodyInitialScale;
    private Vector3 dynamicArrowHeadInitialLocalPosition;
    private Vector3 dynamicArrowHeadInitialScale;
    private Vector3 dynamicArrowBodyInitialLocalPosition;


    void Start()
    {
        if (dynamicArrowBody != null)
        {
            dynamicArrowBodyInitialScale = dynamicArrowBody.localScale;
            dynamicArrowBodyInitialLocalPosition = dynamicArrowBody.localPosition;
        }


        if (dynamicArrowHead != null)
        {
            dynamicArrowHeadInitialScale = dynamicArrowHead.localScale;
            dynamicArrowHeadInitialLocalPosition = dynamicArrowHead.localPosition;
        }

        UpdateVisuals();
    }

    void Update()
    {
        if (vectorCube == null || centerReference == null)
            return;

        if (incomingCable != null)
        {
            wasConnected = true;

            currentVector = incomingCable.GetVectorFromSource();

            Vector3 targetLocalOffset = currentVector / vectorScaleFactor;
            targetLocalOffset.x = -targetLocalOffset.x;

            vectorCube.localPosition = Vector3.Lerp(
                vectorCube.localPosition,
                centerReference.localPosition + targetLocalOffset,
                Time.deltaTime * lerpSpeed
            );
        }
        else
        {
            if (wasConnected)
            {
                ResetBlock();
            }

            Vector3 localOffset = vectorCube.localPosition - centerReference.localPosition;
            localOffset.x = -localOffset.x;
            currentVector = localOffset * vectorScaleFactor;
        }

        UpdateVisuals();
    }
    void LateUpdate()
    {
        UpdateDynamicArrow();
    }


    void ResetBlock()
    {
        if (vectorCube != null && centerReference != null)
        {
            vectorCube.localPosition = centerReference.localPosition;
        }

        currentVector = Vector3.zero;
        wasConnected = false;
    }

    void UpdateVisuals()
    {
        if (valueText != null)
        {
            string x = currentVector.x.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            string y = currentVector.y.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            string z = currentVector.z.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);

            valueText.text = $"({x}, {y}, {z})";
        }
    }

    void UpdateDynamicArrow()
    {
        if (dynamicArrowRoot == null || dynamicArrowBody == null || dynamicArrowHead == null)
            return;

        if (vectorCube == null || centerReference == null)
            return;

        Vector3 localOffset = vectorCube.localPosition - centerReference.localPosition;
        float magnitude = localOffset.magnitude;

        if (magnitude < 0.001f)
        {
            dynamicArrowRoot.gameObject.SetActive(false);
            return;
        }

        dynamicArrowRoot.gameObject.SetActive(true);

        // La flecha nace en el centro del área
        dynamicArrowRoot.localPosition = centerReference.localPosition;

        // Dirección hacia el cubito
        Vector3 direction = localOffset.normalized;

        // Como tu flecha base está montada "hacia arriba"
        dynamicArrowRoot.localRotation = Quaternion.FromToRotation(Vector3.up, direction);

        // Longitud visual según distancia al cubo
        float lengthFactor = Mathf.Max(0.001f, (magnitude + 0.1f) * arrowLengthMultiplier);
        float lengthFactorHead = Mathf.Max(0.001f, (magnitude + 0.5f) * arrowLengthMultiplier);

        dynamicArrowBody.localScale = new Vector3(
            dynamicArrowBodyInitialScale.x,
            dynamicArrowBodyInitialScale.y * lengthFactor,
            dynamicArrowBodyInitialScale.z
        );

        dynamicArrowBody.localPosition = new Vector3(
            dynamicArrowBodyInitialLocalPosition.x,
            dynamicArrowBodyInitialLocalPosition.y * lengthFactor,
            dynamicArrowBodyInitialLocalPosition.z
        );

        dynamicArrowHead.localScale = new Vector3(
            dynamicArrowHeadInitialScale.x,
            dynamicArrowHeadInitialScale.y * lengthFactorHead,
            dynamicArrowHeadInitialScale.z
        );
        dynamicArrowHead.localPosition = new Vector3(
            dynamicArrowHeadInitialLocalPosition.x,
            (dynamicArrowHeadInitialLocalPosition.y * lengthFactor) + 0.05f,
            dynamicArrowHeadInitialLocalPosition.z
        );


    }
}
