using UnityEngine;

public class NumberSliderController : MonoBehaviour
{
    [Header("Slider")]
    public GameObject sliderRoot;
    public Transform sliderHandle;

    [Header("Movement")]
    public float maxLocalX = 0.3f;
    public float minLocalX = -0.3f;
    public float sliderMoveThreshold = 0.001f;

    [Header("Values")]
    public float maxSliderValue = 20f;
    public float minSliderValue = -20f;

    private float originalLocalY;
    private float originalLocalZ;
    private Quaternion originalLocalRotation;
    private float lastSliderLocalX;

    public void Initialize()
    {
        if (sliderHandle == null) return;

        originalLocalY = sliderHandle.localPosition.y;
        originalLocalZ = sliderHandle.localPosition.z;
        originalLocalRotation = sliderHandle.localRotation;
        lastSliderLocalX = Mathf.Clamp(sliderHandle.localPosition.x, minLocalX, maxLocalX);
    }

    public bool TryReadValue(out float value)
    {
        value = 0f;

        if (sliderRoot == null || sliderHandle == null || !sliderRoot.activeInHierarchy)
        {
            return false;
        }

        float localX = Mathf.Clamp(sliderHandle.localPosition.x, minLocalX, maxLocalX);

        sliderHandle.localPosition = new Vector3(localX, originalLocalY, originalLocalZ);
        sliderHandle.localRotation = originalLocalRotation;

        if (Mathf.Abs(localX - lastSliderLocalX) <= sliderMoveThreshold)
        {
            return false;
        }

        lastSliderLocalX = localX;

        float normalizedValue = Mathf.InverseLerp(minLocalX, maxLocalX, localX);
        float calculatedValue = Mathf.Lerp(minSliderValue, maxSliderValue, normalizedValue);

        value = -calculatedValue;
        return true;
    }

    public void SetValue(float value)
    {
        if (sliderHandle == null) return;

        float clampedSliderValue = Mathf.Clamp(-value, minSliderValue, maxSliderValue);
        float normalizedValue = Mathf.InverseLerp(minSliderValue, maxSliderValue, clampedSliderValue);
        float targetX = Mathf.Lerp(minLocalX, maxLocalX, normalizedValue);

        sliderHandle.localPosition = new Vector3(targetX, originalLocalY, originalLocalZ);
        sliderHandle.localRotation = originalLocalRotation;
        lastSliderLocalX = targetX;
    }

    public void SetVisible(bool visible)
    {
        if (sliderRoot != null)
        {
            sliderRoot.SetActive(visible);
        }
    }
}