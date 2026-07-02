using UnityEngine;

public class NumberKeyboardController : MonoBehaviour
{
    [Header("Keyboard")]
    public GameObject keyboardPanelPrefab;
    public Transform keyboardSpawnPoint;
    public Vector3 keyboardSpawnScale = Vector3.one * 0.15f;

    private GameObject currentKeyboardInstance;
    private NumberBlock targetNumberBlock;

    public void Initialize(NumberBlock target)
    {
        targetNumberBlock = target;
    }

    public void Open()
    {
        if (keyboardPanelPrefab == null || targetNumberBlock == null) return;

        Close();

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
            keyboardPanel.targetNumberBlock = targetNumberBlock;
        }
    }

    public void Close()
    {
        if (currentKeyboardInstance != null)
        {
            Destroy(currentKeyboardInstance);
            currentKeyboardInstance = null;
        }
    }
}