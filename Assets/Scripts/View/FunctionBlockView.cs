using UnityEngine;
using TMPro;

public class FunctionBlockView : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI visualText;
    public TextMeshProUGUI firstInputText;
    public TextMeshProUGUI secondInputText;
    public TextMeshProUGUI outputText;

    [Header("Block Color")]
    public Renderer blockRenderer;
    public Color vectorFunctionColor = new Color(0.11f, 0.65f, 0.85f);
    public Color numberFunctionColor = new Color(0.85f, 0.55f, 0.00f);
    public Color neutralFunctionColor = Color.white;

    public void SetFunctionLabel(string label)
    {
        if (visualText != null)
        {
            visualText.text = label;
        }
    }
    
    public void SetSingleOutputLabel(string outputLabel)
    {
        if (outputText != null)
        {
            outputText.text = outputLabel;
        }
    }

    public void SetInputOutputLabels(string firstLabel, string secondLabel, string outputLabel)
    {
        if (firstInputText != null)
        {
            firstInputText.text = firstLabel;
        }

        if (secondInputText != null)
        {
            secondInputText.text = secondLabel;
        }

        if (outputText != null)
        {
            outputText.text = outputLabel;
        }
    }

    public void SetFunctionColor(NodeValueType outputType)
    {
        Color targetColor = outputType == NodeValueType.Number
            ? numberFunctionColor
            : vectorFunctionColor;

        SetBlockColor(targetColor);
    }

    public void SetNeutralFunctionColor()
    {
        SetBlockColor(neutralFunctionColor);
    }

    void SetBlockColor(Color targetColor)
    {
        if (blockRenderer == null) return;

        Material mat = blockRenderer.material;

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
            mat.SetColor("_EmissionColor", targetColor * 0.6f);
        }
    }
}