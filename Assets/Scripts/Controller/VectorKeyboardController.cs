using UnityEngine;

public class VectorKeyboardController : MonoBehaviour
{
    [Header("Keyboard")]
    public GameObject keyboardPanelPrefab;
    public Transform keyboardSpawnPoint;
    public Vector3 keyboardSpawnScale = Vector3.one * 0.15f;

    private GameObject currentKeyboardInstance;
    private VectorBlock targetVectorBlock;

    public void Initialize(VectorBlock target)
    {
        targetVectorBlock = target;
    }

    public void Open()
    {
        if (keyboardPanelPrefab == null || targetVectorBlock == null) return;

        Close();

        Vector3 spawnPosition = keyboardSpawnPoint != null
            ? keyboardSpawnPoint.position
            : transform.position + transform.right * 0.35f;

        Quaternion spawnRotation = keyboardSpawnPoint != null
            ? keyboardSpawnPoint.rotation
            : transform.rotation;

        currentKeyboardInstance = Instantiate(keyboardPanelPrefab, spawnPosition, spawnRotation);
        currentKeyboardInstance.transform.localScale = keyboardSpawnScale;

        VectorKeyboardPanel keyboardPanel = currentKeyboardInstance.GetComponent<VectorKeyboardPanel>();
        if (keyboardPanel != null)
        {
            keyboardPanel.targetVectorBlock = targetVectorBlock;
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