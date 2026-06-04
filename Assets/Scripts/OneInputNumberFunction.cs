using UnityEngine;
using TMPro;

public class OneInputNumberFunction : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI visualText;

    [Header("Settings")]
    public string[] functions = { "SENO", "COSENO", "TANGENTE","ARCOSENO", "ARCOCOSENO", "ARCOTANGENTE" };

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

    public bool OutputsNumber()
    {
        return true;
    }

    public float GetCurrentResult()
    {
        if (input == null) return 0f;

        float value = input.GetValueFromSource();
        float resultado = 0f;

        switch (functions[actual])
        {
            case "SENO":
                resultado = Mathf.Sin(value * Mathf.Deg2Rad);
                break;

            case "COSENO":
                resultado = Mathf.Cos(value * Mathf.Deg2Rad);
                break;

            case "ARCOSENO":
                resultado = Mathf.Asin(Mathf.Clamp(value, -1f, 1f)) * Mathf.Rad2Deg;
                break;

            case "ARCOCOSENO":
                resultado = Mathf.Acos(Mathf.Clamp(value, -1f, 1f)) * Mathf.Rad2Deg;
                break;

            case "TANGENTE":
                resultado = Mathf.Tan(value * Mathf.Deg2Rad);
                break;

            case "ARCOTANGENTE":
                resultado = Mathf.Atan(value) * Mathf.Rad2Deg;
                break;

            default:
                resultado = 0f;
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
}