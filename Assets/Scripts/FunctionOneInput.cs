using UnityEngine;
using TMPro;

public class FunctionOneInput : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI visualText;

    [Header("Settings")]
    public string[] functions = { "MAGNITUD", "NORMALIZACIÓN", "OPUESTO" };

    [Header("Data Input")]
    public DataCable input;

    private int actual = 0;

    void Start()
    {
        if (visualText != null && functions.Length > 0)
        {
            visualText.text = functions[actual];
        }
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

            default:
                resultado = Vector3.zero;
                break;
        }

        return resultado;
    }

    public void ChangeNextFunction()
    {
        actual++;

        if (actual >= functions.Length)
        {
            actual = 0;
        }

        if (visualText != null)
        {
            visualText.text = functions[actual];
        }
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
}
