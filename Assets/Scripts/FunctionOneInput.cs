using UnityEngine;
using TMPro;

public class FunctionOneInput : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI visualText;

    [Header("Settings")]
    public string[] functions = { "MAGNITUD", "NORMALIZACIÓN", "OPUESTO", "ABSOLUTO" };

    [Header("Data Input")]
    public DataCable input;

    [Header("Visuals")]
    public Renderer blockRenderer;
    public Color vectorFunctionColor = new Color(0.11f, 0.65f, 0.85f); 
    public Color numberFunctionColor = new Color(0.85f, 0.55f, 0.00f);

    private int actual = 0;

    void Start()
    {
        if (visualText != null && functions.Length > 0)
        {
            visualText.text = functions[actual];
        }
        UpdateBlockColor();
    }
    public bool OutputsNumber()
    {
        return functions[actual] == "MAGNITUD";
    }

    public bool OutputsVector()
    {
        return !OutputsNumber();
    }   


    public float GetCurrentResult()
    {
        if (input == null) return 0f;

        Vector3 v = input.GetVectorFromSource();
        float resultado = 0f;

        switch (functions[actual])
        {
            case "MAGNITUD":
                resultado = v.magnitude;
                break;

            default:
                resultado = 0f;
                break;
        }

        return resultado;
    }

    public Vector3 GetCurrentVectorResult()
    {
        if (input == null) return Vector3.zero;

        Vector3 v = input.GetVectorFromSource();
        Vector3 resultado = Vector3.zero;

        switch (functions[actual])
        {
            case "NORMALIZACIÓN":
                resultado = (v != Vector3.zero) ? v.normalized : Vector3.zero;
                break;

            case "OPUESTO":
                resultado = -v;
                break;
            
            case "ABSOLUTO":
                resultado = new Vector3(
                    Mathf.Abs(v.x),
                    Mathf.Abs(v.y),
                    Mathf.Abs(v.z)
                );
                break;

            default:
                resultado = Vector3.zero;
                break;
        }

        return resultado;
    }


    public void SetFunctionByIndex(int index)
    {
        if (functions == null || functions.Length == 0) return;

        if (index < 0 || index >= functions.Length)
        {
            index = 0;
        }

        actual = index;

        if (visualText != null)
        {
            visualText.text = functions[actual];
        }
    }

    void UpdateBlockColor()
    {
        if (blockRenderer == null) return;

        Color targetColor = OutputsNumber() ? numberFunctionColor : vectorFunctionColor;
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
