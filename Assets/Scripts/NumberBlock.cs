using UnityEngine;
using TMPro;
using Oculus.Interaction;

public class NumberBlock : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI valueText;

    [Header("Settings")]
    public float currentValue = 0;

    [Header("Cable")]
    public DataCable incomingCable;

    [Header("Keyboard")]
    public GameObject keyboardPanelPrefab;
    public Transform keyboardSpawnPoint;
    public Vector3 keyboardSpawnScale = Vector3.one * 0.15f;

    [Header("Input Controls")]
    public GameObject editButton;
    private bool lastHadIncomingCable;

    [Header("Slider")]
    public GameObject Slider;
    public Transform sliderHandle;
    public float maxLocalX = 0.3f;
    public float minLocalX = -0.3f;
    private int maxSliderValue = 20;
    private int minSliderValue = -20;
    private float originalLocalY;
    private float originalLocalZ;
    private Quaternion originalLocalRotation;
    private float lastSliderLocalX;
    public float sliderMoveThreshold = 0.001f;



private GameObject currentKeyboardInstance;

    void Start()
    {
        UpdateVisuals();

        lastHadIncomingCable = incomingCable != null;
        SetInputMode(!lastHadIncomingCable);

        if(sliderHandle != null)
        {
            originalLocalY = sliderHandle.localPosition.y;
            originalLocalZ = sliderHandle.localPosition.z;
            originalLocalRotation = sliderHandle.localRotation;
            lastSliderLocalX = Mathf.Clamp(sliderHandle.localPosition.x, minLocalX, maxLocalX);
        }
    }

    void Update()
    {
        if(incomingCable != null)
        {
            float result = incomingCable.GetValueFromSource();
            currentValue = result;
            valueText.text = result.ToString("F1");
        }
        else if (Slider != null && sliderHandle != null && Slider.activeInHierarchy)
        {
            float localX = Mathf.Clamp(sliderHandle.localPosition.x, minLocalX, maxLocalX);
            sliderHandle.localPosition = new Vector3(localX, originalLocalY, originalLocalZ);
            sliderHandle.localRotation = originalLocalRotation;
            if(Mathf.Abs(localX - lastSliderLocalX) > sliderMoveThreshold){
                float normalizedValue = Mathf.InverseLerp(minLocalX, maxLocalX, localX);
                float calculatedValue = Mathf.Lerp(minSliderValue, maxSliderValue, normalizedValue);

                currentValue = -calculatedValue;
                UpdateVisuals();
            }
        }

        bool hasIncomingCable = incomingCable != null;

        if (hasIncomingCable != lastHadIncomingCable)
        {
            SetInputMode(!hasIncomingCable);

            if (hasIncomingCable)
            {
                CloseKeyboard();
            }

            lastHadIncomingCable = hasIncomingCable;
        }
    }

    void UpdateVisuals()
    {
        if(valueText != null && incomingCable == null)
        {
            valueText.text = currentValue.ToString("F1");
        }
    }

    public void SetValueFromSlider(int value)
    {
        if (incomingCable != null) return;

            currentValue = value;
            UpdateVisuals();
    }

    public void SetValueFromKeyboard(float value)
    {
        if (incomingCable != null) return;

        currentValue = value;

        if (sliderHandle != null)
        {
            float clampedSliderValue = Mathf.Clamp(-currentValue, minSliderValue, maxSliderValue);
            float normalizedValue = Mathf.InverseLerp(minSliderValue, maxSliderValue, clampedSliderValue);
            float targetX = Mathf.Lerp(minLocalX, maxLocalX, normalizedValue);

            sliderHandle.localPosition = new Vector3(targetX, originalLocalY, originalLocalZ);
            sliderHandle.localRotation = originalLocalRotation;
            lastSliderLocalX = targetX;
        }

        UpdateVisuals();
    }

    public void OpenKeyboard()
    {
        if (keyboardPanelPrefab == null) return;

        if (currentKeyboardInstance != null)
        {
            Destroy(currentKeyboardInstance);
            currentKeyboardInstance = null;
        }
        if(Slider != null)
        {
            Slider.SetActive(false);
        }

        Vector3 spawnPosition = keyboardSpawnPoint != null
            ? keyboardSpawnPoint.position
            : transform.position + transform.right * 0.35f;

        Quaternion spawnRotation = keyboardSpawnPoint != null
            ? keyboardSpawnPoint.rotation
            : transform.rotation;

        currentKeyboardInstance = Instantiate(keyboardPanelPrefab, spawnPosition, spawnRotation);
        currentKeyboardInstance.transform.localScale = keyboardSpawnScale;

        NumberKeyboardPanel keyboardPanel = currentKeyboardInstance.GetComponent<NumberKeyboardPanel>();
        if (keyboardPanel != null)
        {
            keyboardPanel.targetNumberBlock = this;
        }
    }

    public void CloseKeyboard()
    {
        if (currentKeyboardInstance != null)
        {
            Destroy(currentKeyboardInstance);
            currentKeyboardInstance = null;
        }
        if(Slider != null && sliderHandle != null)
        {
            Slider.SetActive(true);
        }
    }

    void SetInputMode(bool isEditable)
    {
        if (editButton != null)
        {
            editButton.SetActive(isEditable);
        }
    }

    private void OnDestroy()
    {
        CloseKeyboard();
    }

}
