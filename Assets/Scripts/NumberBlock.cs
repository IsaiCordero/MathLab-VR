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



private GameObject currentKeyboardInstance;

    void Start()
    {
        UpdateVisuals();

        lastHadIncomingCable = incomingCable != null;
        SetInputMode(!lastHadIncomingCable);
    }

    void Update()
    {
        if(incomingCable != null)
        {
            float result = incomingCable.GetValueFromSource();
            currentValue = result;
            valueText.text = result.ToString("F1");
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
