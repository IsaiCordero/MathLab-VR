using UnityEngine;

public class VectorArrowView : MonoBehaviour
{
    [Header("Dynamic Arrow")]
    public Transform dynamicArrowRoot;
    public Transform dynamicArrowBody;
    public Transform dynamicArrowHead;

    [Header("Settings")]
    public float arrowLengthMultiplier = 2f;

    private Vector3 dynamicArrowBodyInitialScale;
    private Vector3 dynamicArrowHeadInitialLocalPosition;
    private Vector3 dynamicArrowHeadInitialScale;
    private Vector3 dynamicArrowBodyInitialLocalPosition;

    private bool arrowWasVisible = false;

    void Awake()
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
    }

    public void UpdateArrow(Vector3 localOffset, Vector3 originLocalPosition)
    {
        if (dynamicArrowRoot == null || dynamicArrowBody == null || dynamicArrowHead == null)
        {
            return;
        }

        float magnitude = localOffset.magnitude;

        if (magnitude < 0.001f)
        {
            Hide();
            return;
        }

        dynamicArrowRoot.gameObject.SetActive(true);
        arrowWasVisible = true;

        dynamicArrowRoot.localPosition = originLocalPosition;

        Vector3 direction = localOffset.normalized;
        dynamicArrowRoot.localRotation = Quaternion.FromToRotation(Vector3.up, direction);

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

    public void Hide()
    {
        if (dynamicArrowRoot != null)
        {
            dynamicArrowRoot.gameObject.SetActive(false);
        }

        arrowWasVisible = false;
    }

    public bool ShouldRefreshWhenHidden()
    {
        return !arrowWasVisible && dynamicArrowRoot != null;
    }
}