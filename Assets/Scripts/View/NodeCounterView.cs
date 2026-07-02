using UnityEngine;
using TMPro;

public class NodeCounterView : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI counterText;

    void Update()
    {
        if (counterText == null) return;

        int nodeCount = NodeManager.Instance != null
            ? NodeManager.Instance.NodeCount
            : 0;

        counterText.text = "Nodos: " + nodeCount;
    }
}